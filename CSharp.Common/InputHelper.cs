namespace AdventOfCode.CSharp.Common;

public static class InputHelper
{
    public static IEnumerable<string> LoadInputFile(int day, string name)
    {
        var filename = $@".\input\Day{day:00}\{name}.txt";
        return File.ReadLines(filename);
    }
    public static IEnumerable<string> LoadAnswerFile(int day, int part, string name)
    {
        try
        {
            var filename = $@".\input\Day{day:00}\{name}-answer{part}.txt";
            return File.ReadLines(filename);
        }
        catch { return null; }
    }
}
