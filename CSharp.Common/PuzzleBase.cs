namespace AdventOfCode.CSharp.Common;

public abstract class PuzzleBase
{
    public PuzzleBase()
    {
        Run = RunWithoutBenchmarks;
        RunExample = Run_DoNothing;
    }

    public void SetOptions(bool benchmarks, bool examples)
    {
        if (benchmarks)
            Run = RunWithBenchmarks;

        if (examples)
            RunExample = Run;
    }


    public abstract IEnumerable<string> SolvePuzzle();


    private string RunWithoutBenchmarks(Func<string> method)
    {
        var text = method.Invoke();
        return text;
    }
    private string RunWithBenchmarks(Func<string> method)
    {
        var benchmark = new Stopwatch();
        benchmark.Start();
        var text = RunWithoutBenchmarks(method);
        benchmark.Stop();
        return text + $" - {benchmark.ElapsedMilliseconds} ms";
    }
    private string Run_DoNothing(Func<string> method) => null;

    protected Func<Func<string>, string> Run = (f) => null;
    protected Func<Func<string>, string> RunExample = (f) => null;

}
