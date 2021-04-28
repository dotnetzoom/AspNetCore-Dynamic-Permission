using System.Security.Cryptography;
using System.Text;

namespace DynamicPermission.CQRS.AppCode
{
    public static class SecurityExtensions
    {
        public static string GetMd5Hash(this string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }
    }
}
