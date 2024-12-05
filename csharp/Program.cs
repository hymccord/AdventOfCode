using AdventOfCode;
using AdventOfCode.Options;
using AdventOfCode.Solutions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Spectre.Console.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddTransient<ISolutionCollector, SolutionCollector>();
builder.Services.Scan(static scan => scan
    .FromAssemblyOf<ISolution>()
    .AddClasses(static classes => classes.AssignableTo<ISolution>())
        .As<ISolution>()
        .WithTransientLifetime());
builder.Services.AddSpectreConsole<SolutionRunnerCommand>(args, configurator =>
{
    configurator.AddCommand<UpdateCommand>("update");
});

builder.Services.Configure<Session>(builder.Configuration.GetSection(nameof(Session)));
builder.Services.Configure<ConsoleLifetimeOptions>(static opts => opts.SuppressStatusMessages = true);

var host = builder.Build();

await host.RunAsync();
