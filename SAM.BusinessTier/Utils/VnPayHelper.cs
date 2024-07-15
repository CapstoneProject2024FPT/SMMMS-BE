using System.Security.Cryptography;
using System.Text;

namespace SAM.BusinessTier.Utils
{
    public static class VnPayHelper
    {
        public static string GenerateSecureHash(string input, string key)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                var hex = new StringBuilder(hashValue.Length * 2);
                foreach (byte b in hashValue)
                    hex.AppendFormat("{0:x2}", b);
                return hex.ToString();
            }
        }
    }
}
