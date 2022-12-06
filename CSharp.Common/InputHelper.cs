namespace AdventOfCode.CSharp.Common;

public static class InputHelper
{
    [Obsolete]
    public static IEnumerable<string> LoadInputFile(int day, string name)
    {
        var filename = GetDataFilePath(day, name);
        return File.ReadLines(filename);
    }

    [Obsolete]
    public static IEnumerable<string> LoadAnswerFile(int day, int part, string name)
    {
        try
        {
            var filename = GetDataFilePath(day, $"{name}-answer{part}");
            return File.ReadLines(filename);
        }
        catch { return null; }
    }


    public static string GetDataFilePath(int day, string name)
    {
        var filename = $@".\input\Day{day:00}\{name}.txt";
        return filename;
    }

    public static IEnumerable<string> ReadLines(string path)
    {
        try
        {
            return File.ReadLines(path);
        }
        catch { return null; }
    }

    public static string ReadText(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch { return null; }
    }
}
