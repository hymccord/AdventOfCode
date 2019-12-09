using System; 
using System.Collections; 
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq; 
using System.Reflection; 

namespace AdventOfCode.Solutions {
    
    class SolutionCollector : IEnumerable<ISolution> {

        IEnumerable<ISolution> Solutions;

        public SolutionCollector(int year, HashSet<int> days) => Solutions = LoadSolutions(year, days);

        public ISolution GetSolution(int day) {
            try {
                return Solutions.Single(s => s.Day == day);
            } catch(InvalidOperationException) {
                return null; 
            }
        }

        public IEnumerator<ISolution> GetEnumerator() {
            return Solutions.GetEnumerator(); 
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator(); 
        }

        IEnumerable<ISolution> LoadSolutions(int year, HashSet<int> days)
        {

            var conventions = new ConventionBuilder();
            conventions.ForTypesDerivedFrom<ISolution>().ExportInterfaces();

            var configuration = new ContainerConfiguration()
                .WithAssembly(Assembly.GetExecutingAssembly(), conventions);

            using (var container = configuration.CreateContainer())
            {
                var e = container.GetExports<ISolution>()
                    .Where(i => i.Year == year && (days.Sum() == 0 || days.Contains(i.Day) ))
                    .OrderBy(i => i.Day);
                return e;
            }
        }
    }
}
