using System.Collections;
//using System.Composition.Convention;
//using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Options;

namespace AdventOfCode.Solutions
{
    internal interface ISolutionCollector
    {
        IEnumerable<ISolution> GetSolutions();
    }
    class SolutionCollector : ISolutionCollector
    {
        private readonly IOptions<Config> _config;

        public SolutionCollector(IOptions<Config> config)
        {
            _config = config;
        }

        public IEnumerable<ISolution> GetSolutions()
        {
            var allSolutions = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(ISolution)))
                .Select(Activator.CreateInstance)
                .Cast<ISolution>();

            IEnumerable<ISolution> filteredSolutions = allSolutions;
            if (_config.Value.Year is int year)
            {
                filteredSolutions = filteredSolutions.Where(s => s.Year == year);
            }

            if (_config.Value.Days is int[] days && days.Length > 0)
            {
                var set = new HashSet<int>(days);
                filteredSolutions = filteredSolutions.Where(s => set.Contains(s.Day));
            }
            else
            {
                filteredSolutions = filteredSolutions.Where(s => s.Day == DateTime.Now.Day);
            }

            return filteredSolutions;
        }
    }
}
