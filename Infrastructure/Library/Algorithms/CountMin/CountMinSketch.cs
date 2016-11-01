using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Demo.Library.Algorithms.CountMin
{
    /// <summary>
    /// CountMinSketch implements a Count-Min Sketch as described by Cormode and
    /// Muthukrishnan in An Improved Data Stream Summary: The Count-Min Sketch and its
    /// Applications:
    ///
    /// http://dimacs.rutgers.edu/~graham/pubs/papers/cm-full.pdf
    ///
    /// A Count-Min Sketch (CMS) is a probabilistic data structure which approximates
    /// the frequency of events in a data stream. Unlike a hash map, a CMS uses
    /// sub-linear space at the expense of a configurable error factor. Similar to
    /// Counting Bloom filters, items are hashed to a series of buckets, which increment
    /// a counter. The frequency of an item is estimated by taking the minimum of each of
    /// the item's respective counter values.
    ///
    /// Count-Min Sketches are useful for counting the frequency of events in massive
    /// data sets or unbounded streams online. In these situations, storing the entire
    /// data set or allocating counters for every event in memory is impractical. It may
    /// be possible for offline processing, but real-time processing requires fast,
    /// space-efficient solutions like the CMS. For approximating set cardinality, refer
    /// to the HyperLogLog.
    /// </summary>
    public class CountMinSketch
    {
        /// <summary>
        /// Count matrix
        /// </summary>
        internal ulong[][] Matrix { get; set; }
        private readonly object _lock = new object();
        /// <summary>
        /// Matrix width
        /// </summary>
        internal uint Width { get; set; }
        /// <summary>
        /// Matrix depth
        /// </summary>
        internal uint Depth { get; set; }
        /// <summary>
        /// Number of items added
        /// </summary>
        private ulong count { get; set; }
        /// <summary>
        /// Relative-accuracy factor
        /// </summary>
        private double epsilon { get; set; }
        /// <summary>
        /// Relative-accuracy probability
        /// </summary>
        private double delta { get; set; }
        /// <summary>
        /// Hash function
        /// </summary>
        private HashAlgorithm Hash { get; set; }

        /// <summary>
        /// Creates a new Count-Min Sketch whose relative accuracy is within a factor of
        /// epsilon with probability delta. Both of these parameters affect the space and
        /// time complexity.
        /// </summary>
        /// <param name="epsilon">Relative-accuracy factor</param>
        /// <param name="delta">Relative-accuracy probability</param>
        public CountMinSketch(double epsilon = 0.001, double delta = 0.99)
        {
            var width = (uint)(Math.Ceiling(Math.E / epsilon));
            var depth = (uint)(Math.Ceiling(Math.Log(1 / delta)));
            this.Matrix = new ulong[depth][];

            lock (_lock)
            {
                for (int i = 0; i < depth; i++)
                {
                    this.Matrix[i] = new ulong[width];
                }
            }

            this.Width = width;
            this.Depth = depth;
            this.epsilon = epsilon;
            this.delta = delta;
            this.Hash = Defaults.GetDefaultHashAlgorithm();
        }
        internal CountMinSketch(CountMinSketchState state)
        {
            this.epsilon = state.Epsilon;
            this.delta = state.Delta;
            this.Width = (uint)(Math.Ceiling(Math.E / state.Epsilon));
            this.Depth = (uint)(Math.Ceiling(Math.Log(1 / state.Delta)));
            lock (_lock)
            {
                this.Matrix = state.Matrix;
            }
            this.Hash = Defaults.GetDefaultHashAlgorithm();
        }

        /// <summary>
        /// Returns the relative-accuracy factor, epsilon.
        /// </summary>
        /// <returns>The relative-accuracy factor, epsilon</returns>
        public double Epsilon()
        {
            return this.epsilon;
        }

        /// <summary>
        /// Returns the relative-accuracy probability, delta.
        /// </summary>
        /// <returns>The relative-accuracy probability, delta</returns>
        public double Delta()
        {
            return this.delta;
        }

        /// <summary>
        /// Returns the number of items added to the sketch.
        /// </summary>
        /// <returns>The number of items added to the sketch.</returns>
        public ulong TotalCount()
        {
            return this.count;
        }

        public ulong Add(string element)
        {
            return Add(Encoding.UTF8.GetBytes(element));
        }

        public ulong Add(int element)
        {
            return Add(BitConverter.GetBytes(element));
        }

        public ulong Add(uint element)
        {
            return Add(BitConverter.GetBytes(element));
        }

        public ulong Add(long element)
        {
            return Add(BitConverter.GetBytes(element));
        }

        public ulong Add(ulong element)
        {
            return Add(BitConverter.GetBytes(element));
        }

        public ulong Add(float element)
        {
            return Add(BitConverter.GetBytes(element));
        }

        public ulong Add(double element)
        {
            return Add(BitConverter.GetBytes(element));
        }
        /// <summary>
        /// Add the data to the set. Returns the new count of added element
        /// </summary>
        /// <param name="data">The data to add.</param>
        /// <returns>The CountMinSketch</returns>
        public ulong Add(byte[] data)
        {
            var hashKernel = Utils.HashKernel(data, this.Hash);
            var lower = hashKernel.LowerBaseHash;
            var upper = hashKernel.UpperBaseHash;

            lock (_lock)
            {
                // Increment count in each row.
                for (uint i = 0; i < this.Depth; i++)
                {
                    this.Matrix[i][(lower + upper * i) % this.Width]++;
                }
            }

            this.count++;
            return Count(data);
        }
        public ulong Remove(byte[] data)
        {
            var hashKernel = Utils.HashKernel(data, this.Hash);
            var lower = hashKernel.LowerBaseHash;
            var upper = hashKernel.UpperBaseHash;

            lock (_lock)
            {
                // Increment count in each row.
                for (uint i = 0; i < this.Depth; i++)
                {
                    this.Matrix[i][(lower + upper * i) % this.Width]--;
                }
            }

            this.count--;
            return Count(data);
        }

        public ulong Remove(string element)
        {
            return Remove(Encoding.UTF8.GetBytes(element));
        }

        public ulong Remove(int element)
        {
            return Remove(BitConverter.GetBytes(element));
        }

        public ulong Remove(uint element)
        {
            return Remove(BitConverter.GetBytes(element));
        }

        public ulong Remove(long element)
        {
            return Remove(BitConverter.GetBytes(element));
        }

        public ulong Remove(ulong element)
        {
            return Remove(BitConverter.GetBytes(element));
        }

        public ulong Remove(float element)
        {
            return Remove(BitConverter.GetBytes(element));
        }

        public ulong Remove(double element)
        {
            return Remove(BitConverter.GetBytes(element));
        }
        /// <summary>
        /// Returns the approximate count for the specified item, correct within
        /// epsilon * total count with a probability of delta.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The data to count.</returns>
        public ulong Count(byte[] data)
        {
            var hashKernel = Utils.HashKernel(data, this.Hash);
            var lower = hashKernel.LowerBaseHash;
            var upper = hashKernel.UpperBaseHash;
            var count = ulong.MaxValue;

            for (uint i = 0; i < this.Depth; i++)
            {
                count = Math.Min(count, this.Matrix[i][(lower + upper * i) % this.Width]);
            }

            return count;
        }
        public ulong Count(string element)
        {
            return Count(Encoding.UTF8.GetBytes(element));
        }

        public ulong Count(int element)
        {
            return Count(BitConverter.GetBytes(element));
        }

        public ulong Count(uint element)
        {
            return Count(BitConverter.GetBytes(element));
        }

        public ulong Count(long element)
        {
            return Count(BitConverter.GetBytes(element));
        }

        public ulong Count(ulong element)
        {
            return Count(BitConverter.GetBytes(element));
        }

        public ulong Count(float element)
        {
            return Count(BitConverter.GetBytes(element));
        }

        public ulong Count(double element)
        {
            return Count(BitConverter.GetBytes(element));
        }

        /// <summary>
        /// Combines this CountMinSketch with another. Returns a bool if the merge was
        /// successful. Throws an exception if the matrix width and depth are not equal.
        /// </summary>
        /// <param name="other">The CountMinSketch to merge with the current
        /// instance.</param>
        /// <returns>True if successful.</returns>
        public bool Merge(CountMinSketch other)
        {
            if (this.Depth != other.Depth)
            {
                throw new Exception("Matrix depth must match.");
            }

            if (this.Width != other.Width)
            {
                throw new Exception("Matrix width must match.");
            }

            lock (_lock)
            {
                for (uint i = 0; i < this.Depth; i++)
                {
                    for (int j = 0; j < this.Width; j++)
                    {
                        this.Matrix[i][j] += other.Matrix[i][j];
                    }
                }
            }

            this.count += other.count;
            return true;
        }

        /// <summary>
        /// Restores the CountMinSketch to its original state. It returns itself to allow
        /// for chaining.
        /// </summary>
        /// <returns>The CountMinSketch</returns>
        public void Reset()
        {
            lock (_lock)
            {
                this.Matrix = new ulong[this.Depth][];
                for (uint i = 0; i < this.Depth; i++)
                {
                    this.Matrix[i] = new ulong[this.Width];
                }
            }

            this.count = 0;
        }

        /// <summary>
        /// Sets the hashing function used in the filter.
        /// </summary>
        /// <param name="h">The HashAlgorithm to use.</param>
        public void SetHash(HashAlgorithm h)
        {
            this.Hash = h;
        }


        internal CountMinSketchState GetState()
        {
            lock (_lock)
            {
                var matrix = this.Matrix.Select(a => a.ToArray()).ToArray();
                return new CountMinSketchState
                {
                    Epsilon = this.epsilon,
                    Delta = this.delta,
                    Count = this.count,
                    Matrix = this.Matrix
                };
            }
        }
    }
}
