using System;
using System.Linq;
using System.Security.Cryptography;

namespace Demo.Library.Algorithms.Bloom
{
    /// <summary>
    /// CuckooFilter implements a Cuckoo Bloom filter as described by Andersen, Kaminsky,
    /// and Mitzenmacher in Cuckoo Filter: Practically Better Than Bloom:
    ///
    /// http://www.pdl.cmu.edu/PDL-FTP/FS/cuckoo-conext2014.pdf
    ///
    /// A Cuckoo Filter is a Bloom filter variation which provides support for removing
    /// elements without significantly degrading space and performance. It works by using
    /// a cuckoo hashing scheme for inserting items. Instead of storing the elements
    /// themselves, it stores their fingerprints which also allows for item removal
    /// without false negatives (if you don't attempt to remove an item not contained in
    /// the filter).
    ///
    /// For applications that store many items and target moderately low false-positive
    /// rates, cuckoo filters have lower space overhead than space-optimized Bloom filters.
    /// </summary>
    public class CuckooBloomFilter 
    {
        /// <summary>
        /// The maximum number of relocations to attempt when inserting an element before
        /// considering the filter full.
        /// </summary>
        private const int MaxNumKicks = 500;

        internal byte[][][] Buckets { get; set; }
        /// <summary>
        /// Hash algorithm.
        /// </summary>
        private HashAlgorithm Hash { get; set; }
        /// <summary>
        /// Number of buckets
        /// </summary>
        private uint M { get; set; }
        /// <summary>
        /// Number of entries per bucket
        /// </summary>
        internal uint B { get; set; }
        /// <summary>
        /// Length of fingerprints (in bytes)
        /// </summary>
        private uint F { get; set; }
        /// <summary>
        /// Number of items in the filter
        /// </summary>
        private uint count { get; set; }
        /// <summary>
        /// Filter capacity
        /// </summary>
        private uint N { get; set; }

        private readonly FastRandom _random = new FastRandom();

        /// <summary>
        /// Creates a new Cuckoo Bloom filter optimized to store n items with a specified
        /// target false-positive rate.
        /// </summary>
        /// <param name="n">Number of items to store</param>
        /// <param name="fpRate">Target false-positive rate</param>
        public CuckooBloomFilter(uint n, double fpRate)
        {
            var b = (uint)4;
            var f = CalculateF(b, fpRate);
            var m = Power2(n / f * 8);
            var buckets = new byte[m][][];

            for (uint i = 0; i < m; i++)
            {
                buckets[i] = new byte[b][];
            }

            this.Buckets = buckets;
            this.Hash = Defaults.GetDefaultHashAlgorithm();
            this.M = m;
            this.B = b;
            this.F = f;
            this.N = n;
        }

        /// <summary>
        /// Returns the number of buckets.
        /// </summary>
        /// <returns>The number of buckets</returns>
        public uint BucketCount()
        {
            return this.M;
        }

        /// <summary>
        /// Returns the number of items the filter can store.
        /// </summary>
        /// <returns>The number of items the filter can store</returns>
        public uint Capacity()
        {
            return this.N;
        }

        /// <summary>
        /// Returns the number of items in the filter.
        /// </summary>
        /// <returns>The number of items in the filter</returns>
        public uint Count()
        {
            return this.count;
        }

        /// <summary>
        /// Will test for membership of the data and returns true if it is a member,
        /// false if not. This is a probabilistic test, meaning there is a non-zero
        /// probability of false positives.
        /// </summary>
        /// <param name="data">The data to test for</param>
        /// <returns>Whether or not the data is a member</returns>
        public bool Test(byte[] data)
        {
            var components = this.GetComponents(data);
            var i1 = components.Hash1;
            var i2 = components.Hash2;
            var f = components.Fingerprint;

            // If either bucket containsf, it's a member.
            var b1 = this.Buckets[i1 % this.M];
            foreach (var sequence in b1)
            {
                if (sequence != null)
                    if (sequence.SequenceEqual(f))
                        return true;
            }
            var b2 = this.Buckets[i2 % this.M];
            foreach (var sequence in b2)
            {
                if (sequence != null)
                    if (sequence.SequenceEqual(f))
                        return true;
            }
            return false;
        }

