using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VersionedMethods.Algorithms;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IFirstAlgorithm, FirstAlgorithm>();
        services.AddSingleton<ISecondAlgorithm, SecondAlgorithm>();
        services.AddHostedService<Worker>();
    }).ConfigureLogging(log =>
    {
        log.AddFilter("Microsoft", level => level == LogLevel.Warning);
        log.AddConsole();
    })
    .Build();

await host.RunAsync();

public class Worker : BackgroundService
{
    private readonly IFirstAlgorithm _firstAlgorithm;

    private readonly ISecondAlgorithm _secondAlgorithm;

    private readonly IHost _host;

    private readonly ILogger<Worker> _logger;

    public Worker(IFirstAlgorithm firstAlgorithm, ISecondAlgorithm secondAlgorithm, IHost host, ILogger<Worker> logger)
    {
        _firstAlgorithm = firstAlgorithm;
        _secondAlgorithm = secondAlgorithm;
        _host = host;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopwatch = Stopwatch.StartNew();
        for (var i = 0; i < 1000; ++i)
        {
            RunFirstAlgorithmAndPrintResult(_firstAlgorithm, 0, 0);
            RunFirstAlgorithmAndPrintResult(_firstAlgorithm, 2, 2);
            RunFirstAlgorithmAndPrintResult(_firstAlgorithm, 1, 2);
            RunFirstAlgorithmAndPrintResult(_firstAlgorithm, 0, -1);
        }
        var firstTime = stopwatch.ElapsedTicks;
        
        stopwatch.Restart();
        for (var i = 0; i < 1000; ++i)
        {
            RunSecondAlgorithmAndPrintResult(_secondAlgorithm, 0, 0);
            RunSecondAlgorithmAndPrintResult(_secondAlgorithm, 2, 2);
            RunSecondAlgorithmAndPrintResult(_secondAlgorithm, 1, 2);
            RunSecondAlgorithmAndPrintResult(_secondAlgorithm, 0, -1);
        }
        var secondTime = stopwatch.ElapsedMilliseconds;
        
        _logger.LogInformation("IFirstAlgorithm time (ticks): {FirstTime}\nISecondAlgorithm time (ticks): {SecondTime}", firstTime, secondTime);
        return _host.StopAsync(stoppingToken);
    }
    
    private void RunFirstAlgorithmAndPrintResult(IFirstAlgorithm algorithm, int input, int version)
    {
        try
        {
            var result = algorithm.Run(input, version);
            // switch (result)
            // {
            //     case null:
            //         _logger.LogInformation("For input {Input} and version {Version}: null", input, version);
            //         break;
            //     default:
            //         _logger.LogInformation("For input {Input} and version {Version}: {Result}", input, version, result);
            //         break;
            // }
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "For input {Input} and version {Version}:", input, version);
        }
    }
    
    private void RunSecondAlgorithmAndPrintResult(ISecondAlgorithm algorithm, int input, int version)
    {
        try
        {
            var result = algorithm.Run(input, version);
            // switch (result)
            // {
            //     case null:
            //         _logger.LogInformation("For input {Input} and version {Version}: null", input, version);
            //         break;
            //     default:
            //         _logger.LogInformation("For input {Input} and version {Version}: {Result}", input, version, result);
            //         break;
            // }
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "For input {Input} and version {Version}:", input, version);
        }
    }
}