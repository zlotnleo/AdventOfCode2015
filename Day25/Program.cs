using System;

namespace Day25
{
    class Program
    {
        private static long GetCodeNumber(long row, long col)
        {
            // the diagonal that (row, col) is on is nth where n = row + col - 1
            // the number of codes on all diagonals up to and including nth is n * (n + 1) / 2
            // and then subtract (row - 1), codes on the last diagonal after (row, col)
            return (row + col - 1) * (row + col) / 2 - row + 1;
        }

        private static long ModularPower(long b, long e, long m)
        {
            var result = 1L;
            while (e > 0)
            {
                if ((e & 1) == 1)
                {
                    result = result * b % m;
                }

                e >>= 1;
                b = b * b % m;
            }

            return result;
        }

        public static void Main()
        {
            const long row = 2978;
            const long col = 3083;
            var codeNumber = GetCodeNumber(row, col);

            const long initialCode = 20151125;
            const long multiple = 252533;
            const long divisor = 33554393;
            var result = initialCode * ModularPower(multiple, codeNumber - 1, divisor) % divisor;
            Console.WriteLine(result);
        }
    }
}
