
using Demo.Library.Algorithms.Bloom;
using Demo.Library.Algorithms.Cardinality;
using Demo.Library.Algorithms.CountMin;
using Demo.Library.Algorithms.Frugal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class AlgorithmExtensions
    {
        public static byte[] SerializeCardinality(this CardinalityEstimator estimator, CardinalityEstimatorSerializer serializer)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, estimator);

                return stream.ToArray();

            }
        }
        public static CardinalityEstimator DeserializeCardinality(this byte[] estimator, CardinalityEstimatorSerializer serializer)
        {
            var stream = new MemoryStream(estimator);

            return serializer.Deserialize(stream);
        }
        public static byte[] SerializeFrugal(this FrugalQuantile estimator, FrugalSerializer serializer)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, estimator);

                return stream.ToArray();

            }
        }
        public static FrugalQuantile DeserializeFrugal(this byte[] estimator, FrugalSerializer serializer)
        {
            var stream = new MemoryStream(estimator);

            return serializer.Deserialize(stream);
        }
        public static byte[] SerializeCountMin(this CountMinSketch estimator, CountMinSketchSerializer serializer)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, estimator);

                return stream.ToArray();

            }
        }
        public static CountMinSketch DeserializeCountMin(this byte[] estimator, CountMinSketchSerializer serializer)
        {
            var stream = new MemoryStream(estimator);

            return serializer.Deserialize(stream);
        }
        public static byte[] SerializeBloom(this ScalableBloomFilter bloom, ScalableBloomSerializer serializer)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, bloom);

                return stream.ToArray();

            }
        }
        public static ScalableBloomFilter DeserializeBloom(this byte[] bloom, ScalableBloomSerializer serializer)
        {
            var stream = new MemoryStream(bloom);

            return serializer.Deserialize(stream);
        }
    }
}
