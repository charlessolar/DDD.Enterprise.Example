using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Algorithms.Bloom
{
    public interface IFilter<in T>
    {
        /// <summary>
        /// Will test for membership of the data and returns true if it is a member,
        /// false if not.
        /// </summary>
        /// <param name="data">The data to test for.</param>
        /// <returns>Whether or not the data is probably contained in the filter.</returns>
        bool Test(T data);
        /// <summary>
        /// Add will add the data to the Bloom filter. It returns the filter to allow
        /// for chaining.
        /// </summary>
        /// <param name="data">The data to add.</param>
        /// <returns>The filter.</returns>
        IFilter<T> Add(T data);
        /// <summary>
        /// Is equivalent to calling Test followed by Add. It returns true if the data is
        /// a member, false if not.
        /// </summary>
        /// <param name="data">The data to test for and add if it doesn't exist.</param>
        /// <returns>Whether or not the data was probably contained in the filter.</returns>
        bool TestAndAdd(T data);
    }
}
