using AdventOfCode.Solutions;

namespace AdventOfCode
{

    class Program
    {

        public static Config Config = Config.Get("config.json");

        static void Main(string[] args)
        {
            var solutions = new SolutionCollector(Config.Year, new HashSet<int>(Config.Days));

            if (!solutions.Any())
            {
                Console.WriteLine("Nothing found to run.");
            }

            foreach (ASolution solution in solutions)
            {
                solution.Solve();
            }
        }
    }
}
