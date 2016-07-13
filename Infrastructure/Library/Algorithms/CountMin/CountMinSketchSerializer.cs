using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Algorithms.CountMin
{
    public class CountMinSketchSerializer
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

        public void Serialize(Stream stream, CountMinSketch estimator)
        {
            using (var bw = new BinaryWriter(stream))
            {
                bw.Write(DataFormatMajorVersion);
                bw.Write(DataFormatMinorVersion);

                var data = estimator.GetState();

                bw.Write(data.Count);
                bw.Write(data.Epsilon);
                bw.Write(data.Delta);

                foreach (var row in data.Matrix)
                    foreach (var col in row)
                        bw.Write(col);

                bw.Flush();
            }
        }
        public CountMinSketch Deserialize(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                int dataFormatMajorVersion = br.ReadUInt16();
                int dataFormatMinorVersion = br.ReadUInt16();

                AssertDataVersionCanBeRead(dataFormatMajorVersion, dataFormatMinorVersion);

                var count = br.ReadUInt64();
                var epsilon = br.ReadDouble();
                var delta = br.ReadDouble();

                var width = (uint)(Math.Ceiling(Math.E / epsilon));
                var depth = (uint)(Math.Ceiling(Math.Log(1 / delta)));
                
                var matrix = new UInt64[depth][];

                for (var i = 0; i < depth; i++)
                {
                    matrix[i] = new UInt64[width];
                    for (var j = 0; j < width; j++)
                    {
                        var element = br.ReadUInt64();
                        matrix[i][j] = element;
                    }
                }

                var data = new CountMinSketchState
                {
                    Count = count,
                    Epsilon = epsilon,
                    Delta = delta,
                    Matrix = matrix
                };

                var result = new CountMinSketch(data);

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
