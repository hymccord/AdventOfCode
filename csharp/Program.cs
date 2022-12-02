using AdventOfCode;
using AdventOfCode.Options;
using AdventOfCode.Solutions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config)=>
    {
        config.AddUserSecrets<Program>();
        config.AddJsonFile("appsettings.local.json");
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<ISolutionCollector, SolutionCollector>();
        services.AddHostedService<SolutionRunner>();

        services.Configure<Config>(context.Configuration.GetSection(nameof(Config)));
        services.Configure<Session>(options => options.Token = context.Configuration.GetValue<string>("session"));
    })
    .UseConsoleLifetime(options =>
    {
        options.SuppressStatusMessages = true;
    })
    .Build()
    .RunAsync();
