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

namespace Demo.Library.Algorithms.Cardinality.Hash
{
    /// <summary>
    ///     Helper class to computes the 64-bit FNV-1a hash of byte arrays, <see cref="GetHashCode" />
    /// </summary>
    internal class Fnv1A : IHashFunction
    {
        /// <summary>
        ///     Computes the 64-bit FNV-1a hash of the given <paramref name="bytes" />, see
        ///     <see cref="http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function" />
        ///     and <see cref="http://www.isthe.com/chongo/src/fnv/hash_64a.c" />
        /// </summary>
        /// <param name="bytes">Text to compute the hash for</param>
        /// <returns>The 64-bit fnv1a hash</returns>
        public ulong GetHashCode(byte[] bytes)
        {
            const ulong fnv1A64Init = 14695981039346656037;
            const ulong fnv64Prime = 0x100000001b3;
            ulong hash = fnv1A64Init;

            foreach (byte b in bytes)
            {
                /* xor the bottom with the current octet */
                hash ^= b;
                /* multiply by the 64 bit FNV magic prime mod 2^64 */
                hash *= fnv64Prime;
            }

            return hash;
        }

        public HashFunctionId HashFunctionId => HashFunctionId.Fnv1A;
    }
}