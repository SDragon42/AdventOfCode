namespace AdventOfCode.CSharp.Year2025;

internal static class TestServices
{
    private const string INPUT_ROOT_PATH = @"../../../../../../AdventOfCode.Input/2025";

    public static IInputReaderService Input { get; } = new InputReaderService(INPUT_ROOT_PATH);
}
