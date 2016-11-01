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
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Demo.Library.Algorithms.Bloom
{
    /// <summary>
    /// ScalableBloomFilter implements a Scalable Bloom Filter as described by
    /// Almeida, Baquero, Preguica, and Hutchison in Scalable Bloom Filters:
    ///
    /// http://gsd.di.uminho.pt/members/cbm/ps/dbloom.pdf
    ///
    /// A Scalable Bloom Filter dynamically adapts to the number of elements in the
    /// data set while enforcing a tight upper bound on the false-positive rate.
    /// This works by adding Bloom filters with geometrically decreasing
    /// false-positive rates as filters become full. The tightening ratio, r,
    /// controls the filter growth. The compounded probability over the whole series
    /// converges to a target value, even accounting for an infinite series.
    ///
    /// Scalable Bloom Filters are useful for cases where the size of the data set
    /// isn't known a priori and memory constraints aren't of particular concern.
    /// For situations where memory is bounded, consider using Inverse or Stable
    /// Bloom Filters.
    /// </summary>
    public class ScalableBloomFilter : IFilter<string>, IFilter<int>, IFilter<uint>,
        IFilter<long>, IFilter<ulong>, IFilter<float>, IFilter<double>, IFilter<DateTime>, IFilter<bool>, IFilter<Guid>,
        IFilter<byte[]>, IEquatable<ScalableBloomFilter>
    {
        /// <summary>
        /// Filters with geometrically decreasing error rates
        /// </summary>
        internal List<PartitionedBloomFilter> Filters { get; set; }
        /// <summary>
        /// Tightening ratio
        /// </summary>
        internal double R { get; set; }
        /// <summary>
        /// Target false-positive rate
        /// </summary>
        internal double Fp { get; set; }
        /// <summary>
        /// Partition fill ratio
        /// </summary>
        private double P { get; set; }
        /// <summary>
        /// Filter size hint
        /// </summary>
        internal uint Hint { get; set; }

        /// <summary>
        /// Creates a new Scalable Bloom Filter with the specified target false-positive
        /// rate and tightening ratio. Use NewDefaultScalableBloomFilter if you don't
        /// want to calculate all these parameters.
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="fpRate"></param>
        /// <param name="r"></param>
        public ScalableBloomFilter(uint hint, double fpRate, double r)
        {
            this.Filters = new List<PartitionedBloomFilter>();
            this.R = r;
            this.Fp = fpRate;
            this.P = Defaults.FillRatio;
            this.Hint = hint;

            this.AddFilter();
        }
        internal ScalableBloomFilter(ScalableBloomState state)
        {
            this.Filters = state.Partitions.Select(x => new PartitionedBloomFilter(x)).ToList();
            this.R = state.R;
            this.Fp = state.Fp;
            this.P = state.P;
            this.Hint = state.Hint;
        }
        internal ScalableBloomState GetState()
        {
            return new ScalableBloomState
            {
                Partitions = this.Filters.Select(x => x.GetState()).ToArray(),
                R = this.R,
                Fp = this.Fp,
                P = this.P,
                Hint = this.Hint
            };
        }
        public bool Equals(ScalableBloomFilter other)
        {
            if (this.R != other.R) return false;
            if (this.Fp != other.Fp) return false;
            if (this.P != other.P) return false;
            if (this.Hint != other.Hint) return false;
            if (!this.Filters.SequenceEqual(other.Filters)) return false;

            return true;
        }

        /// <summary>
        /// Creates a new Scalable Bloom Filter with the specified target false-positive
        /// rate and an optimal tightening ratio.
        /// </summary>
        /// <param name="fpRate"></param>
        public static ScalableBloomFilter NewDefaultScalableBloomFilter(double fpRate)
        {
            return new ScalableBloomFilter(10000, fpRate, 0.8);
        }

        /// <summary>
        /// Returns the current Scalable Bloom Filter capacity, which is the sum of the
        /// capacities for the contained series of Bloom filters.
        /// </summary>
        /// <returns>The current Scalable Bloom Filter capacity</returns>
        public uint Capacity()
        {
            var capacity = 0u;
            foreach (var filter in this.Filters)
            {
                capacity += filter.Capacity();
            }
            return capacity;
        }

        /// <summary>
        /// Returns the number of hash functions used in each Bloom filter.
        /// </summary>
        /// <returns>The number of hash functions used in each Bloom filter</returns>
        public uint K()
        {
            return this.Filters[0].K();
        }

        /// <summary>
        /// Returns the average ratio of set bits across every filter.
        /// </summary>
        /// <returns>The average ratio of set bits across every filter</returns>
        public double FillRatio()
        {
            var sum = 0.0;
            foreach (var filter in this.Filters)
            {
                sum += filter.FillRatio();
            }
            return (double)sum / this.Filters.Count;
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
        public bool Test(DateTime element)
        {
            return Test(element.Ticks);
        }
        public bool Test(bool element)
        {
            return TestBytes(BitConverter.GetBytes(element));
        }
        public bool Test(Guid element)
        {
            return TestBytes(element.ToByteArray());
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
        public bool TestAndAdd(DateTime element)
        {
            return TestAndAdd(element.Ticks);
        }
        public bool TestAndAdd(bool element)
        {
            return TestAndAddBytes(BitConverter.GetBytes(element));
        }
        public bool TestAndAdd(Guid element)
        {
            return TestAndAddBytes(element.ToByteArray());
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
        public IFilter<DateTime> Add(DateTime element)
        {
            return AddBytes<DateTime>(BitConverter.GetBytes(element.Ticks));
        }
        public IFilter<bool> Add(bool element)
        {
            return AddBytes<bool>(BitConverter.GetBytes(element));
        }
        public IFilter<Guid> Add(Guid element)
        {
            return AddBytes<Guid>(element.ToByteArray());
        }

        public IFilter<byte[]> Add(byte[] element)
        {
            return AddBytes<byte[]>(element);
        }

        /// <summary>
        /// Will test for membership of the data and returns true if it is a member,
        /// false if not. This is a probabilistic test, meaning there is a non-zero
        /// probability of false positives but a zero probability of false negatives.
        /// </summary>
        /// <param name="data">The data to search for.</param>
        /// <returns>Whether or not the data is maybe contained in the filter.</returns>
        private bool TestBytes(byte[] data)
        {
            // Querying is made by testing for the presence in each filter.
            foreach (var filter in this.Filters)
            {
                if (filter.Test(data))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add will add the data to the Bloom filter. It returns the filter to allow
        /// for chaining.
        /// </summary>
        /// <param name="data">The data to add</param>
        /// <returns>The ScalableBloomFilter</returns>
        private IFilter<T> AddBytes<T>(byte[] data)
        {
            var idx = this.Filters.Count - 1;

            // If the last filter has reached its fill ratio, add a new one.
            if (this.Filters[idx].EstimatedFillRatio() >= this.P)
            {
                this.AddFilter();
                idx++;
            }

            this.Filters[idx].Add(data);
            return this as IFilter<T>;
        }

        /// <summary>
        /// Is equivalent to calling Test followed by Add. It returns true if the data
        /// is a member, false if not.
        /// </summary>
        /// <param name="data">The data to test for and add</param>
        /// <returns>Whether or not the data was present before adding it</returns>
        private bool TestAndAddBytes(byte[] data)
        {
            var member = this.Test(data);
            this.Add(data);
            return member;
        }

        /// <summary>
        /// Sets the hashing function used in the filter.
        /// </summary>
        /// <param name="h">The HashAlgorithm to use.</param>
        // TODO: Add SetHash to the IFilter interface?
        public void SetHash(HashAlgorithm h)
        {
            foreach (var filter in this.Filters)
            {
                filter.SetHash(h);
            }
        }

        /// <summary>
        /// Restores the Bloom filter to its original state. It returns the filter to
        /// allow for chaining.
        /// </summary>
        /// <returns>The reset bloom filter.</returns>
        public ScalableBloomFilter Reset()
        {
            this.Filters = new List<PartitionedBloomFilter>();
            this.AddFilter();
            return this;
        }

        /// <summary>
        /// Adds a new Bloom filter with a restricted false-positive rate to the
        /// Scalable Bloom Filter
        /// </summary>
        internal void AddFilter()
        {
            var fpRate = this.Fp * Math.Pow(this.R, this.Filters.Count);
            var p = new PartitionedBloomFilter(this.Hint, fpRate);
            if (this.Filters.Any())
            {
                p.SetHash(this.Filters[0].Hash);
            }
            this.Filters.Add(p);
        }
    }
}
