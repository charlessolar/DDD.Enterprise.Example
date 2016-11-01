/*
Original work Copyright (c) 2013 zhenjl
Modified work Copyright (c) 2015 Tyler Treat
Modified work Copyright (c) 2015 Matthew Lorimor

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
*/

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Demo.Library.Algorithms.Bloom
{
    /// <summary>
    /// PartitionedBloomFilter implements a variation of a classic Bloom filter as
    /// described by Almeida, Baquero, Preguica, and Hutchison in Scalable Bloom
    /// Filters:
    ///
    /// http://gsd.di.uminho.pt/members/cbm/ps/dbloom.pdf
    ///
    /// This filter works by partitioning the M-sized bit array into k slices of
    /// size m = M/k bits. Each hash function produces an index over m for its
    /// respective slice. Thus, each element is described by exactly k bits, meaning
    /// the distribution of false positives is uniform across all elements.
    /// </summary>
    public class PartitionedBloomFilter : IFilter<string>, IFilter<int>, IFilter<uint>,
        IFilter<long>, IFilter<ulong>, IFilter<float>, IFilter<double>,
        IFilter<byte[]>, IEquatable<PartitionedBloomFilter>
    {
        public bool Equals(PartitionedBloomFilter other)
        {
            if (this.M != other.M) return false;
            if (this.k != other.k) return false;
            if (this.S != other.S) return false;
            if (this.Hash != other.Hash) return false;
            if (!this.Partitions.SequenceEqual(other.Partitions)) return false;

            return true;
        }
        /// <summary>
        /// Partitioned filter data
        /// </summary>
        internal Buckets[] Partitions { get; set; }
        /// <summary>
        /// Hash algorithm
        /// </summary>
        internal HashAlgorithm Hash { get; set; }
        /// <summary>
        /// Filter size (divided into k partitions)
        /// </summary>
        private uint M { get; set; }
        /// <summary>
        /// Number of hash functions (and partitions)
        /// </summary>
        private uint k { get; set; }
        /// <summary>
        /// Partition size (m / k)
        /// </summary>
        private uint S { get; set; }
        /// <summary>
        /// Number of items added
        /// </summary>
        private uint count { get; set; }

        /// <summary>
        /// Creates a new partitioned Bloom filter optimized to store n items with a
        /// specified target false-positive rate.
        /// </summary>
        /// <param name="n">Number of items</param>
        /// <param name="fpRate">Desired false-positive rate</param>
        public PartitionedBloomFilter(uint n, double fpRate)
        {
            var m = Utils.OptimalM(n, fpRate);
            var k = Utils.OptimalK(fpRate);
            var partitions = new Buckets[k];
            var s = (uint)Math.Ceiling((double)m / (double)k);

            for (uint i = 0; i < k; i++)
            {
                partitions[i] = new Buckets(s, 1);
            }

            this.Partitions = partitions;
            this.Hash = Defaults.GetDefaultHashAlgorithm();
            this.M = m;
            this.k = k;
            this.S = s;
        }
        internal PartitionedBloomFilter(PartitionedBloomState state)
        {
            this.Hash = Defaults.GetDefaultHashAlgorithm();
            this.M = state.M;
            this.k = state.K;
            this.S = state.S;
            this.count = state.Count;
            this.Partitions = state.Buckets.Select(x => new Buckets(x)).ToArray();
        }
        internal PartitionedBloomState GetState()
        {
            return new PartitionedBloomState
            {
                Buckets = this.Partitions.Select(x => x.GetState()).ToArray(),
                M = M,
                K = k,
                S = S,
                Count = count
            };
        }

        /// <summary>
        /// Returns the Bloom filter capacity, m.
        /// </summary>
        /// <returns>The Bloom filter capacity, m</returns>
        public uint Capacity()
        {
            return this.M;
        }

        /// <summary>
        /// Returns the number of hash functions.
        /// </summary>
        /// <returns>The number of hash functions</returns>
        public uint K()
        {
            return this.k;
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
        /// Returns the current estimated ratio of set bits.
        /// </summary>
        /// <returns>The current estimated ratio of set bits</returns>
        public double EstimatedFillRatio()
        {
            return 1 - Math.Exp(-(double)this.count / (double)this.S);
        }

        /// <summary>
        /// Returns the average ratio of set bits across all partitions.
        /// </summary>
        /// <returns>The average ratio of set bitsacross all partitions</returns>
        public double FillRatio()
        {
            var t = (double)0;
            for (uint i = 0; i < this.k; i++)
            {
                uint sum = 0;
                for (uint j = 0; j < this.Partitions[i].Count; j++)
                {
                    sum += this.Partitions[i].Get(j);
                }
                t += ((double)sum / (double)this.S);
            }
            return (double)t / (double)this.k;
        }

        public bool Test(string element)
        {
            return TestBytes(Encoding.UTF8.GetBytes(element));
        }

        public bool Test(int element)
        {
            return TestBytes(BitConverter.GetBytes(element));
        }

        public bool Test(uint element)
        {
            return TestBytes(BitConverter.GetBytes(element));
        }

        public bool Test(long element)
        {
            return TestBytes(BitConverter.GetBytes(element));
        }

        public bool Test(ulong element)
        {
            return TestBytes(BitConverter.GetBytes(element));
        }

        public bool Test(float element)
        {
            return TestBytes(BitConverter.GetBytes(element));
        }

        public bool Test(double element)
        {
            return TestBytes(BitConverter.GetBytes(element));
        }

        public bool Test(byte[] element)
        {
            return TestBytes(element);
        }
        public bool TestAndAdd(string element)
        {
            return TestAndAddBytes(Encoding.UTF8.GetBytes(element));
        }

        public bool TestAndAdd(int element)
        {
            return TestAndAddBytes(BitConverter.GetBytes(element));
        }

        public bool TestAndAdd(uint element)
        {
            return TestAndAddBytes(BitConverter.GetBytes(element));
        }

        public bool TestAndAdd(long element)
        {
            return TestAndAddBytes(BitConverter.GetBytes(element));
        }

        public bool TestAndAdd(ulong element)
        {
            return TestAndAddBytes(BitConverter.GetBytes(element));
        }

        public bool TestAndAdd(float element)
        {
            return TestAndAddBytes(BitConverter.GetBytes(element));
        }

        public bool TestAndAdd(double element)
        {
            return TestAndAddBytes(BitConverter.GetBytes(element));
        }

        public bool TestAndAdd(byte[] element)
        {
            return TestAndAddBytes(element);
        }
        public IFilter<string> Add(string element)
        {
            return AddBytes<string>(Encoding.UTF8.GetBytes(element));
        }

        public IFilter<int> Add(int element)
        {
            return AddBytes<int>(BitConverter.GetBytes(element));
        }

        public IFilter<uint> Add(uint element)
        {
            return AddBytes<uint>(BitConverter.GetBytes(element));
        }

        public IFilter<long> Add(long element)
        {
            return AddBytes<long>(BitConverter.GetBytes(element));
        }

        public IFilter<ulong> Add(ulong element)
        {
            return AddBytes<ulong>(BitConverter.GetBytes(element));
        }

        public IFilter<float> Add(float element)
        {
            return AddBytes<float>(BitConverter.GetBytes(element));
        }

        public IFilter<double> Add(double element)
        {
            return AddBytes<double>(BitConverter.GetBytes(element));
        }

        public IFilter<byte[]> Add(byte[] element)
        {
            return AddBytes<byte[]>(element);
        }

        /// <summary>
        /// Will test for membership of the data and returns true if it is a
        /// member, false if not. This is a probabilistic test, meaning there is a
        /// non-zero probability of false positives but a zero probability of false
        /// negatives. Due to the way the filter is partitioned, the probability of
        /// false positives is uniformly distributed across all elements.
        /// </summary>
        /// <param name="data">The data to test for</param>
        /// <returns>Whether or not the data was found</returns>
        private bool TestBytes(byte[] data)
        {
            var hashKernel = Utils.HashKernel(data, this.Hash);
            var lower = hashKernel.LowerBaseHash;
            var upper = hashKernel.UpperBaseHash;

            // If any of the K partiion bits are not set, then it's not a member.
            for (uint i = 0; i < this.k; i++)
            {
                if (this.Partitions[i].Get((lower + upper * i) % this.S) == 0)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Will add the data to the Bloom filter. It returns the filter to allow for
        /// chaining.
        /// </summary>
        /// <param name="data">The data to add</param>
        /// <returns>The PartitionedBloomFilter</returns>
        private IFilter<T> AddBytes<T>(byte[] data)
        {
            var hashKernel = Utils.HashKernel(data, this.Hash);
            var lower = hashKernel.LowerBaseHash;
            var upper = hashKernel.UpperBaseHash;

            // Set the K partition bits.
            for (uint i = 0; i < this.k; i++)
            {
                this.Partitions[i].Set((lower + upper * i) % this.S, 1);
            }

            this.count++;
            return this as IFilter<T>;
        }

        /// <summary>
        /// Equivalent to calling Test followed by Add. It returns true if the data is a
        /// member, false if not.
        /// </summary>
        /// <param name="data">The data to test for and add</param>
        /// <returns>
        /// Whether the data was present in the filter prior to adding it
        /// </returns>
        private bool TestAndAddBytes(byte[] data)
        {
            var hashKernel = Utils.HashKernel(data, this.Hash);
            var lower = hashKernel.LowerBaseHash;
            var upper = hashKernel.UpperBaseHash;
            var member = true;

            // If any K partition bits are not set, then it's not a member.
            for (uint i = 0; i < this.k; i++)
            {
                var idx = (lower + upper * i) % this.S;
                if (this.Partitions[i].Get(idx) == 0)
                {
                    member = false;
                }
                this.Partitions[i].Set(idx, 1);
            }

            this.count++;
            return member;
        }

        /// <summary>
        /// Restores the Bloom filter to its original state. It returns the filter
        /// to allow for chaining.
        /// </summary>
        /// <returns>The PartitionedBloomFilter</returns>
        public PartitionedBloomFilter Reset()
        {
            foreach (var partition in this.Partitions)
            {
                partition.Reset();
            }
            return this;
        }

        /// <summary>
        /// Sets the hashing function used in the filter.
        /// </summary>
        /// <param name="h">The HashAlgorithm to use.</param>
        // TODO: Add SetHash to the IFilter interface?
        public void SetHash(HashAlgorithm h)
        {
            this.Hash = h;
        }

    }
}
