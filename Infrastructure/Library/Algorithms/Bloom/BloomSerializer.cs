using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Demo.Library.Algorithms.Bloom
{
    public class ScalableBloomSerializer
    {
        public const ushort DataFormatMajorVersion = 1;

        public const ushort DataFormatMinorVersion = 0;

        public void Serialize(Stream stream, ScalableBloomFilter filter)
        {

            using (var bw = new BinaryWriter(stream))
            {
                bw.Write(DataFormatMajorVersion);
                bw.Write(DataFormatMinorVersion);

                ScalableBloomState data = filter.GetState();

                bw.Write(data.R);
                bw.Write(data.Fp);
                bw.Write(data.P);
                bw.Write(data.Hint);

                bw.Write(data.Partitions.Length);

                foreach (var partition in data.Partitions)
                    ParitionedBloomSerializer.Serialize(bw, partition);

                bw.Flush();
            }
        }
        public ScalableBloomFilter Deserialize(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                int dataFormatMajorVersion = br.ReadUInt16();
                int dataFormatMinorVersion = br.ReadUInt16();

                AssertDataVersionCanBeRead(dataFormatMajorVersion, dataFormatMinorVersion);

                double r = br.ReadDouble();
                double fp = br.ReadDouble();
                double p = br.ReadDouble();
                uint hint = br.ReadUInt32();

                int count = br.ReadInt32();
                var stored = new List<PartitionedBloomState>();
                for (var i = 0; i < count; i++)
                {
                    stored.Add(ParitionedBloomSerializer.Deserialize(br));
                }


                var data = new ScalableBloomState
                {
                    Partitions = stored.ToArray(),
                    R = r,
                    Fp = fp,
                    P = p,
                    Hint = hint
                };

                var result = new ScalableBloomFilter(data);

                return result;
            }

        }

        private static void AssertDataVersionCanBeRead(int dataFormatMajorVersion, int dataFormatMinorVersion)
        {
            if (dataFormatMajorVersion > DataFormatMajorVersion)
            {
                throw new SerializationException(
                    $"Incompatible data format, can't deserialize data version {dataFormatMajorVersion}.{dataFormatMinorVersion} (serializer version: {DataFormatMajorVersion}.{DataFormatMinorVersion})");
            }
        }
    }

    public class ParitionedBloomSerializer
    {
        public const ushort DataFormatMajorVersion = 1;

        public const ushort DataFormatMinorVersion = 0;

        public void Serialize(Stream stream, PartitionedBloomFilter filter)
        {
            using (var bw = new BinaryWriter(stream))
            {
                bw.Write(DataFormatMajorVersion);
                bw.Write(DataFormatMinorVersion);

                PartitionedBloomState data = filter.GetState();

                Serialize(bw, data);

                bw.Flush();
            }

        }
        internal static void Serialize(BinaryWriter bw, PartitionedBloomState state)
        {
            bw.Write(state.M);
            bw.Write(state.K);
            bw.Write(state.S);
            bw.Write(state.Count);

            bw.Write(state.Buckets.Length);
            foreach (var bucket in state.Buckets)
                BucketSerializer.Serialize(bw, bucket);

        }
        public PartitionedBloomFilter Deserialize(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                int dataFormatMajorVersion = br.ReadUInt16();
                int dataFormatMinorVersion = br.ReadUInt16();

                AssertDataVersionCanBeRead(dataFormatMajorVersion, dataFormatMinorVersion);

                return new PartitionedBloomFilter(Deserialize(br));
            }
        }
        internal static PartitionedBloomState Deserialize(BinaryReader br)
        {
            uint m = br.ReadUInt32();
            uint k = br.ReadUInt32();
            uint s = br.ReadUInt32();
            uint count = br.ReadUInt32();

            int buckets = br.ReadInt32();
            var stored = new List<BucketState>();
            for (var i = 0; i < buckets; i++)
            {
                stored.Add(BucketSerializer.Deserialize(br));
            }

            return new PartitionedBloomState
            {
                Buckets = stored.ToArray(),
                M = m,
                K = k,
                S = s,
                Count = count
            };
        }

        private static void AssertDataVersionCanBeRead(int dataFormatMajorVersion, int dataFormatMinorVersion)
        {
            if (dataFormatMajorVersion > DataFormatMajorVersion)
            {
                throw new SerializationException(
                    $"Incompatible data format, can't deserialize data version {dataFormatMajorVersion}.{dataFormatMinorVersion} (serializer version: {DataFormatMajorVersion}.{DataFormatMinorVersion})");
            }
        }
    }
    internal static class BucketSerializer
    {
        public static void Serialize(BinaryWriter bw, BucketState state)
        {
            bw.Write(state.BucketSize);
            bw.Write(state.Max);
            bw.Write(state.Count);
            bw.Write(state.Data);
        }
        public static BucketState Deserialize(BinaryReader br)
        {
            byte bucketSize = br.ReadByte();
            byte max = br.ReadByte();
            uint count = br.ReadUInt32();

            byte[] data = br.ReadBytes((int)(count * bucketSize + 7) / 8);

            return new BucketState
            {
                BucketSize = bucketSize,
                Max = max,
                Count = count,
                Data = data,
            };
        }
    }
}
