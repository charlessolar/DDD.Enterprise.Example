using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
                bw.Write(data.FP);
                bw.Write(data.P);
                bw.Write(data.Hint);

                bw.Write(data.partitions.Length);

                foreach (var partition in data.partitions)
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

                double R = br.ReadDouble();
                double FP = br.ReadDouble();
                double P = br.ReadDouble();
                uint Hint = br.ReadUInt32();

                Int32 count = br.ReadInt32();
                var stored = new List<PartitionedBloomState>();
                for (var i = 0; i < count; i++)
                {
                    stored.Add(ParitionedBloomSerializer.Deserialize(br));
                }


                var data = new ScalableBloomState
                {
                    partitions = stored.ToArray(),
                    R = R,
                    FP = FP,
                    P = P,
                    Hint = Hint
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
                    string.Format("Incompatible data format, can't deserialize data version {0}.{1} (serializer version: {2}.{3})",
                        dataFormatMajorVersion, dataFormatMinorVersion, DataFormatMajorVersion, DataFormatMinorVersion));
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
            bw.Write(state.k);
            bw.Write(state.S);
            bw.Write(state.count);

            bw.Write(state.buckets.Length);
            foreach (var bucket in state.buckets)
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
            uint M = br.ReadUInt32();
            uint k = br.ReadUInt32();
            uint S = br.ReadUInt32();
            uint count = br.ReadUInt32();

            Int32 buckets = br.ReadInt32();
            var stored = new List<BucketState>();
            for (var i = 0; i < buckets; i++)
            {
                stored.Add(BucketSerializer.Deserialize(br));
            }

            return new PartitionedBloomState
            {
                buckets = stored.ToArray(),
                M = M,
                k = k,
                S = S,
                count = count
            };
        }

        private static void AssertDataVersionCanBeRead(int dataFormatMajorVersion, int dataFormatMinorVersion)
        {
            if (dataFormatMajorVersion > DataFormatMajorVersion)
            {
                throw new SerializationException(
                    string.Format("Incompatible data format, can't deserialize data version {0}.{1} (serializer version: {2}.{3})",
                        dataFormatMajorVersion, dataFormatMinorVersion, DataFormatMajorVersion, DataFormatMinorVersion));
            }
        }
    }
    internal static class BucketSerializer
    {
        public static void Serialize(BinaryWriter bw, BucketState state)
        {
            bw.Write(state.bucketSize);
            bw.Write(state.max);
            bw.Write(state.count);
            bw.Write(state.data);
        }
        public static BucketState Deserialize(BinaryReader br)
        {
            byte bucketSize = br.ReadByte();
            byte max = br.ReadByte();
            uint count = br.ReadUInt32();

            byte[] data = br.ReadBytes((Int32)(count * bucketSize + 7) / 8);

            return new BucketState
            {
                bucketSize = bucketSize,
                max = max,
                count = count,
                data = data,
            };
        }
    }
}
