#nullable enable
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Options;

namespace AdventOfCode.Solutions;

internal interface ISolutionCollector
{
    ISolution? GetSolution(int? year, int? day);
}
class SolutionCollector : ISolutionCollector
{
    private readonly IEnumerable<ISolution> _solutions;

    public SolutionCollector(IEnumerable<ISolution> solutions)
    {
        _solutions = solutions;
    }

    public ISolution? GetSolution(int? year, int? day)
    {
        IEnumerable<ISolution> solutions = _solutions;
        if (year is int)
        {
            solutions = solutions.Where(s => s.Year == year);
        }
        else
        {
            solutions = solutions.Where(s => s.Year == DateTime.Now.Year);
        }

        if (day is int)
        {
            solutions = solutions.Where(s => s.Day == day);
        }
        else
        {
            solutions = solutions.Where(s => s.Day == DateTime.Now.Day);
        }

        return solutions.FirstOrDefault();
    }
}
