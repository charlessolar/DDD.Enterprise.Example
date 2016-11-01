namespace Demo.Library.Algorithms.Bloom
{
    internal class BucketState
    {
        public byte[] Data;
        public byte BucketSize;
        public byte Max;
        public uint Count;
    }
    internal class PartitionedBloomState
    {
        public BucketState[] Buckets;
        public uint M;
        public uint K;
        public uint S;
        public uint Count;
    }

    internal class ScalableBloomState
    {
        public PartitionedBloomState[] Partitions;

        public double R;
        public double Fp;
        public double P;
        public uint Hint;
    }
}