        /// <summary>
        /// Will add the data to the Cuckoo Filter. It returns false if the filter is
        /// full. If the filter is full, an item is removed to make room for the new
        /// item. This introduces a possibility for false negatives. To avoid this, use
        /// Count and Capacity to check if the filter is full before adding an item.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// True if the add was successful. False if the filter is full.
        /// </returns>
        public bool Add(byte[] data)
        {
            var components = this.GetComponents(data);
            var i1 = components.Hash1;
            var i2 = components.Hash2;
            var f = components.Fingerprint;
            return this.Insert(i1, i2, f);
        }

        /// <summary>
        /// Equivalent to calling Test followed by Add. It returns (true, false) if the
        /// data is a member, (false, add()) if not. False is returned if the filter is
        /// full. If the filter is full, an item is removed to make room for the new
        /// item. This introduces a possibility for false negatives. To avoid this, use
        /// Count and Capacity to check if the filter is full before adding an item.
        /// </summary>
        /// <returns>
        /// (true, false) if the data is a member, (false, add()) if not
        /// </returns>
        public TestAndAddReturnValue TestAndAdd(byte[] data)
        {
            var components = this.GetComponents(data);
            var i1 = components.Hash1;
            var i2 = components.Hash2;
            var f = components.Fingerprint;

            // If either bucket contains f, it's a member.
            var b1 = this.Buckets[i1 % this.M];
            foreach (var sequence in b1)
            {
                if (sequence != null)
                { 
                    if (sequence.SequenceEqual(f))
                    {
                        return TestAndAddReturnValue.Create(true, false);
                    }
                }
            }
            var b2 = this.Buckets[i2 % this.M];
            foreach (var sequence in b2)
            {
                if (sequence != null)
                {
                    if (sequence.SequenceEqual(f))
                    {
                        return TestAndAddReturnValue.Create(true, false);
                    }
                }
            }

            return TestAndAddReturnValue.Create(false, this.Insert(i1, i2, f));
        }

