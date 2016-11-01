/*
Original work Copyright (c) 2012 Jeff Hodges. All rights reserved.
Modified work Copyright (c) 2015 Tyler Treat. All rights reserved.
Modified work Copyright (c) 2015 Matthew Lorimor. All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are
met:

   * Redistributions of source code must retain the above copyright
notice, this list of conditions and the following disclaimer.
   * Redistributions in binary form must reproduce the above
copyright notice, this list of conditions and the following disclaimer
in the documentation and/or other materials provided with the
distribution.
   * Neither the name of Jeff Hodges nor the names of this project's
contributors may be used to endorse or promote products derived from
this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Demo.Library.Algorithms.Bloom
{
    /// <summary>
    /// InverseBloomFilter is a concurrent "inverse" Bloom filter, which is
    /// effectively the opposite of a classic Bloom filter. This was originally
    /// described and written by Jeff Hodges:
    ///
    /// http://www.somethingsimilar.com/2012/05/21/the-opposite-of-a-bloom-filter/
    ///
    /// The InverseBloomFilter may report a false negative but can never report a
    /// false positive. That is, it may report that an item has not been seen when
    /// it actually has, but it will never report an item as seen which it hasn't
    /// come across. This behaves in a similar manner to a fixed-size hashmap which
    /// does not handle conflicts.
    ///
    /// An example use case is deduplicating events while processing a stream of
    /// data. Ideally, duplicate events are relatively close together.
    /// </summary>
    public class InverseBloomFilter : IFilter<string>, IFilter<int>, IFilter<uint>,
        IFilter<long>, IFilter<ulong>, IFilter<float>, IFilter<double>,
        IFilter<byte[]>, IEquatable<InverseBloomFilter>
    {
        private byte[][] Array { get; set; }
        internal HashAlgorithm Hash { get; set; }
        private uint capacity { get; set; }

        /// <summary>
        /// Instantiates an InverseBloomFilter with the specified capacity.
        /// </summary>
        /// <param name="capacity">The capacity of the filter</param>
        public InverseBloomFilter(uint capacity)
        {
            this.Array = new byte[capacity][];
            this.Hash = Defaults.GetDefaultHashAlgorithm();
            this.capacity = capacity;
        }


        public bool Equals(InverseBloomFilter other)
        {
            if (this.Hash != other.Hash) return false;
            if (this.capacity != other.capacity) return false;

            if (Array.GetLength(0) != other.Array.GetLength(0) ||
                Array.GetLength(1) != other.Array.GetLength(1))
                return false;
            for (int i = 0; i < Array.GetLength(0); i++)
                for (int j = 0; j < Array.GetLength(1); j++)
                    if (Array.ElementAt(i).ElementAt(j).CompareTo(other.Array.ElementAt(i).ElementAt(j)) != 0)
                        return false;
            return true;
            
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
        /// non-zero probability of false negatives but a zero probability of false
        /// positives. That is, it may return false even though the data was added, but
        /// it will never return true for data that hasn't been added.
        /// </summary>
        /// <param name="data">The data to test for</param>
        /// <returns>Whether or not the data is present</returns>
        private bool TestBytes(byte[] data)
        {
            var index = this.Index(data);
            var val = this.Array[index];
            if (val == null)
            {
                return false;
            }
            return val.SequenceEqual(data);
        }

        /// <summary>
        /// Will add the data to the filter. It returns the filter to allow for chaining.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IFilter<T> AddBytes<T>(byte[] data)
        {
            var index = this.Index(data);
            this.GetAndSet(index, data);
            return this as IFilter<T>;
        }

        /// <summary>
        /// Equivalent to calling Test followed by Add atomically. It returns true if
        /// the data is a member, false if not.
        /// </summary>
        /// <param name="data">The data to test and add</param>
        /// <returns>Whether the data was already a member</returns>
        private bool TestAndAddBytes(byte[] data)
        {
            var index = this.Index(data);
            var oldId = this.GetAndSet(index, data);
            if (oldId == null)
            {
                return false;
            }
            return oldId.SequenceEqual(data);
        }

        /// <summary>
        /// Returns the filter capactiy.
        /// </summary>
        /// <returns>The filter capactiy</returns>
        public uint Capacity()
        {
            return this.capacity;
        }

        /// <summary>
        /// Returns the data that was in the array at the given index after putting the
        /// new data in the array at that index, atomically.
        /// </summary>
        /// <param name="index">The index to get and set</param>
        /// <param name="data">The data to set</param>
        /// <returns>
        /// The data that was in the array at the index before setting it
        /// </returns>
        private byte[] GetAndSet(uint index, byte[] data)
        {
            var oldData = this.Array[index];
            this.Array[index] = data;
            return oldData;
        }

        /// <summary>
        /// Returns the array index for the given data.
        /// </summary>
        /// <param name="data">The data to find the index for</param>
        /// <returns>The array index for the given data</returns>
        private uint Index(byte[] data)
        {
            var index = this.ComputeHashSum32(data) % this.capacity;
            return index;
        }

        /// <summary>
        /// Returns a 32-bit hash value for the given data.
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
