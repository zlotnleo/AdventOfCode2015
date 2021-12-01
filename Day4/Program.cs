using System;
using System.Security.Cryptography;
using System.Text;

namespace Day4
{
    class Program
    {
        private const string SecretKey = "ckczppom";

        private static string StringToMd5HexString(string input)
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = MD5.HashData(bytes);
            return Convert.ToHexString(hashBytes);
        }

        private static int GetNumberWithLeadingZeros(int numZeros)
        {
            var target = new string('0', numZeros);
            for (var i = 1;; i++)
            {
                if (StringToMd5HexString($"{SecretKey}{i}").StartsWith(target))
                {
                    return i;
                }
            }
        }

        public static void Main()
        {
            Console.WriteLine(GetNumberWithLeadingZeros(5));
            Console.WriteLine(GetNumberWithLeadingZeros(6));
        }
    }
}
