namespace AdventOfCode.CSharp.Year2022;

using ItemType = UInt64;

/// <remarks>
/// I received help from these sources:
/// https://github.com/jake-gordon/aoc/blob/main/2022/D11/Explanation.md
/// https://github.com/matheusstutzel/adventOfCode/blob/main/2022/11/p2.py
/// </remarks>
public class Day11_Monkey_in_the_Middle
{
    private const int DAY = 11;

    private readonly ITestOutputHelper output;
    public Day11_Monkey_in_the_Middle(ITestOutputHelper output) => this.output = output;



    private (string input, ItemType? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadText(DAY, inputName);

        var expectedTxt = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}");
        ItemType? expected = null;
        if (expectedTxt is not null)
            expected = ItemType.Parse(expectedTxt);

        return (input, expected);
    }




    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetLevelOfMonkeyBusiness(input, 20);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetLevelOfMonkeyBusiness(input, 10000, false);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    private List<Monkey> BuildMonkeyList(string input, bool defaultWorryReduction)
    {
        var parts = input.Split(Environment.NewLine + Environment.NewLine);

        var monkeyList = new List<Monkey>();
        var product = (ItemType)1;

        foreach (var def in parts)
        {
            var monkey = new Monkey();

            var lines = def.Split(Environment.NewLine);

            // Items
            var itemList = lines[1].Split(':')[1]
                .Split(',')
                .Select(ItemType.Parse);
            foreach (var item in itemList)
                monkey.HeldItems.Enqueue(item);

            // Operation
            var operationParts = lines[2].Split('=')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (operationParts[0] == operationParts[2])
            {
                monkey.OperationFunc = operationParts[1] switch
                {
                    "*" => (old) => old * old,
                    _ => (old) => old + old,
                };
            }
            else
            {
                monkey.OperationFunc = operationParts[1] switch
                {
                    "*" => (old) => old * ItemType.Parse(operationParts[2]),
                    _ => (old) => old + ItemType.Parse(operationParts[2]),
                };
            }

            // Test
            var divisibleBy = ItemType.Parse(lines[3].Split("divisible by")[1]);
            var trueMonkey = int.Parse(lines[4].AsSpan().Slice(29));
            var falseMonkey = int.Parse(lines[5].AsSpan().Slice(30));

            monkey.GetMonkeyIdToThrowTo = (old) => (old % divisibleBy == 0) ? trueMonkey : falseMonkey;

            product *= divisibleBy;

            // Worry Reduction
            monkey.WorryReductionFunc = (defaultWorryReduction)
                ? (i) => i / 3
                : (i) => i % product;

            monkeyList.Add(monkey);
        }

        foreach (var monkey in monkeyList)
            monkey.ThrowToMonkey = (ItemType item, int toMonkey) => monkeyList[toMonkey].HeldItems.Enqueue(item);

        return monkeyList;
    }


    private ItemType GetLevelOfMonkeyBusiness(string input, int numRounds, bool defaultWorryReduction = true)
    {
        var monkeys = BuildMonkeyList(input, defaultWorryReduction);

        for (int round = 1; round <= numRounds; round++)
        {
            for (int i = 0; i < monkeys.Count; i++)
            {
                var currentMonkey = monkeys[i];
                while (currentMonkey.HeldItems.Count > 0)
                {
                    currentMonkey.RunItemInspection();
                }
            }
        }

        var top2 = monkeys
            .Select(m => m.InspectionCount)
            .OrderByDescending(c => c)
            .Take(2)
            .ToArray();

        var result = (top2[0] * top2[1]);
        return result;
    }




    private delegate ItemType OperationDelegate(ItemType item);
    private delegate int TestDelegate(ItemType item);
    private delegate void ThrowToDelegate(ItemType item, int monkeyId);

    private class Monkey
    {
        public Queue<ItemType> HeldItems { get; private set; } = new Queue<ItemType>();

        public OperationDelegate OperationFunc { get; set; } = (a) => a;
        public OperationDelegate WorryReductionFunc { get; set; } = (a) => a;
        public TestDelegate GetMonkeyIdToThrowTo { get; set; } = (a) => 0;
        public ThrowToDelegate ThrowToMonkey { get; set; } = (a, b) => { };

        public ItemType InspectionCount { get; private set; } = 0;

        public void RunItemInspection()
        {
            if (HeldItems.Count == 0)
                return;

            InspectionCount = InspectionCount + 1;

            var item = HeldItems.Dequeue();
            item = OperationFunc(item);
            item = WorryReductionFunc(item);

            var monkeyId = GetMonkeyIdToThrowTo(item);

            ThrowToMonkey(item, monkeyId);
        }

    }
}