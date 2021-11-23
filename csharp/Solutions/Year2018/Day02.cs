
using System;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{
    class Day2 : ASolution
    {
        public Day2() : base(2, 2018, "Inventory Management System")
        {
        }

        protected sealed override object SolvePartOne()
        {
            return "";
        }

        protected sealed override object SolvePartTwo()
        {
            var ids = Input.SplitByNewline();
            for (int i = 0; i < ids.Length - 1; i++)
            {
                for (int j = i + 1; j < ids.Length; j++)
                {
                    if (ids[i].ToCharArray().Zip(ids[j].ToCharArray(), (char1, char2) => char1 == char2).Count(b => !b) == 1)
                    {
                        var x = ids[i].ToCharArray().Zip(ids[j].ToCharArray(), (char1, char2) => char1 == char2).ToArray();
                        return new string(ids[i].Where((c, index) => x[index]).ToArray());
                    }
                }
            }

            throw new NotImplementedException();
        }

        const string input = @"abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz";

        
    }
}
