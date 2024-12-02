using AdventOfCode;
using AdventOfCode.Options;
using AdventOfCode.Solutions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Spectre.Console.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
builder.Configuration.AddJsonFile("appsettings.local.json");

builder.Services.AddSingleton<ISolutionCollector, SolutionCollector>();
builder.Services.AddSpectreConsole<SolutionRunnerCommand>(args);

builder.Services.Configure<Config>(builder.Configuration.GetSection(nameof(Config)));
builder.Services.Configure<Session>(options => options.Token = builder.Configuration.GetValue<string>("session"));
builder.Services.Configure<ConsoleLifetimeOptions>(static opts => opts.SuppressStatusMessages = true);

var host = builder.Build();
await host.RunAsync();
