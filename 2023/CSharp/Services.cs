namespace AdventOfCode.CSharp.Year2023;

internal static class Services
{
    private const string INPUT_ROOT_PATH = @"../../../../../../AdventOfCode.Input/2023";

    public static IInputReaderService Input { get; } = new InputReaderService(INPUT_ROOT_PATH);
}
