using System.Security.Cryptography;
using System.Text;

namespace Server.Security
{
    public static class Hash
    {
        private static readonly HashAlgorithm hashAlgorithm = SHA256.Create();

        public static byte[] Get(byte[] bytes) => hashAlgorithm.ComputeHash(bytes);

        public static string GetString(string inputString, Encoding encoding = null)
        {
            byte[] bytes = encoding?.GetBytes(inputString) ?? Encoding.UTF8.GetBytes(inputString);
            bytes = Get(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
