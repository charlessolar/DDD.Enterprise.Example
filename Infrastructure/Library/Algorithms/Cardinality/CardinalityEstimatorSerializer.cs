// /*  
//     See https://github.com/Microsoft/CardinalityEstimation.
//     The MIT License (MIT)
// 
//     Copyright (c) 2015 Microsoft
// 
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
// 
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
// 
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
// */

namespace Demo.Library.Algorithms.Cardinality
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using Hash;

    /// <summary>
    ///     Efficient serializer for <see cref="CardinalityEstimator" />
    /// </summary>
    public class CardinalityEstimatorSerializer
    {
        /// <summary>
        ///     Highest major version of the serialization format which this serializer can deserialize. A breaking change in the format requires a
        ///     bump in major version, i.e. version 2.X cannot read 3.Y
        /// </summary>
        public const ushort DataFormatMajorVersion = 2;

        /// <summary>
        ///     Minor version of the serialization format. A non-breaking change should be marked by a bump in minor version, i.e. version 2.2
        ///     should be able to read version 2.3
        /// </summary>
        public const ushort DataFormatMinorVersion = 1;

        /// <summary>
        ///     Serialize the given <paramref name="cardinalityEstimator" /> to <paramref name="stream" />
        /// </summary>
        public void Serialize(Stream stream, CardinalityEstimator cardinalityEstimator)
        {
            using (var bw = new BinaryWriter(stream))
            {
                bw.Write(DataFormatMajorVersion);
                bw.Write(DataFormatMinorVersion);

                CardinalityEstimatorState data = cardinalityEstimator.GetState();

                bw.Write((byte)data.HashFunctionId);
                bw.Write(data.BitsPerIndex);
                bw.Write((byte)(((data.IsSparse ? 1 : 0) << 1) + (data.DirectCount != null ? 1 : 0)));
                if (data.DirectCount != null)
                {
                    bw.Write(data.DirectCount.Count);
                    foreach (ulong element in data.DirectCount)
                    {
                        bw.Write(element);
                    }
                }
                else if (data.IsSparse)
                {
                    bw.Write(data.LookupSparse.Count);
                    foreach (KeyValuePair<ushort, byte> element in data.LookupSparse)
                    {
                        bw.Write(element.Key);
                        bw.Write(element.Value);
                    }
                }
                else
                {
                    bw.Write(data.LookupDense.Length);
                    foreach (byte element in data.LookupDense)
                    {
                        bw.Write(element);
                    }
                }

                bw.Write(data.CountAdditions);
                bw.Flush();
            }
        }

        /// <summary>
        ///     Deserialize a <see cref="CardinalityEstimator" /> from the given <paramref name="stream" />
        /// </summary>
        public CardinalityEstimator Deserialize(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                int dataFormatMajorVersion = br.ReadUInt16();
                int dataFormatMinorVersion = br.ReadUInt16();

                AssertDataVersionCanBeRead(dataFormatMajorVersion, dataFormatMinorVersion);

                HashFunctionId hashFunctionId;
                if (dataFormatMajorVersion >= 2)
                {
                    // Starting with version 2.0, the serializer writes the hash function ID
                    hashFunctionId = (HashFunctionId)br.ReadByte();
                }
                else
                {
                    // Versions before 2.0 all used FNV-1a
                    hashFunctionId = HashFunctionId.Fnv1A;
                }

                int bitsPerIndex = br.ReadInt32();
                byte flags = br.ReadByte();
                bool isSparse = ((flags & 2) == 2);
                bool isDirectCount = ((flags & 1) == 1);

                HashSet<ulong> directCount = null;
                IDictionary<ushort, byte> lookupSparse = isSparse ? new Dictionary<ushort, byte>() : null;
                byte[] lookupDense = null;

                if (isDirectCount)
                {
                    int count = br.ReadInt32();
                    directCount = new HashSet<ulong>();

                    for (var i = 0; i < count; i++)
                    {
                        ulong element = br.ReadUInt64();
                        directCount.Add(element);
                    }
                }
                else if (isSparse)
                {
                    int count = br.ReadInt32();

                    for (var i = 0; i < count; i++)
                    {
                        ushort elementKey = br.ReadUInt16();
                        byte elementValue = br.ReadByte();
                        lookupSparse.Add(elementKey, elementValue);
                    }
                }
                else
                {
                    int count = br.ReadInt32();
                    lookupDense = br.ReadBytes(count);
                }

                // Starting with version 2.1, the serializer writes CountAdditions
                ulong countAdditions = 0UL;
                if (dataFormatMajorVersion >= 2 && dataFormatMinorVersion >= 1)
                {
                    countAdditions = br.ReadUInt64();
                }

                var data = new CardinalityEstimatorState
                {
                    HashFunctionId = hashFunctionId,
                    BitsPerIndex = bitsPerIndex,
                    DirectCount = directCount,
                    IsSparse = isSparse,
                    LookupDense = lookupDense,
                    LookupSparse = lookupSparse,
                    CountAdditions = countAdditions,
                };

                var result = new CardinalityEstimator(data);

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
                    $"Incompatible data format, can't deserialize data version {dataFormatMajorVersion}.{dataFormatMinorVersion} (serializer version: {DataFormatMajorVersion}.{DataFormatMinorVersion})");
            }
        }
    }
}