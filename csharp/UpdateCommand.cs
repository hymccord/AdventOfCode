using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spectre.Console.Cli;

namespace AdventOfCode;
internal class UpdateCommand : AsyncCommand<YearAndDayCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, YearAndDayCommandSettings settings)
    {
        await Task.CompletedTask;

        int day = Math.Min(25, settings.Day ?? DateTime.Now.Day);
        int year = settings.Year ?? DateTime.Now.Year;
        // If it's before December 1st, use the previous year
        if (DateTime.Now.Month < 12)
        {
            year--;
        }

        string yearDir = Path.Combine("Solutions", $"Year{year}");
        if (!Directory.Exists(yearDir))
        {
            Directory.CreateDirectory(yearDir);
        }

        string dayFile = Path.Combine(yearDir, $"Day{day:D2}.cs");
        if (!Directory.Exists(dayFile))
        {
            await File.WriteAllTextAsync(dayFile, GenerateTemplate(year, day));
        }

        return 0;
    }

    private static string GenerateTemplate(int year, int day)
    {
        return $$"""
            namespace AdventOfCode.Solutions.Year{{year}};

            internal class Day{{day:D2}} : ASolution
            {
                public Day{{day:D2}}() : base({{day:D2}}, {{year}}, "", true)
                { }

                protected override object SolvePartOne()
                {
                    return null;
                }

                protected override object SolvePartTwo()
                {
                    return null;
                }

                protected override IEnumerable<ExampleInput> LoadExampleInput()
                {
                    return [
                        ];
                }
            }
            """;
    }
}

internal class YearAndDayCommandSettings : CommandSettings
{
    [CommandOption("-y|--year")]
    public int? Year { get; set; }
    [CommandOption("-d|--day")]
    public int? Day { get; set; }
}
