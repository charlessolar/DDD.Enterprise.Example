using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Library.Algorithms.MinHash
{
    /// <summary>
    /// MinHash is a variation of the technique for estimating similarity between
    /// two sets as presented by Broder in On the resemblance and containment of
    /// documents:
    ///
    /// http://gatekeeper.dec.com/ftp/pub/dec/SRC/publications/broder/positano-final-wpnums.pdf
    ///
    /// This can be used to cluster or compare documents by splitting the corpus
    /// into a bag of words. MinHash returns the approximated similarity ratio of
    /// the two bags. The similarity is less accurate for very small bags of words.
    /// </summary>
    public static class MinHash
    {
        private static readonly FastRandom Random = new FastRandom();

        /// <summary>
        /// Returns the similarity between two bags.
        /// </summary>
        /// <param name="bag1">The first bag</param>
        /// <param name="bag2">The second bag</param>
        /// <returns>The similarity between the bags</returns>
        public static float Similarity(string[] bag1, string[] bag2)
        {
            var k = bag1.Length + bag2.Length;
            var hashes = new int[k];
            for (int i = 0; i < k; i++)
            {
                var a = Random.Next();
                var b = Random.Next();
                var c = Random.Next();
                var x = ComputeHash((uint)(a * b * c), (uint)a, (uint)b, c);
                hashes[i] = (int)x;
            }

            var bMap = BitMap(bag1, bag2);
            var minHashValues = HashBuckets(2, k);
            minHash(bag1, 0, minHashValues, bMap, k, hashes);
            minHash(bag2, 1, minHashValues, bMap, k, hashes);
            return similarity(minHashValues, k);
        }

        private static void minHash(
            string[] bag,
            int bagIndex,
            int[][] minHashValues,
            Dictionary<string, bool[]> bitArray,
            int k,
            int[] hashes)
        {
            var options = new ParallelOptions {MaxDegreeOfParallelism = 4};
            var index = 0;

            foreach (var element in bitArray)
            {
                Parallel.For(0, k, options, (i, loopState) =>
                {
                    if (bag.Contains(element.Key))
                    {
                        var hindex = hashes[index];
                        if (hindex < minHashValues[bagIndex][index])
                        {
                            minHashValues[bagIndex][index] = hindex;
                        }
                    }
                });
                index++;
            }
        }

        private static Dictionary<string, bool[]> BitMap(string[] bag1, string[] bag2)
        {
            var bitArray = new Dictionary<string, bool[]>();
            foreach (var element in bag1)
            {
                bitArray[element] = new bool[] { true, false };
            }

            foreach (var element in bag2)
            {
                if (bitArray.ContainsKey(element))
                {
                    bitArray[element] = new bool[] { true, true };
                }
                else
                {
                    bitArray[element] = new bool[] { false, true };
                }
            }

            return bitArray;
        }

        private static int[][] HashBuckets(int numSets, int k)
        {
            var minHashValues = new int[numSets][];
            for (int i = 0; i < numSets; i++)
            {
                minHashValues[i] = new int[k];
            }

            for (int i = 0; i < numSets; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    minHashValues[i][j] = int.MaxValue;
                }
            }
            return minHashValues;
        }

        private static uint ComputeHash(uint x, uint a, uint b, int u)
        {
            return (a * x + b) >> (32 - u);
        }

        private static float similarity(int[][] minHashValues, int k)
        {
            var identicalMinHashes = 0;
            for (int i = 0; i < k; i++)
            {
                if (minHashValues[0][i] == minHashValues[1][i])
                {
                    identicalMinHashes++;
                }
            }

            return (float)(1.0 * (float)identicalMinHashes) / (float)k;
        }
    }
}
