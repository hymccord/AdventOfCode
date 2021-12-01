namespace AdventOfCode.Solutions.Year2018
{
    class Day14 : ASolution
    {
        public Day14() : base(14, 2018)
        {

        }

        protected override object SolvePartOne()
        {
            return string.Join("", MakeRecipes(int.Parse(Input)));
        }

        private IEnumerable<int> MakeRecipes(int num)
        {
            List<int> recipes = new List<int> { 3, 7 };
            int elfOneIndex = 0;
            int elfTwoIndex = 1;

            while (recipes.Count < num + 10)
            {
                int newRecipe = recipes[elfOneIndex] + recipes[elfTwoIndex];
                if (newRecipe > 9)
                {
                    recipes.Add(1);
                }
                recipes.Add(newRecipe % 10);

                elfOneIndex = (elfOneIndex + recipes[elfOneIndex] + 1) % recipes.Count;
                elfTwoIndex = (elfTwoIndex + recipes[elfTwoIndex] + 1) % recipes.Count;
            }

            return recipes.Skip(num).Take(10);
        }

        static List<int> scores = new List<int> { 7, 9, 3, 0, 3, 1 };
        //static List<int> scores = new List<int> { 5,1,5,8,9 };

        protected override object SolvePartTwo()
        {
            List<int> recipes = new List<int> { 3, 7 };
            int index = 0;
            int elfOneIndex = 0;
            int elfTwoIndex = 1;

            bool found = false;
            while (!found)
            {
                int newRecipe = recipes[elfOneIndex] + recipes[elfTwoIndex];
                if (newRecipe > 9)
                {
                    recipes.Add(1);
                }
                recipes.Add(newRecipe % 10);

                elfOneIndex = (elfOneIndex + recipes[elfOneIndex] + 1) % recipes.Count;
                elfTwoIndex = (elfTwoIndex + recipes[elfTwoIndex] + 1) % recipes.Count;

                int pos = 0;
                while (index + pos < recipes.Count)
                {
                    if (scores[pos] == recipes[index + pos])
                    {
                        if (pos == 5)
                        {
                            found = true;
                            break;
                        }
                        pos++;
                    }
                    else
                    {
                        pos = 0;
                        index++;
                    }
                }
            }

            return index;
        }

    }
}
