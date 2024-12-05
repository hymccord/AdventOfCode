using AdventOfCode.Options;
using AdventOfCode.Solutions;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Spectre.Console.Cli;

namespace AdventOfCode;

internal class SolutionRunnerCommand : Spectre.Console.Cli.AsyncCommand<SolutionRunnerCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-y|--year [year]")]
        public FlagValue<int> Year { get; set; }

        [CommandOption("-d|--day [day]")]
        public FlagValue<int> Day { get; set; }
    }

    private readonly ILogger<SolutionRunnerCommand> _logger;
    private readonly IOptions<Session> _session;
    private readonly ISolutionCollector _solutionCollector;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public SolutionRunnerCommand(ILogger<SolutionRunnerCommand> logger, IOptions<Session> session, ISolutionCollector solutionCollector, IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _session = session;
        _solutionCollector = solutionCollector;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        ISolution solution = _solutionCollector.GetSolution(settings.Year.IsSet ? settings.Year.Value : null, settings.Day.IsSet ? settings.Day.Value : null);

        if (solution is null)
        {
            _logger.LogWarning("No solution to run.");
            return 1;
        }

        await solution.RunAsync(_session, _hostApplicationLifetime.ApplicationStopping);

        return 0;
    }
}

