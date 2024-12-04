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
    public string Input { get; set; }
    public string Name { get; }
    
    protected SolutionBase(int year, int day, bool runTest, ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
        // TODO: Add support for multiple input files
        // TODO: Add support for multiple configuration files
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        Input = InputLoader.Load(config["Inputs"]!, year.ToString(), day.ToString(), runTest);
        
        Name = $"{year}_{day:D2}{(runTest ? "_test" : "")}".Trim();
    }
    
    public abstract Task<Answer> Part1(string input);

    public abstract Task<Answer> Part2(string input);
    
    [Fact]
    public async Task RunPart1()
    {
        var answer = await Part1(Input);
        
        Assert.NotNull(answer);
        Assert.NotEqual(Answer.Failed, answer);
        
        OutputHelper.WriteLine($"Part 1: {answer}");
        
        Snapshot.Match(answer, new SnapshotNameExtension(Name + "_p1"));
    }
    
    [Fact]
    public async Task RunPart2()
    {
        var answer = await Part2(Input);
        
        Assert.NotNull(answer);
        Assert.NotEqual(Answer.Failed, answer);
        
        OutputHelper.WriteLine($"Part 2: {answer}");
        
        Snapshot.Match(answer, new SnapshotNameExtension(Name + "_p2"));
    }
}