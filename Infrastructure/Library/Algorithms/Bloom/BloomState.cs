using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Algorithms.Bloom
{
    internal class BucketState
    {
        public byte[] data;
        public byte bucketSize;
        public byte max;
        public uint count;
    }
    internal class PartitionedBloomState
    {
        public BucketState[] buckets;
        public uint M;
        public uint k;
        public uint S;
        public uint count;
    }

    internal class ScalableBloomState
    {
        public PartitionedBloomState[] partitions;

        public double R;
        public double FP;
        public double P;
        public uint Hint;
    }
}
