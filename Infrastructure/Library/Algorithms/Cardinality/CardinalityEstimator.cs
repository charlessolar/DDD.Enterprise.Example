// /*  
//     See https://github.com/Microsoft/CardinalityEstimation.
//     The MIT License (MIT)
// 
//     Copyright (c) 2015 Microsoft
// 
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
// 
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
// 
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
// */

namespace Demo.Library.Algorithms.Cardinality
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using Hash;
    /// <summary>
    ///     A cardinality estimator for sets of some common types, which uses a HashSet for small cardinalities,
    ///     LinearCounting for medium-range cardinalities and HyperLogLog for large cardinalities.  Based off of the following:
    ///     1. Flajolet et al., "HyperLogLog: the analysis of a near-optimal cardinality estimation algorithm",
    ///     DMTCS proc. AH 2007, <see cref="http://algo.inria.fr/flajolet/Publications/FlFuGaMe07.pdf" />
    ///     2. Heule, Nunkesser and Hall 2013, "HyperLogLog in Practice: Algorithmic Engineering of a State of The Art Cardinality Estimation
    ///     Algorithm",
    ///     <see cref="http://static.googleusercontent.com/external_content/untrusted_dlcp/research.google.com/en/us/pubs/archive/40671.pdf" />
    /// </summary>
    /// <remarks>
    ///     1. This implementation is not thread-safe
    ///     2. By default, it uses the 128-bit Murmur3 hash function, <see cref="http://github.com/darrenkopp/murmurhash-net"/>.
    ///        For legacy support, the c'tor also allows using the 64-bit Fowler/Noll/Vo-0 FNV-1a hash function, <see cref="http://www.isthe.com/chongo/src/fnv/hash_64a.c" />
    ///     3. Estimation is perfect up to 100 elements, then approximate
    /// </remarks>
    [Serializable]
    public class CardinalityEstimator : ICardinalityEstimator<string>, ICardinalityEstimator<int>, ICardinalityEstimator<uint>,
        ICardinalityEstimator<long>, ICardinalityEstimator<ulong>, ICardinalityEstimator<float>, ICardinalityEstimator<double>,
        ICardinalityEstimator<byte[]>
    {
        /// <summary> Number of bits for indexing HLL substreams - the number of estimators is 2^bitsPerIndex </summary>
        private readonly int _bitsPerIndex;

        /// <summary> Number of bits to compute the HLL estimate on </summary>
        private readonly int _bitsForHll;

        /// <summary> HLL lookup table size </summary>
        private readonly int _m;

        /// <summary> Fixed bias correction factor </summary>
        private readonly double _alphaM;

        /// <summary> Threshold determining whether to use LinearCounting or HyperLogLog based on an initial estimate </summary>
        private readonly double _subAlgorithmSelectionThreshold;

        /// <summary> Lookup table for the dense representation </summary>
        private byte[] _lookupDense;

        /// <summary> Lookup dictionary for the sparse representation </summary>
        private IDictionary<ushort, byte> _lookupSparse;

        /// <summary> Max number of elements to hold in the sparse representation </summary>
        private readonly int _sparseMaxElements;

        /// <summary> Indicates that the sparse representation is currently used </summary>
        private bool _isSparse;

        /// <summary> Set for direct counting of elements </summary>
        private HashSet<ulong> _directCount;

        /// <summary> Max number of elements to hold in the direct representation </summary>
        private const int DirectCounterMaxElements = 100;

        /// <summary> ID of hash function used </summary>
        private readonly HashFunctionId _hashFunctionId;

        /// <summary> Hash function used </summary>
        [NonSerialized]
        private IHashFunction _hashFunction;
        /// <summary>
        ///     C'tor
        /// </summary>
        /// <param name="b">
        ///     Number of bits determining accuracy and memory consumption, in the range [4, 16] (higher = greater accuracy and memory usage).
        ///     For large cardinalities, the standard error is 1.04 * 2^(-b/2), and the memory consumption is bounded by 2^b kilobytes.
        ///     The default value of 14 typically yields 3% error or less across the entire range of cardinalities (usually much less),
        ///     and uses up to ~16kB of memory.  b=4 yields less than ~100% error and uses less than 1kB. b=16 uses up to ~64kB and usually yields 1%
        ///     error or less
        /// </param>
        /// <param name="hashFunctionId">Type of hash function to use. Default is Murmur3, and FNV-1a is provided for legacy support</param>
        public CardinalityEstimator(int b = 14, HashFunctionId hashFunctionId = HashFunctionId.Murmur3) : this(CreateEmptyState(b, hashFunctionId)) { }

        /// <summary>
        ///     Creates a CardinalityEstimator with the given <paramref name="state" />
        /// </summary>
        internal CardinalityEstimator(CardinalityEstimatorState state)
        {
            this._bitsPerIndex = state.BitsPerIndex;
            this._bitsForHll = 64 - this._bitsPerIndex;
            this._m = (int)Math.Pow(2, this._bitsPerIndex);
            this._alphaM = GetAlphaM(this._m);
            this._subAlgorithmSelectionThreshold = GetSubAlgorithmSelectionThreshold(this._bitsPerIndex);

            // Init the hash function
            this._hashFunctionId = state.HashFunctionId;
            this._hashFunction = HashFunctionFactory.GetHashFunction(this._hashFunctionId);

            // Init the direct count
            this._directCount = state.DirectCount != null ? new HashSet<ulong>(state.DirectCount) : null;

            // Init the sparse representation
            this._isSparse = state.IsSparse;
            this._lookupSparse = state.LookupSparse != null ? new Dictionary<ushort, byte>(state.LookupSparse) : null;
            this._lookupDense = state.LookupDense;
            this.CountAdditions = state.CountAdditions;

            // Each element in the sparse representation takes 15 bytes, and there is some constant overhead
            this._sparseMaxElements = Math.Max(0, this._m / 15 - 10);
            // If necessary, switch to the dense representation
            if (this._sparseMaxElements <= 0)
            {
                SwitchToDenseRepresentation();
            }

            // if DirectCount is not null, populate the HLL lookup with its elements.  This allows serialization to include only directCount
            if (this._directCount != null)
            {
                foreach (ulong element in this._directCount)
                {
                    AddElementHash(element);
                }
            }
            else
            {
                this._directCount = null;
            }
        }

        public ulong CountAdditions { get; private set; }

        public void Add(string element)
        {
            ulong hashCode = GetHashCode(Encoding.UTF8.GetBytes(element));
            AddElementHash(hashCode);
            this.CountAdditions++;
        }

        public void Add(int element)
        {
            ulong hashCode = GetHashCode(BitConverter.GetBytes(element));
            AddElementHash(hashCode);
            this.CountAdditions++;
        }

        public void Add(uint element)
        {
            ulong hashCode = GetHashCode(BitConverter.GetBytes(element));
            AddElementHash(hashCode);
            this.CountAdditions++;
        }

        public void Add(long element)
        {
            ulong hashCode = GetHashCode(BitConverter.GetBytes(element));
            AddElementHash(hashCode);
            this.CountAdditions++;
        }

        public void Add(ulong element)
        {
            ulong hashCode = GetHashCode(BitConverter.GetBytes(element));
            AddElementHash(hashCode);
            this.CountAdditions++;
        }

        public void Add(float element)
        {
            ulong hashCode = GetHashCode(BitConverter.GetBytes(element));
            AddElementHash(hashCode);
            this.CountAdditions++;
        }

        public void Add(double element)
        {
            ulong hashCode = GetHashCode(BitConverter.GetBytes(element));
            AddElementHash(hashCode);
            this.CountAdditions++;
        }

        public void Add(byte[] element)
        {
            ulong hashCode = GetHashCode(element);
            AddElementHash(hashCode);
            this.CountAdditions++;
        }


        public ulong Count()
        {
            // If only a few elements have been seen, return the exact count
            if (this._directCount != null)
            {
                return (ulong)this._directCount.Count;
            }

            double zInverse = 0;
            double v = 0;

            if (this._isSparse)
            {
                // calc c and Z's inverse
                foreach (KeyValuePair<ushort, byte> kvp in this._lookupSparse)
                {
                    byte sigma = kvp.Value;
                    zInverse += Math.Pow(2, -sigma);
                }
                v = this._m - this._lookupSparse.Count;
                zInverse += (this._m - this._lookupSparse.Count);
            }
            else
            {
                // calc c and Z's inverse
                for (var i = 0; i < this._m; i++)
                {
                    byte sigma = this._lookupDense[i];
                    zInverse += Math.Pow(2, -sigma);
                    if (sigma == 0)
                    {
                        v++;
                    }
                }
            }

            double e = this._alphaM * this._m * this._m / zInverse;
            if (e <= 5.0 * this._m)
            {
                e = BiasCorrection.CorrectBias(e, this._bitsPerIndex);
            }

            double h;
            if (v > 0)
            {
                // LinearCounting estimate
                h = this._m * Math.Log(this._m / v);
            }
            else
            {
                h = e;
            }

            if (h <= this._subAlgorithmSelectionThreshold)
            {
                return (ulong)Math.Round(h);
            }
            return (ulong)Math.Round(e);
        }

        /// <summary>
        ///     Merges the given <paramref name="other" /> CardinalityEstimator instance into this one
        /// </summary>
        /// <param name="other">another instance of CardinalityEstimator</param>
        public void Merge(CardinalityEstimator other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (other._m != this._m)
            {
                throw new ArgumentOutOfRangeException(nameof(other),
                    "Cannot merge CardinalityEstimator instances with different accuracy/map sizes");
            }

            this.CountAdditions += other.CountAdditions;
            if (this._isSparse && other._isSparse)
            {
                // Merge two sparse instances
                foreach (KeyValuePair<ushort, byte> kvp in other._lookupSparse)
                {
                    ushort index = kvp.Key;
                    byte otherRank = kvp.Value;
                    byte thisRank;
                    this._lookupSparse.TryGetValue(index, out thisRank);
                    this._lookupSparse[index] = Math.Max(thisRank, otherRank);
                }

                // Switch to dense if necessary
                if (this._lookupSparse.Count > this._sparseMaxElements)
                {
                    SwitchToDenseRepresentation();
                }
            }
            else
            {
                // Make sure this (target) instance is dense, then merge
                SwitchToDenseRepresentation();
                if (other._isSparse)
                {
                    foreach (KeyValuePair<ushort, byte> kvp in other._lookupSparse)
                    {
                        ushort index = kvp.Key;
                        byte rank = kvp.Value;
                        this._lookupDense[index] = Math.Max(this._lookupDense[index], rank);
                    }
                }
                else
                {
                    for (var i = 0; i < this._m; i++)
                    {
                        this._lookupDense[i] = Math.Max(this._lookupDense[i], other._lookupDense[i]);
                    }
                }
            }

            if (other._directCount != null)
            {
                // Other instance is using direct counter. If this instance is also using direct counter, merge them.
                _directCount?.UnionWith(other._directCount);
            }
            else
            {
                // Other instance is not using direct counter, make sure this instance doesn't either
                this._directCount = null;
            }
        }

        /// <summary>
        ///     Merges the given CardinalityEstimator instances and returns the result
        /// </summary>
        /// <param name="estimators">Instances of CardinalityEstimator</param>
        /// <returns>The merged CardinalityEstimator</returns>
        public static CardinalityEstimator Merge(IList<CardinalityEstimator> estimators)
        {
            if (!estimators.Any())
            {
                throw new ArgumentException($"Was asked to merge 0 instances of {typeof(CardinalityEstimator)}",
                    nameof(estimators));
            }

            var ans = new CardinalityEstimator(estimators[0]._bitsPerIndex);
            foreach (CardinalityEstimator estimator in estimators)
            {
                ans.Merge(estimator);
            }

            return ans;
        }

        internal CardinalityEstimatorState GetState()
        {
            return new CardinalityEstimatorState
            {
                BitsPerIndex = this._bitsPerIndex,
                DirectCount = this._directCount,
                IsSparse = this._isSparse,
                LookupDense = this._lookupDense,
                LookupSparse = this._lookupSparse,
                HashFunctionId = this._hashFunctionId,
                CountAdditions = this.CountAdditions,
            };
        }

        /// <summary>
        ///     Creates state for an empty CardinalityEstimator : DirectCount and LookupSparse are empty, LookupDense is null.
        /// </summary>
        /// <param name="b"><see cref="CardinalityEstimator(int, HashFunctionId)" /></param>
        /// <param name="hashFunctionId"><see cref="CardinalityEstimator(int, HashFunctionId)" /></param>
        private static CardinalityEstimatorState CreateEmptyState(int b, HashFunctionId hashFunctionId)
        {
            if (b < 4 || b > 16)
            {
                throw new ArgumentOutOfRangeException(nameof(b), b, "Accuracy out of range, legal range is 4 <= BitsPerIndex <= 16");
            }

            return new CardinalityEstimatorState
            {
                BitsPerIndex = b,
                DirectCount = new HashSet<ulong>(),
                IsSparse = true,
                LookupSparse = new Dictionary<ushort, byte>(),
                LookupDense = null,
                HashFunctionId = hashFunctionId,
                CountAdditions = 0,
            };
        }

        /// <summary>
        ///     Returns the threshold determining whether to use LinearCounting or HyperLogLog for an estimate. Values are from the supplementary
        ///     material of Huele et al.,
        ///     <see cref="http://docs.google.com/document/d/1gyjfMHy43U9OWBXxfaeG-3MjGzejW1dlpyMwEYAAWEI/view?fullscreen#heading=h.nd379k1fxnux" />
        /// </summary>
        /// <param name="bits">Number of bits</param>
        /// <returns></returns>
        private static double GetSubAlgorithmSelectionThreshold(int bits)
        {
            switch (bits)
            {
                case 4:
                    return 10;
                case 5:
                    return 20;
                case 6:
                    return 40;
                case 7:
                    return 80;
                case 8:
                    return 220;
                case 9:
                    return 400;
                case 10:
                    return 900;
                case 11:
                    return 1800;
                case 12:
                    return 3100;
                case 13:
                    return 6500;
                case 14:
                    return 11500;
                case 15:
                    return 20000;
                case 16:
                    return 50000;
                case 17:
                    return 120000;
                case 18:
                    return 350000;
            }
            throw new ArgumentOutOfRangeException(nameof(bits), "Unexpected number of bits (should never happen)");
        }

        /// <summary>
        ///     Adds an element's hash code to the counted set
        /// </summary>
        /// <param name="hashCode">Hash code of the element to add</param>
        private void AddElementHash(ulong hashCode)
        {
            if (this._directCount != null)
            {
                this._directCount.Add(hashCode);
                if (this._directCount.Count > DirectCounterMaxElements)
                {
                    this._directCount = null;
                }
            }

            var substream = (ushort)(hashCode >> this._bitsForHll);
            byte sigma = GetSigma(hashCode, this._bitsForHll);
            if (this._isSparse)
            {
                byte prevRank;
                this._lookupSparse.TryGetValue(substream, out prevRank);
                this._lookupSparse[substream] = Math.Max(prevRank, sigma);
                if (this._lookupSparse.Count > this._sparseMaxElements)
                {
                    SwitchToDenseRepresentation();
                }
            }
            else
            {
                this._lookupDense[substream] = Math.Max(this._lookupDense[substream], sigma);
            }
        }

        /// <summary>
        ///     Gets the appropriate value of alpha_M for the given <paramref name="m" />
        /// </summary>
        /// <param name="m">size of the lookup table</param>
        /// <returns>alpha_M for bias correction</returns>
        private static double GetAlphaM(int m)
        {
            switch (m)
            {
                case 16:
                    return 0.673;
                case 32:
                    return 0.697;
                case 64:
                    return 0.709;
                default:
                    return 0.7213 / (1 + 1.079 / m);
            }
        }

        /// <summary>
        ///     Returns the base-2 logarithm of <paramref name="x" />.
        ///     This implementation is faster than <see cref="Math.Log(double,double)" /> as it avoids input checks
        /// </summary>
        /// <param name="x"></param>
        /// <returns>The base-2 logarithm of <paramref name="x" /></returns>
        public static double Log2(double x)
        {
            const double ln2 = 0.693147180559945309417232121458;
            return Math.Log(x) / ln2;
        }

        /// <summary>
        ///     Computes the hash of the given <paramref name="bytes" />
        /// </summary>
        /// <param name="bytes">data to compute the hash for</param>
        /// <returns>The hash code of <paramref name="bytes"/></returns>
        private ulong GetHashCode(byte[] bytes)
        {
            return this._hashFunction.GetHashCode(bytes);
        }

        /// <summary>
        ///     Returns the number of leading zeroes in the <paramref name="bitsToCount" /> highest bits of <paramref name="hash" />, plus one
        /// </summary>
        /// <param name="hash">Hash value to calculate the statistic on</param>
        /// <param name="bitsToCount">Lowest bit to count from <paramref name="hash" /></param>
        /// <returns>The number of leading zeroes in the binary representation of <paramref name="hash" />, plus one</returns>
        internal static byte GetSigma(ulong hash, int bitsToCount)
        {
            byte sigma = 1;
            for (int i = bitsToCount - 1; i >= 0; --i)
            {
                if (((hash >> i) & 1) == 0)
                {
                    sigma++;
                }
                else
                {
                    break;
                }
            }
            return sigma;
        }

        /// <summary>
        ///     Converts this estimator from the sparse to the dense representation
        /// </summary>
        private void SwitchToDenseRepresentation()
        {
            if (!this._isSparse)
            {
                return;
            }

            this._lookupDense = new byte[this._m];
            foreach (KeyValuePair<ushort, byte> kvp in this._lookupSparse)
            {
                int index = kvp.Key;
                this._lookupDense[index] = kvp.Value;
            }
            this._lookupSparse = null;
            this._isSparse = false;
        }

        [OnDeserialized]
        internal void SetHashFunctionAfterDeserializing(StreamingContext context)
        {
            this._hashFunction = HashFunctionFactory.GetHashFunction(this._hashFunctionId);
        }
    }
}