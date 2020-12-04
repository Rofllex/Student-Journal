using System.Security.Cryptography;
using System.Text;

namespace Journal.Server.Security
{
    public static class Hash
    {
        private static readonly HashAlgorithm _hashAlgorithm = SHA256.Create();

        /// <summary>
        /// Получить хэш из входного набора байт.
        /// </summary>
        public static byte[] Get(byte[] bytes) => _hashAlgorithm.ComputeHash(bytes);

        /// <summary>
        /// Получить хэш строку из входной строки.
        /// </summary>
        /// <param name="encoding">Кодировка строки. Если равна null, то будет использована <see cref="Encoding.UTF8"/></param>
        public static string GetString(string inputString, Encoding encoding = null)
        {
            byte[] bytes = Get(encoding?.GetBytes(inputString) ?? Encoding.UTF8.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                sb.Append(bytes[i].ToString("x2"));
            return sb.ToString();
        }
    }
}
