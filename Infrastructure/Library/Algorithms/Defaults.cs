using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Algorithms
{
    // https://github.com/mattlorimor/ProbabilisticDataStructures/blob/master/ProbabilisticDataStructures/Defaults.cs
    public static class Defaults
    {
        public const double FILL_RATIO = 0.5;

        /// <summary>
        /// Returns the default hashing algorithm for the library.
        /// </summary>
        /// <returns>The default hashing algorithm for the library</returns>
        internal static HashAlgorithm GetDefaultHashAlgorithm()
        {
            return HashAlgorithm.Create("MD5");
        }
    }
}
