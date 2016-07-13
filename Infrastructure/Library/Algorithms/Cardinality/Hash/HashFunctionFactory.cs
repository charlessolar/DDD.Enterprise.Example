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
    using System;

    internal static class HashFunctionFactory
    {
        /// <summary>
        ///     Creates a hash function with the implementation id <paramref name="id" />
        /// </summary>
        /// <param name="id">Identifies a particular implementation of a hash function</param>
        /// <returns>The relevant hash function implementation</returns>
        /// <remarks>This method instantiates a new instance on each call. Make sure to reuse instances when appropriate</remarks>
        internal static IHashFunction GetHashFunction(HashFunctionId id)
        {
            switch (id)
            {
                case HashFunctionId.Murmur3:
                    return new Murmur3();
                case HashFunctionId.Fnv1A:
                    return new Fnv1A();
                default:
                    throw new NotImplementedException(string.Format("Support not implemented for hash function of type {0}", id));
            }
        }
    }
}