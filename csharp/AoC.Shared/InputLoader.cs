namespace AoC.Shared;

public static class InputLoader
{
    public static string Load(
        string inputPath,
        string year,
        string day,
        bool loadExample = false)
    {
        var fileExtension = loadExample ? ".test" : "";
        var inputFile = Path.Combine(inputPath, year, day + fileExtension + ".txt");
        
        return File.ReadAllText(inputFile).Trim();
    }
}