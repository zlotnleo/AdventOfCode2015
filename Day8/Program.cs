using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
    class Program
    {
        private static int Part1(ICollection<string> stringsInCode)
        {
            var lengthInCode = stringsInCode.Sum(s => s.Length);
            var lengthInMemory = stringsInCode.Sum(s =>
            {
                var len = 0;
                for (var i = 1; i < s.Length - 1; len++)
                {
                    if (s[i] != '\\')
                    {
                        i++;
                        continue;
                    }

                    if (i + 1 >= s.Length - 1)
                    {
                        throw new ArgumentOutOfRangeException(nameof(stringsInCode));
                    }

                    switch (s[i + 1])
                    {
                        case '\\':
                        case '\"':
                            i += 2;
                            break;
                        case 'x':
                            if (i + 3 >= s.Length - 1)
                            {
                                throw new ArgumentOutOfRangeException(nameof(stringsInCode));
                            }
                            if ((s[i + 2] >= '0' && s[i + 2] <= '9' || s[i + 2] >= 'a' && s[i + 2] <= 'f')
                                && (s[i + 3] >= '0' && s[i + 3] <= '9' || s[i + 3] >= 'a' && s[i + 3] <= 'f'))
                            {
                                i += 4;
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException(nameof(stringsInCode));
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(stringsInCode));
                    }
                }

                return len;
            });

            return lengthInCode - lengthInMemory;
        }

        private static int Part2(ICollection<string> strings)
        {
            var originalLength = strings.Sum(s => s.Length);
            var encodedLength = strings.Sum(s => 2 + s.Length + s.Count(c => c is '\"' or '\\'));
            return encodedLength - originalLength;
        }

        public static void Main()
        {
            var strings = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(strings));
            Console.WriteLine(Part2(strings));
        }
    }
}
