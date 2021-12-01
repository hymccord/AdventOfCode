using AdventOfCode.Solutions;

namespace AdventOfCode
{

    class Program
    {

        public static Config Config = Config.Get("config.json");

        static void Main(string[] args)
        {
            SolutionCollector Solutions = new SolutionCollector(Config.Year, new HashSet<int>(Config.Days));

            foreach (ASolution solution in Solutions)
            {
                solution.Solve();
            }
        }
    }
}
