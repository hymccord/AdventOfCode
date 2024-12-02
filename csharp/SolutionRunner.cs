using AdventOfCode.Options;
using AdventOfCode.Solutions;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Spectre.Console.Cli;

namespace AdventOfCode;

internal class SolutionRunnerCommand : Spectre.Console.Cli.AsyncCommand
{
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

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        IEnumerable<ISolution> solutions = _solutionCollector.GetSolutions();

        if (!solutions.Any())
        {
            _logger.LogWarning("No solutions to run.");
            return 1;
        }

        foreach (ISolution solution in solutions)
        {
            await solution.RunAsync(_session, _hostApplicationLifetime.ApplicationStopping);
        }

        return 0;
    }
}

//public abstract class CancellableAsyncCommand : AsyncCommand
//{
//    private readonly ConsoleAppCancellationTokenSource _cancellationTokenSource = new();

//    public abstract Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellation);

//    public sealed override async Task<int> ExecuteAsync(CommandContext context)
//        => await ExecuteAsync(context, _cancellationTokenSource.Token);

//}

//public abstract class CancellableAsyncCommand<TSettings> : AsyncCommand<TSettings>
//    where TSettings : CommandSettings
//{
//    private readonly ConsoleAppCancellationTokenSource _cancellationTokenSource = new();

//    public abstract Task<int> ExecuteAsync(CommandContext context, TSettings settings, CancellationToken cancellation);

//    public sealed override async Task<int> ExecuteAsync(CommandContext context, TSettings settings)
//        => await ExecuteAsync(context, settings, _cancellationTokenSource.Token);
//}
