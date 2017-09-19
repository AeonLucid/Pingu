using System;
using System.Security.Cryptography;
using System.Text;

namespace Pingu.Util
{
    internal static class Hash
    {
        public static string Md5(string input)
        {
            using (var hasher = new MD5CryptoServiceProvider())
            {
                var bytes = hasher.ComputeHash(Encoding.ASCII.GetBytes(input));

                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
