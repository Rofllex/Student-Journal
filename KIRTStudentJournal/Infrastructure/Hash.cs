using System.Text;
using System.Security.Cryptography;

namespace KIRTStudentJournal.Infrastructure
{
    public static class Hash
    {
        private static readonly HashAlgorithm _hashAlgorithm = SHA256.Create();
        /// <summary>
        /// Получение хэша из строки.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string GetHashFromString(string inputString)
        {
            byte[] buffer = _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            string str = string.Empty;
            for (int i = 0; i < buffer.Length; i++)
                str += buffer[i].ToString("x2");
            return str;
        }
    }
}
