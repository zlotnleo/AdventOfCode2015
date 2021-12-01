using System;
using System.Collections.Generic;
using System.Linq;

namespace Day20
{
    class Program
    {
        private static List<int> GetPrimesUpTo(int n)
        {
            var sieve = Enumerable.Repeat(true, n - 1).ToArray();
            var limit = Math.Floor(Math.Sqrt(n));
            for (var i = 2; i <= limit; i++)
            {
                if (sieve[i - 2])
                {
                    for (var j = i * i; j <= n; j+=i)
                    {
                        sieve[j - 2] = false;
                    }
                }
            }

            var primes = new List<int>();
            for (var i = 0; i < sieve.Length; i++)
            {
                if (sieve[i])
                {
                    primes.Add(i + 2);
                }
            }

            return primes;
        }

        private static IEnumerable<(int prime, int power)> GetFactorisation(int n, IList<int> primes)
        {
            var factorisation = new List<(int, int)>();
            for (var i = 0; i < primes.Count && primes[i] <= n; i++)
            {
                var count = 0;
                while (n % primes[i] == 0)
                {
                    n /= primes[i];
                    count++;
                }

                if(count > 0) {
                    factorisation.Add((primes[i], count));
                }
            }

            return factorisation;
        }

        private static IEnumerable<int> GetDivisors(ICollection<(int prime, int power)> factorisation)
        {
            if (!factorisation.Any())
            {
                return new[] {1};
            }

            var (prime, power) = factorisation.First();
            var rest = factorisation.Skip(1).ToList();
            var divisorsOfRest = GetDivisors(rest).ToList();

            var divisors = new List<int>();
            var currentMultiple = 1;
            for (var pow = 0; pow <= power; pow++)
            {
                divisors.AddRange(divisorsOfRest.Select(d => d * currentMultiple));
                currentMultiple *= prime;
            }

            return divisors.ToList();
        }

        private static int Part1(int presents)
        {
            var sumOfElfNumbers = presents / 10;
            var primes = GetPrimesUpTo(sumOfElfNumbers);
            for (var houseNumber = 1;; houseNumber++)
            {
                var factorisation = GetFactorisation(houseNumber, primes).ToList();
                var sumDivisors = GetDivisors(factorisation).Sum();
                if (sumDivisors >= sumOfElfNumbers)
                {
                    return houseNumber;
                }
            }
        }

        private static int Part2(int presents)
        {
            var sumOfElfNumbers = presents / 11;
            var primes = GetPrimesUpTo(sumOfElfNumbers);
            for (var houseNumber = 1;; houseNumber++)
            {
                var factorisation = GetFactorisation(houseNumber, primes).ToList();
                var sumDivisors = GetDivisors(factorisation)
                    .Where(divisor => houseNumber / divisor <= 50)
                    .Sum();
                if (sumDivisors >= sumOfElfNumbers)
                {
                    return houseNumber;
                }
            }
        }

        public static void Main()
        {
            const int presents = 36000000;
            Console.WriteLine(Part1(presents));
            Console.WriteLine(Part2(presents));
        }
    }
}
