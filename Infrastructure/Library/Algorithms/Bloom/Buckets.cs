using System;
using System.Linq;

namespace Demo.Library.Algorithms.Bloom
{
    /// <summary>
    /// Buckets is a fast, space-efficient array of buckets where each bucket can store
    /// up to a configured maximum value.
    /// </summary>
    public class Buckets : IEquatable<Buckets>
    {
        private byte[] Data { get; set; }
        private byte BucketSize { get; set; }
        private byte _max;
        private int Max
        {
            get
            {
                return _max;
            }
            set
            {
                // TODO: Figure out this truncation thing.
                // I'm not sure if MaxValue is always supposed to be capped at 255 via
                // a byte conversion or not...
                if (value > byte.MaxValue)
                    _max = byte.MaxValue;
                else
                    _max = (byte)value;
            }
        }
        internal uint Count { get; set; }

        /// <summary>
        /// Creates a new Buckets with the provided number of buckets where each bucket
        /// is the specified number of bits.
        /// </summary>
        /// <param name="count">Number of buckets.</param>
        /// <param name="bucketSize">Number of bits per bucket.</param>
        internal Buckets(uint count, byte bucketSize)
        {
            this.Count = count;
            this.Data = new byte[(count * bucketSize + 7) / 8];
            this.BucketSize = bucketSize;
            this.Max = (1 << bucketSize) - 1;
        }

        internal Buckets(BucketState state)
        {
            this.Count = state.Count;
            this.Data = state.Data;
            this.BucketSize = state.BucketSize;
            this.Max = state.Max;
        }
        internal BucketState GetState()
        {
            return new BucketState
            {
                Count = this.Count,
                Data = this.Data,
                BucketSize = this.BucketSize,
                Max = this._max
            };
        }

        public bool Equals(Buckets other)
        {
            if (this.BucketSize != other.BucketSize) return false;
            if (this._max != other._max) return false;
            if (!this.Data.SequenceEqual(other.Data)) return false;
            return true;
        }
        /// <summary>
        /// Returns the maximum value that can be stored in a bucket.
        /// </summary>
        /// <returns>The bucket max value.</returns>
        internal byte MaxBucketValue()
        {
            return this._max;
        }

        /// <summary>
        /// Increment the value in the specified bucket by the provided delta. A bucket
        /// can be decremented by providing a negative delta.
        /// <para>
        ///     The value is clamped to zero and the maximum bucket value. Returns itself
        ///     to allow for chaining.
        /// </para>
        /// </summary>
        /// <param name="bucket">The bucket to increment.</param>
        /// <param name="delta">The amount to increment the bucket by.</param>
        /// <returns>The modified bucket.</returns>
        internal Buckets Increment(uint bucket, int delta)
        {
            int val = (int)(GetBits(bucket * this.BucketSize, this.BucketSize) + delta);

            if (val > this.Max)
                val = this.Max;
            else if (val < 0)
                val = 0;

            SetBits((uint)bucket * (uint)this.BucketSize, this.BucketSize, (uint)val);
            return this;
        }

        /// <summary>
        /// Set the bucket value. The value is clamped to zero and the maximum bucket
        /// value. Returns itself to allow for chaining.
        /// </summary>
        /// <param name="bucket">The bucket to change the value of.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The modified bucket.</returns>
        internal Buckets Set(uint bucket, byte value)
        {
            if (value > this._max)
                value = this._max;

            SetBits(bucket * this.BucketSize, this.BucketSize, value);
            return this;
        }

        /// <summary>
        /// Returns the value in the specified bucket.
        /// </summary>
        /// <param name="bucket">The bucket to get.</param>
        /// <returns>The specified bucket.</returns>
        internal uint Get(uint bucket)
        {
            return GetBits(bucket * this.BucketSize, this.BucketSize);
        }

        /// <summary>
        /// Restores the Buckets to the original state. Returns itself to allow for
        /// chaining.
        /// </summary>
        /// <returns>The Buckets object the reset operation was performed on.</returns>
        internal Buckets Reset()
        {
            this.Data = new byte[(this.Count * this.BucketSize + 7) / 8];
            return this;
        }

        /// <summary>
        /// Returns the bits at the specified offset and length.
        /// </summary>
        /// <param name="offset">The position to start reading at.</param>
        /// <param name="length">The distance to read from the offset.</param>
        /// <returns>The bits at the specified offset and length.</returns>
        internal uint GetBits(uint offset, int length)
        {
            uint byteIndex = offset / 8;
            int byteOffset = (int)(offset % 8);

            if ((byteOffset + length) > 8)
            {
                int rem = 8 - byteOffset;
                return GetBits(offset, rem)
                    | (GetBits((uint)(offset + rem), length - rem) << rem);
            }

            int bitMask = (1 << length) - 1;
            return (uint)((this.Data[byteIndex] & (bitMask << byteOffset)) >> byteOffset);
        }

        /// <summary>
        /// Sets bits at the specified offset and length.
        /// </summary>
        /// <param name="offset">The position to start writing at.</param>
        /// <param name="length">The distance to write from the offset.</param>
        /// <param name="bits">The bits to write.</param>
        internal void SetBits(uint offset, int length, uint bits)
        {
            uint byteIndex = offset / 8;
            int byteOffset = (int)(offset % 8);

            if ((byteOffset + length) > 8)
            {
                int rem = 8 - byteOffset;
                SetBits(offset, (byte)rem, bits);
                SetBits((uint)(offset + rem), length - rem, bits >> rem);
                return;
            }

            int bitMask = (1 << length) - 1;
            this.Data[byteIndex] =
                (byte)((this.Data[byteIndex]) & ~(bitMask << byteOffset));
            this.Data[byteIndex] =
                (byte)((this.Data[byteIndex]) | ((bits & bitMask) << byteOffset));
        }
    }
}