        /// <summary>
        /// Will test for membership of the data and remove it from the filter if it
        /// exists. Returns true if the data was a member, false if not.
        /// </summary>
        /// <param name="data">Data to test for and remove</param>
        /// <returns>Whether the data was a member or not</returns>
        public bool TestAndRemove(byte[] data)
        {
            var components = this.GetComponents(data);
            var i1 = components.Hash1;
            var i2 = components.Hash2;
            var f = components.Fingerprint;

            // Try to remove from bucket[i1].
            var b1 = this.Buckets[i1 % this.M];
            var idx = IndexOf(b1, f);
            if (idx != -1)
            {
                b1[idx] = null;
                this.count--;
                return true;
            }

            // Try to remove from bucket[i2].
            var b2 = this.Buckets[i2 % this.M];
            idx = IndexOf(b2, f);
            if (idx != -1)
            {
                b2[idx] = null;
                this.count--;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Restores the Bloom filter to its original state. It returns the filter to
        /// allow for chaining.
        /// </summary>
        /// <returns>The CuckooBloomFilter</returns>
        public CuckooBloomFilter Reset()
        {
            var buckets = new byte[this.M][][];
            for (uint i = 0; i < this.M; i++)
            {
                buckets[i] = new byte[this.B][];
            }
            this.Buckets = buckets;
            this.count = 0;
            return this;
        }

        /// <summary>
        /// Sets the hashing function used in the filter.
        /// </summary>
        /// <param name="h">The HashAlgorithm to use.</param>
        public void SetHash(HashAlgorithm h)
        {
            this.Hash = h;
        }

        /// <summary>
        /// Indicates if the given fingerprint is contained in one of the bucket's
        /// entries.
        /// </summary>
        /// <param name="f">Fingerprint</param>
        /// <returns>
        /// Whether or not the fingerprint is contained in one of the bucket's entries.
        /// </returns>
        private static bool Contains(byte[][] bucket, byte[] f)
        {
            return IndexOf(bucket, f) != 1;
        }

        /// <summary>
        /// Returns the entry index of the given fingerprint or -1 if it's not in the
        /// bucket.
        /// </summary>
        /// <param name="f">Fingerprint</param>
        /// <returns>The entry index of the fingerprint or -1 if it's not in the
        /// bucket</returns>
        private static int IndexOf(byte[][] bucket, byte[] f)
        {
            for (int i = 0; i < bucket.Length; i++)
            {
                var sequence = bucket[i];
                if (sequence != null)
                {
                    if (f.SequenceEqual(bucket[i]))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns the index of the next available entry in the bucket or -1 if it's
        /// full.
        /// </summary>
        /// <returns></returns>
        private static int GetEmptyEntry(byte[][] bucket)
        {
            for (int i = 0; i < bucket.Length; i++)
            {
                if (bucket[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Will insert the fingerprint into the filter returning false if the filter is
        /// full.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <param name="f"></param>
        /// <returns>
        /// True if the insert was successful. False if the filter is full
        /// </returns>
        private bool Insert(uint i1, uint i2, byte[] f)
        {
            // Try to insert into bucket[i1].
            var b1 = this.Buckets[i1 % this.M];
            var idx = GetEmptyEntry(b1);
            if (idx != -1)
            {
                b1[idx] = f;
                this.count++;
                return true;
            }

            // Try to insert into bucket[i2].
            var b2 = this.Buckets[i2 % this.M];
            var ids = GetEmptyEntry(b2);
            if (idx != -1)
            {
                b2[idx] = f;
                this.count++;
                return true;
            }

            // Must relocate existing items.
            var i = i1;
            for (int n = 0; n < MaxNumKicks; n++)
            {
                var bucketIdx = i % this.M;
                var entryIdx = _random.Next((int)this.B);
                var tempF = f;
                f = this.Buckets[bucketIdx][entryIdx];
                this.Buckets[bucketIdx][entryIdx] = tempF;
                i = i ^ ComputeHashSum32(f);
                var b = this.Buckets[i % this.M];

                idx = GetEmptyEntry(b);
                if (idx != -1)
                {
                    b[idx] = f;
                    this.count++;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the two hash values used to index into the buckets and the
        /// fingerprint for the given element.
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>The two hash values used to index into the buckets and the
        /// fingerprint for the given data</returns>
        private Components GetComponents(byte[] data)
        {
            var hash = this.ComputeHash(data);
            var f = hash.Take((int)this.F).ToArray();
            var i1 = this.ComputeHashSum32(hash);
            var i2 = this.ComputeHashSum32(f);

            return Components.Create(f, i1, i2);
        }

        /// <summary>
        /// Returns a 32-bit hash value for the given data.
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>32-bit hash value</returns>
        private byte[] ComputeHash(byte[] data)
        {
            var hash = new Hash(this.Hash);
            hash.ComputeHash(data);
            var sum = hash.Sum();
            return sum;
        }

        /// <summary>
        /// Returns the sum of the hash.
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>32-bit hash value</returns>
        private uint ComputeHashSum32(byte[] data)
        {
            var hash = new Hash(this.Hash);
            hash.ComputeHash(data);
            var sum = hash.Sum();
            return Utils.ToBigEndianUInt32(sum);
        }

        /// <summary>
        /// Returns the optimal fingerprint length in bytes for the given bucket size and
        /// false-positive rate epsilon.
        /// </summary>
        /// <param name="b">Bucket size</param>
        /// <param name="epsilon">False positive rate</param>
        /// <returns>The optimal fingerprint length</returns>
        private static uint CalculateF(uint b, double epsilon)
        {
            var f = (uint)Math.Ceiling(Math.Log(2 * b / epsilon));
            f = f / 8;
            if (f <= 0)
            {
                f = 1;
            }
            return f;
        }

        /// <summary>
        /// Calculates the next power of two for the given value.
        /// </summary>
        /// <param name="x">Value</param>
        /// <returns>The next power of two for the given value</returns>
        private static uint Power2(uint x)
        {
            x--;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x |= x >> 32;
            x++;
            return x;
        }

        private struct Components
        {
            internal byte[] Fingerprint;
            internal uint Hash1;
            internal uint Hash2;

            internal static Components Create(byte[] fingerprint, uint hash1, uint hash2)
            {
                return new Components
                {
                    Fingerprint = fingerprint,
                    Hash1 = hash1,
                    Hash2 = hash2
                };
            }
        }

        public struct TestAndAddReturnValue
        {
            public bool WasAlreadyAMember { get; private set; }
            public bool Added { get; private set; }

            internal static TestAndAddReturnValue Create(bool wasAlreadyAMember, bool added)
            {
                return new TestAndAddReturnValue
                {
                    WasAlreadyAMember = wasAlreadyAMember,
                    Added = added
                };
            }
        }
    }
}
