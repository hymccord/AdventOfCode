using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode.Options;
using AdventOfCode.Solutions;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdventOfCode;

internal class ConsoleRunner : IHostedService
{
    private readonly ILogger<ConsoleRunner> _logger;
    private readonly IOptions<Session> _session;
    private readonly ISolutionCollector _solutionCollector;

    public ConsoleRunner(ILogger<ConsoleRunner> logger,
        IOptions<Session> session,
        ISolutionCollector solutionCollector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _session = session;
        _solutionCollector = solutionCollector;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        IEnumerable<ISolution> solutions = _solutionCollector.GetSolutions();

        if (!solutions.Any())
        {
            _logger.LogWarning("No solutions to run.");
            return;
        }

        foreach (ISolution solution in solutions)
        {
            await solution.RunAsync(_session, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
