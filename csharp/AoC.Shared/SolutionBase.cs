using Microsoft.Extensions.Configuration;
using Snapshooter;
using Snapshooter.Xunit;
using Spectre.Console;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace AoC.Shared;

public abstract class SolutionBase
{
    public ITestOutputHelper OutputHelper { get; }
    public string Input { get; }
    public string Name { get; }
    
    public bool IsTest { get; }
    
    protected SolutionBase(int year, int day, bool runTest, ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
        // TODO: Add support for multiple input files
        // TODO: Add support for multiple configuration files
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.local.json", true)
            .Build();
        
        Input = InputLoader.Load(config["Inputs"]!, year.ToString(), day.ToString(), runTest);
        
        Name = $"{year}_{day:D2}{(runTest ? "_test" : "")}".Trim();
        
        IsTest = runTest;
    }
    
    public abstract Task<Answer> Part1(string input);

    public abstract Task<Answer> Part2(string input);
    
    [Fact]
    public async Task RunPart1()
    {
        var answer = await Part1(Input);
        
        Assert.NotNull(answer);
        Assert.NotEqual(Answer.Failed, answer);
        Assert.NotEqual("-1", answer.ToString());
        
        OutputHelper.WriteLine($"Part 1: {answer}");
        
        Snapshot.Match(answer, new SnapshotNameExtension(Name + "_p1"));
    }
    
    [Fact]
    public async Task RunPart2()
    {
        var answer = await Part2(Input);
        
        Assert.NotNull(answer);
        Assert.NotEqual(Answer.Failed, answer);
        Assert.NotEqual("-1", answer.ToString());
        
        OutputHelper.WriteLine($"Part 2: {answer}");
        
        Snapshot.Match(answer, new SnapshotNameExtension(Name + "_p2"));
    }

    protected void WriteIfTest(string message)
    {
        if (IsTest)
        {
            OutputHelper.WriteLine(message);
        }
    }
    
    /// <summary>
    /// Used to lazily write to the output.
    /// Does not run func if not in test mode.
    /// </summary>
    protected void WriteIfTest(Func<string> messageFunc)
    {
        if (IsTest)
        {
            OutputHelper.WriteLine(messageFunc());
        }
    }
}