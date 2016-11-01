using System.Security.Cryptography;

namespace Demo.Library.Algorithms
{
    // https://github.com/mattlorimor/ProbabilisticDataStructures/blob/master/ProbabilisticDataStructures/Defaults.cs
    public static class Defaults
    {
        public const double FillRatio = 0.5;

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
