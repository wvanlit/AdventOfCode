using System.Text;
using AoC.Shared;
using Xunit;
using Xunit.Abstractions;

namespace AoC._2024;

public class Day9(ITestOutputHelper outputHelper) :
    SolutionBase(year: 2024, day: 9, runTest: false, outputHelper: outputHelper)
{
    private record struct Block(long Index, int Len, long? Id)
    {
        public bool IsFile => Id.HasValue;
    }

    private static List<Block> Parse(string input)
    {
        var chars = input.ToCharArray();
        var result = new List<Block>();
        var id = 0;
        var fileIndex = 0;

        for (var i = 0; i < chars.Length; i++)
        {
            var len = int.Parse(chars[i].ToString());
            if (i % 2 == 0) // Even equals file
            {
                result.Add(new(fileIndex, len, id++));
            }
            else // Odd equals empty space
            {
                result.Add(new(fileIndex, len, null));
            }

            fileIndex += len;
        }

        return result;
    }

    private string CurrentState(
        IEnumerable<Block> files,
        IEnumerable<Block> emptySpaces,
        IEnumerable<Block> movedBlocks)
    {
        var result = files
            .Concat(movedBlocks)
            .Concat(emptySpaces)
            .OrderBy(b => b.Index)
            .ToList();

        var sb = new StringBuilder();
        foreach (var r in result)
        {
            if (r.IsFile)
                sb.Append(r.Id!.Value.ToString()[0], r.Len);
            else
                sb.Append('.', r.Len);
        }

        return sb.ToString();
    }

    public override async Task<Answer> Part1(string input)
    {
        var blocks = Parse(input);

        var files = new Stack<Block>(blocks.Where(b => b.IsFile));
        var emptySpaces = new Queue<Block>(blocks.Where(b => !b.IsFile));
        var movedBlocks = new List<Block>();
        
        OutputHelper.WriteLine($"Blocks: {blocks.Count}");
        OutputHelper.WriteLine($"Files: {files.Count}");
        OutputHelper.WriteLine($"Empty spaces: {emptySpaces.Count}");

        while (emptySpaces.TryDequeue(out var nextEmpty))
        {
            if (!files.Any(f => f.Index >= nextEmpty.Index))
            {
                continue;
            }
            
            var newIndex = nextEmpty.Index;
            var remainingLength = nextEmpty.Len;
            while (remainingLength > 0)
            {
                nextEmpty = nextEmpty with { Len = remainingLength, Index = newIndex};
                
                WriteIfTest(() => CurrentState(files, emptySpaces.Prepend(nextEmpty), movedBlocks));

                var currentFile = files.Pop();
                if (currentFile.Len <= remainingLength)
                {
                    movedBlocks.Add(currentFile with { Index = newIndex });
                }
                else
                {
                    var fills = currentFile.Len - remainingLength;

                    movedBlocks.Add(currentFile with { Index = newIndex, Len = remainingLength });
                    files.Push(currentFile with { Len = fills });
                }

                newIndex += currentFile.Len; // Overshooting is fine, we'll reset on the next iteration
                remainingLength -= currentFile.Len;
            }
        }
        
        WriteIfTest(CurrentState(files, emptySpaces, movedBlocks));

        Assert.Empty(emptySpaces);

        var flatList = files
            .Concat(movedBlocks)
            .OrderBy(b => b.Index)
            .SelectMany(block => Enumerable.Range(0, block.Len).Select(i => block.Id!.Value));
        
        WriteIfTest(string.Join("", flatList));
        
        var checksum = flatList.Select((b, idx) => b * idx).Sum();
        
        return checksum;
    }

    public override async Task<Answer> Part2(string input)
    {
        return Answer.Failed;
    }
}