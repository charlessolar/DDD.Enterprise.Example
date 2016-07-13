using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Algorithms.Frugal
{
    public class FrugalSerializer
    {
        /// <summary>
        ///     Highest major version of the serialization format which this serializer can deserialize. A breaking change in the format requires a
        ///     bump in major version, i.e. version 2.X cannot read 3.Y
        /// </summary>
        public const ushort DataFormatMajorVersion = 1;

        /// <summary>
        ///     Minor version of the serialization format. A non-breaking change should be marked by a bump in minor version, i.e. version 2.2
        ///     should be able to read version 2.3
        /// </summary>
        public const ushort DataFormatMinorVersion = 0;

        public void Serialize(Stream stream, FrugalQuantile estimator)
        {
            using (var bw = new BinaryWriter(stream))
            {
                bw.Write(DataFormatMajorVersion);
                bw.Write(DataFormatMinorVersion);

                var data = estimator.GetState();

                bw.Write(data.Quantile);
                bw.Write(data.Estimate);

                bw.Write(data.DirectCount != null ? true : false);
                if (data.DirectCount != null)
                {
                    bw.Write(data.DirectCount.Count);
                    foreach (var element in data.DirectCount)
                        bw.Write(element);
                }
                bw.Flush();
            }
        }
        public FrugalQuantile Deserialize(Stream stream)
        {

            using (var br = new BinaryReader(stream))
            {
                int dataFormatMajorVersion = br.ReadUInt16();
                int dataFormatMinorVersion = br.ReadUInt16();

                AssertDataVersionCanBeRead(dataFormatMajorVersion, dataFormatMinorVersion);

                var quantile = br.ReadDouble();
                var estimate = br.ReadDouble();

                ICollection<double> directCount = null;
                var direct = br.ReadBoolean();
                if (direct)
                {
                    int count = br.ReadInt32();
                    directCount = new List<double>();

                    for (var i = 0; i < count; i++)
                    {
                        var element = br.ReadDouble();
                        directCount.Add(element);
                    }
                }
                var data = new FrugalState
                {
                    DirectCount = directCount,
                    Estimate = estimate,
                    Quantile = quantile
                };

                var result = new FrugalQuantile(data);

                return result;

            }
        }
        /// <summary>
        ///     Checks that this serializer can deserialize data with the given major and minor version numbers
        /// </summary>
        /// <exception cref="SerializationException">If this serializer cannot read data with the given version numbers</exception>
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
}
