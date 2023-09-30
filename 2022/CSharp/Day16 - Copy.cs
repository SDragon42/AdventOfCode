using System.Collections.ObjectModel;

namespace AdventOfCode.CSharp.Year2022;

public class Day16_Copy
{
    private const int DAY = 16;

    private readonly ITestOutputHelper output;
    public Day16_Copy(ITestOutputHelper output) => this.output = output;



    private (IDictionary<string, (int flowRate, string[] toValues)> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
                .Select(str => (str[6..8],
                                int.Parse(str[23..25].Trim(';')),
                                (str.Contains("valves") ? str.Split("valves ")[1] : str.Split("valve ")[1]).Split(", ").ToArray()
                        ))
                .ToDictionary(tp => tp.Item1, tp => (tp.Item2, tp.Item3));

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetMostPreasureReleased(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    record struct State(int time, int currentValve, int currentFlow, uint toOpen);

    const int MINUTES = 29;
    const int MAX = 10000;

    int _nonZeroCount;
    Dictionary<int, (int flow, List<int> edges)> _indexGraph = new();
    int[,] _allPairs = new int[0,0];


    private int GetMostPreasureReleased(IDictionary<string, (int flowRate, string[] toValves)> stringGraph)
    {
        _nonZeroCount = stringGraph
            .Where(kvp => kvp.Value.flowRate != 0)
            .Count();

        var stringToIndex = stringGraph
            .OrderByDescending(kvp => kvp.Value.flowRate)
            .Select((kvp, i) => (kvp.Key, i))
            .ToDictionary(tp => tp.Key, tp => tp.i);

        _indexGraph = stringGraph
            .ToDictionary(
                kvp => stringToIndex[kvp.Key],
                kvp => (
                        kvp.Value.flowRate,
                        kvp.Value.toValves.Select(str => stringToIndex[str])
                        .ToList()
                        )
            );

        //get all pairs shortest paths using Floyd-Warshall
        //note this is the DISTANCE only so still need to take into account time to open the valve
        _allPairs = FloydWarshall(_indexGraph);

        //partition the non-zero valves into all possible disjoint sets
        //skipping 1 bit to ensure that we don't count both (A,B) and (B,A)
        var sets = new List<(uint set1, uint set2)>();
        for (uint i = 0; i < Math.Pow(2, _nonZeroCount - 1); i++)
        {
            (var set1, var set2) = (0U, 0U);
            for (int b = 0; b < _nonZeroCount; b++)
                if (IsBitSet(b, i))
                    set1 = SetBit(b, set1);
                else
                    set2 = SetBit(b, set2);
            sets.Add((set1, set2));
        }

        //for each pair of disjoint sets, recursively DFS each side starting at valve AA
        //at each level of the search, we only need to consider moving to each valve which remains open
        //Find the max of the sum of the pressure released for all disjoint pairs
        var maximums = sets
            .AsParallel()
            .Select(pair => (
                        Solve(pair.set1, stringToIndex["AA"]), 
                        Solve(pair.set2, stringToIndex["AA"])))
            .ToList();

        var result = maximums.Max(maximum => maximum.Item1 + maximum.Item2);
        return result;

    }


    private int Solve(uint toOpen, int startValve)
    {
        int currentBest = 0;
        return Solve2(new Dictionary<State, int>(), ref currentBest, new State(0, startValve, 0, toOpen));
    }

    private int Solve2(Dictionary<State, int> cache, ref int currentBest, State state)
    {
        if (cache.TryGetValue(state, out int result))
        {
            return result;
        }

        //calculate upper limit of pressure released assuming all remaining valves are opened in min time for any valve
        //also remove any valves from our set for where it isn't possible to open them
        //and form a priority queue based on the total pressure released by moving to that valve next
        var queue = new PriorityQueue<int, int>();
        int maxFlow = 0;
        for (int b = 0; b < _nonZeroCount; b++)
        {
            if (!IsBitSet(b, state.toOpen))
                continue;
            if (_allPairs[state.currentValve, b] + state.time + 1 < MINUTES)
            {
                var additionalFlow = (MINUTES - (_allPairs[state.currentValve, b] + state.time + 1)) * _indexGraph[b].flow;
                maxFlow += additionalFlow;
                queue.Enqueue(b, -additionalFlow);
            }
            else
            {
                state.toOpen = ClearBit(b, state.toOpen);
            }
        }

        int best = state.currentFlow;
        //if we can beat the current global best for these sets then keep trying
        if (state.currentFlow + maxFlow > currentBest)
        {
            while (queue.TryDequeue(out int nextValve, out int negAdditionalFlow))
            {
                //next state is the minute after we have opened this valve
                int nextBest = Solve2(cache, ref currentBest, state with
                {
                    time = _allPairs[state.currentValve, nextValve] + state.time + 1,
                    currentFlow = state.currentFlow - negAdditionalFlow,
                    currentValve = nextValve,
                    toOpen = ClearBit(nextValve, state.toOpen)
                });

                best = Math.Max(best, nextBest);
                currentBest = Math.Max(best, currentBest);
            }
        }

        cache[state] = best;
        return best;
    }
    private bool IsBitSet(int bit, uint state) => (state & (1U << bit)) != 0;
    private uint SetBit(int bit, uint state) => state |= 1U << bit;
    private uint ClearBit(int bit, uint state) => state &= ~(1U << bit);

    private static int[,] FloydWarshall(Dictionary<int, (int flow, List<int> edges)> graph)
    {
        int[,] distance = new int[graph.Count, graph.Count];

        for (int i = 0; i < graph.Count; i++)
            for (int j = 0; j < graph.Count; j++)
                distance[i, j] = MAX;

        foreach (var key in graph.Keys)
            foreach (var edge in graph[key].edges)
                distance[key, edge] = 1;

        for (int k = 0; k < graph.Count; k++)
        {
            for (int i = 0; i < graph.Count; i++)
            {
                for (int j = 0; j < graph.Count; j++)
                {
                    var newDist = distance[i, k] + distance[k, j];
                    if (newDist < distance[i, j])
                        distance[i, j] = newDist;
                }
            }
        }

        return distance;
    }

}