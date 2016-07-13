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
    /// <summary>
    ///     Estimator for the number of unique elements in a set
    /// </summary>
    /// <typeparam name="T">The type of elements in the set</typeparam>
    public interface ICardinalityEstimator<in T>
    {
        /// <summary>
        ///     Adds an element to the counted set.  Elements added multiple times will be counted only once.
        /// </summary>
        /// <param name="element">The element to add</param>
        void Add(T element);

        /// <summary>
        ///     Returns the estimated number of unique elements in the counted set
        /// </summary>
        /// <returns>The estimated count of unique elements</returns>
        ulong Count();

        /// <summary>
        ///     Gets the number of times elements were added (including duplicates)
        /// </summary>
        /// <returns>The number of times <see cref="Add"/> was called</returns>
        ulong CountAdditions { get; }
    }
}