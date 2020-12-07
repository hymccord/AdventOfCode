#nullable enable
using System.Linq;

namespace AdventOfCode.Solutions.Year2019 {

    class Day04 : ASolution {
        int low;
        int high;

        public Day04() : base(4, 2019, "") {
            int[] input = Input.Split('-').Select(int.Parse).ToArray();
            low = input[0];
            high = input[1];
        }

        protected override object SolvePartOne() {
            int valid = 0;
            for (int i = low; i <= high; i++)
            {
                bool doubled = false;
                bool increasing = true;

                int previousDigit = i % 10;
                int num = i / 10;
                while (num > 0 && increasing)
                {
                    int curDigit = num % 10;

                    if (curDigit == previousDigit)
                        doubled = true;

                    increasing &= curDigit <= previousDigit;

                    previousDigit = curDigit;
                    num /= 10;
                }

                if (doubled && increasing)
                    valid++;
            }

            return valid.ToString();
        }

        protected override object SolvePartTwo() {
            int valid = 0;
            for (int i = low; i <= high; i++)
            {
                bool doubled = false;
                bool increasing = true;

                int previousDigit = i % 10;
                int num = i / 10;
                while (num > 0 && increasing)
                {
                    int curDigit = num % 10;

                    int extraDigits = 0;
                    while (curDigit == previousDigit)
                    {
                        extraDigits++;
                        previousDigit = curDigit;
                        num /= 10;
                        curDigit = num % 10;
                    }

                    if (extraDigits == 1)
                        doubled = true;

                    increasing &= curDigit <= previousDigit;

                    previousDigit = curDigit;
                    num /= 10;
                }

                if (doubled && increasing)
                    valid++;
            }

            return valid.ToString();
        }
    }
}
