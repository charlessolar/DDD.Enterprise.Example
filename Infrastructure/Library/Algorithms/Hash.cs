using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Algorithms
{
    // https://github.com/mattlorimor/ProbabilisticDataStructures/blob/master/ProbabilisticDataStructures/Hash.cs
    public class Hash
    {
        internal HashAlgorithm HashAlgorithm { get; private set; }
        private string _hash;

        /// <summary>
        /// Create a new Hash object that will use the specified hashing algorithm.
        /// </summary>
        /// <param name="hashAlgorithm">The desired
        /// System.Security.Cryptography.HashAlgorithm.</param>
        public Hash(HashAlgorithm hashAlgorithm)
        {
            HashAlgorithm = hashAlgorithm;
        }

        /// <summary>
        /// Compute the hash for the provided bytes.
        /// </summary>
        /// <param name="inputBytes">The bytes to hash.</param>
        /// <returns>The hash string of the bytes.</returns>
        public string ComputeHash(byte[] inputBytes)
        {
            // Compute the hash of the input byte array.
            byte[] data = HashAlgorithm.ComputeHash(inputBytes);

            // Create a new StringBuilder to collect the bytes and create a string.
            StringBuilder sb = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a
            // hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            _hash = sb.ToString();
            return _hash;
        }

        public byte[] Sum()
        {
            var bytes = StringToByteArray(_hash);
            var uint64 = BitConverter.ToUInt64(bytes, 0);
            return new byte[]{
                ShiftRight(uint64, 56),
                ShiftRight(uint64, 48),
                ShiftRight(uint64, 40),
                ShiftRight(uint64, 32),
                ShiftRight(uint64, 24),
                ShiftRight(uint64, 16),
                ShiftRight(uint64, 8),
                ShiftRight(uint64, 0)
                };
        }

        private byte ShiftRight(UInt64 n, int amount)
        {
            return (byte)(n >> amount);
        }

        private byte[] StringToByteArray(String hex)
        {
            // http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
