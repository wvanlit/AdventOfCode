using System.Collections;
using System.Text;
using AoC.Shared;
using MoreLinq;
using Xunit;
using Xunit.Abstractions;

namespace AoC._2024;

public class Day9(ITestOutputHelper outputHelper) :
    SolutionBase(year: 2024, day: 9, runTest: false, outputHelper: outputHelper)
{
    private record DiskObject(long StartsAtIndex, long BlockLength)
    {
        public virtual string ToShortString() => $"DO({BlockLength}) @ {StartsAtIndex}";

        public record File(long StartsAtIndex, long BlockLength, long Id) : DiskObject(StartsAtIndex, BlockLength)
        {
            public override string ToString() =>
                string.Join("", Enumerable.Range(0, (int)BlockLength).Select(_ => Id.ToString()));

            public override string ToShortString() => $"{Id}({BlockLength}) @ {StartsAtIndex}";
            
            public Empty ToEmpty() => new(StartsAtIndex, BlockLength);
        }

        public record Empty(long StartsAtIndex, long BlockLength) : DiskObject(StartsAtIndex, BlockLength)
        {
            public override string ToString() =>
                string.Join("", Enumerable.Range(0, (int)BlockLength).Select(_ => "."));

            public override string ToShortString() => $"Empty({BlockLength}) @ {StartsAtIndex}";
        }
    }

    private static List<DiskObject> Parse(string input)
    {
        var digits = input.ToCharArray().Select(c => c.ToString()).Select(int.Parse).ToArray();
        var result = new List<DiskObject>();
        var id = 0;
        var fileIndex = 0;

        for (var i = 0; i < digits.Length; i++)
        {
            var len = digits[i];

            if (i % 2 == 0) // Even equals file
            {
                result.Add(new DiskObject.File(fileIndex, len, id++));
            }
            else // Odd equals empty space
            {
                result.Add(new DiskObject.Empty(fileIndex, len));
            }

            fileIndex += len;
        }

        return result;
    }

    private void PrintDiskMap(IEnumerable<DiskObject.File> files, IEnumerable<DiskObject.Empty> emptySpace,
        bool includeIndex = false) =>
        WriteIfTest(() =>
        {
            var list = new List<DiskObject>();

            list.AddRange(files);
            list.AddRange(emptySpace);
            list.Sort((a, b) => a.StartsAtIndex.CompareTo(b.StartsAtIndex));

            var result = string.Join("", list.Select(d => d.ToString()));
            if (!includeIndex) return result;

            return "R: " + result + "\nI: " + string.Join("", result.Select((c, i) => i.ToString().Last()));
        });

    private List<DiskObject.File> CompactPerBlock(List<DiskObject> diskMap)
    {
        var files = new List<DiskObject.File>(diskMap.OfType<DiskObject.File>());
        var emptyBlocks = new Queue<DiskObject.Empty>(diskMap.OfType<DiskObject.Empty>());

        while (emptyBlocks.TryDequeue(out var empty))
        {
            // Empty block is after all files
            if (files.All(f => f.StartsAtIndex < empty.StartsAtIndex)) continue;

            WriteIfTest(() => $"\nEmpty: {empty.ToShortString()}");

            while (empty.BlockLength > 0)
            {
                if (files.All(f => f.StartsAtIndex < empty.StartsAtIndex)) break;

                PrintDiskMap(files, emptyBlocks.Prepend(empty), true);

                var file = files.Last();

                if (file.BlockLength == empty.BlockLength) // file fills exactly
                {
                    var prepend = file with { StartsAtIndex = empty.StartsAtIndex };

                    WriteIfTest(() => $"Exact: {prepend.ToShortString()}");

                    files = files.SkipLast(1).Prepend(prepend).ToList();
                }
                else if (file.BlockLength < empty.BlockLength) // file underfills
                {
                    var prepend = file with { StartsAtIndex = empty.StartsAtIndex };

                    WriteIfTest(() => $"Underfill: {prepend.ToShortString()}");

                    files = files.SkipLast(1).Prepend(prepend).ToList();
                }
                else if (file.BlockLength > empty.BlockLength) // file overfills
                {
                    var diff = file.BlockLength - empty.BlockLength;

                    var prepend = file with { StartsAtIndex = empty.StartsAtIndex, BlockLength = empty.BlockLength };
                    var append = file with { BlockLength = diff };

                    WriteIfTest(() => $"Overfill: {prepend.ToShortString()} & {append.ToShortString()}");

                    files = files.SkipLast(1).Prepend(prepend).Append(append).ToList();
                }
                else
                {
                    Assert.True(false, "Invalid State");
                }

                empty = empty with
                {
                    BlockLength = empty.BlockLength - file.BlockLength,
                    StartsAtIndex = empty.StartsAtIndex + file.BlockLength
                };
            }
        }

        Assert.Empty(emptyBlocks);

        return files.OrderBy(f => f.StartsAtIndex).ToList();
    }

    private long Checksum(IEnumerable<DiskObject.File> files)
    {
        var expanded = files.SelectMany(f => Enumerable.Range(0, (int)f.BlockLength).Select(_ => f.Id)).ToArray();

        WriteIfTest(() => string.Join("", expanded));

        return expanded.Select((id, index) => id * (long)index).Sum();
    }

    public override async Task<Answer> Part1(string input)
    {
        WriteIfTest(input);

        var diskMap = Parse(input);

        var compactedFiles = CompactPerBlock(diskMap);

        PrintDiskMap(compactedFiles, []);

        return Checksum(compactedFiles);
    }

    [Fact]
    public async Task SampleDiskMap()
    {
        var input = "12345";

        WriteIfTest(input);

        var diskMap = Parse(input);

        WriteIfTest("\nCompacting:");
        var compactedFiles = CompactPerBlock(diskMap);

        var expanded = string.Join("", compactedFiles.Select(d => d.ToString()));

        WriteIfTest("\nExpanded:");
        WriteIfTest(expanded);
        foreach (var file in compactedFiles)
        {
            WriteIfTest(file.ToShortString());
        }


        Assert.Equal("022111222", expanded);
    }

    private List<DiskObject> CompactPerFile(List<DiskObject> diskMap)
    {
        var files = new List<DiskObject.File>(diskMap.OfType<DiskObject.File>());
        var emptyBlocks = new Queue<DiskObject.Empty>(diskMap.OfType<DiskObject.Empty>());
        var emptyBlocksToKeep = new List<DiskObject.Empty>();

        while (emptyBlocks.TryDequeue(out var empty))
        {
            if (empty.BlockLength == 0) continue;
            
            // Empty block is after all files
            if (files.All(f => f.StartsAtIndex < empty.StartsAtIndex))
            {
                emptyBlocksToKeep.Add(empty);
                continue;
            }

            WriteIfTest(() => $"\nEmpty: {empty.ToShortString()}");

            while (empty.BlockLength > 0)
            {
                if (files.All(f => f.StartsAtIndex < empty.StartsAtIndex))
                {
                    // No file fits in the (remaining) empty block
                    emptyBlocksToKeep.Add(empty);
                    break;
                }

                PrintDiskMap(files, emptyBlocks.Prepend(empty), true);

                var file = files
                    .Where(f => f.StartsAtIndex > empty.StartsAtIndex)
                    .LastOrDefault(f => f.BlockLength <= empty.BlockLength);


                if (file == default)
                {
                    // No file fits in the (remaining) empty block
                    emptyBlocksToKeep.Add(empty);
                    break;
                }
                
                files.Remove(file);

                if (file.BlockLength == empty.BlockLength) // file fills exactly
                {
                    var prepend = file with { StartsAtIndex = empty.StartsAtIndex };
                    WriteIfTest(() => $"Exact: {prepend.ToShortString()}");

                    files = files.Prepend(prepend).ToList();
                    emptyBlocksToKeep.Add(file.ToEmpty());
                }
                else if (file.BlockLength < empty.BlockLength) // file underfills
                {
                    var prepend = file with { StartsAtIndex = empty.StartsAtIndex };
                    WriteIfTest(() => $"Underfill: {prepend.ToShortString()}");

                    files = files.Prepend(prepend).ToList();
                    emptyBlocksToKeep.Add(file.ToEmpty());
                }
                else // Overfill is not possible in Part 2
                {
                    Assert.True(false, "Invalid State");
                }

                empty = empty with
                {
                    BlockLength = empty.BlockLength - file.BlockLength,
                    StartsAtIndex = empty.StartsAtIndex + file.BlockLength
                };
            }
        }

        Assert.Empty(emptyBlocks);

        return files.Cast<DiskObject>()
            .Concat(emptyBlocksToKeep)
            .OrderBy(f => f.StartsAtIndex)
            .ToList();
    }
    
    private long Checksum(IEnumerable<DiskObject> objects)
    {
        var expanded = objects.SelectMany(f => Enumerable.Range(0, (int)f.BlockLength).Select(_ => f)).ToArray();

        WriteIfTest(() => string.Join("\n", expanded.Select(s => s.ToShortString())));

        return expanded
            .Select((obj, index) => (obj, index))
            .Where(t => t.obj is DiskObject.File)
            .Select(t => ((DiskObject.File)t.obj).Id * t.index)
            .Sum();
    }

    public override async Task<Answer> Part2(string input)
    {
        WriteIfTest(input);

        var diskMap = Parse(input);

        var compactedFiles = CompactPerFile(diskMap);

        PrintDiskMap(compactedFiles.OfType<DiskObject.File>(), compactedFiles.OfType<DiskObject.Empty>());

        return Checksum(compactedFiles);
    }
}