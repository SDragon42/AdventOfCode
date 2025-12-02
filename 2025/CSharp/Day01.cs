namespace AdventOfCode.CSharp.Year2025;

public class Day01(ITestOutputHelper output)
{
    private const int DAY = 1;



    private (IList<string> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
                                      .ToList();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                         ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (rotations, expected) = GetTestData(part, inputName);

        var value = GetNumberOfPositionsAt0(rotations);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (rotations, expected) = GetTestData(part, inputName);

        var value = GetNumberOfClicksAtPosition0(rotations);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    const int DIAL_MIN = 0;
    const int DIAL_MAX = 99;
    const int DIAL_START = 50;


    private int GetNumberOfPositionsAt0(IEnumerable<string> rotations)
    {
        var count = 0;
        var position = DIAL_START;
        foreach (var rotation in rotations)
        {
            var direction = rotation[0];
            var amount = int.Parse(rotation[1..]) * direction switch
            {
                'L' => -1,
                'R' => 1,
                _ => throw new InvalidOperationException($"Invalid rotation direction: {direction}"),
            };

            position = RotateDial(position, amount, out _);

            if (position == 0)
            {
                count++;
            }
        }
        return count;
    }

    private int GetNumberOfClicksAtPosition0(IEnumerable<string> rotations)
    {
        var count = 0;
        var position = DIAL_START;
        foreach (var rotation in rotations)
        {
            var direction = rotation[0];
            var amount = int.Parse(rotation[1..]) * direction switch
            {
                'L' => -1,
                'R' => 1,
                _ => throw new InvalidOperationException($"Invalid rotation direction: {direction}"),
            };

            position = RotateDial(position, amount, out int numberOfTimesAtZero);

            count += numberOfTimesAtZero;
        }
        return count;
    }

    private int RotateDial(int position, int amount, out int numberOfTimesAtZero)
    {
        numberOfTimesAtZero = 0;
        var newPosition = position;

        var step = Math.Sign(amount);
        amount = Math.Abs(amount);

        while (amount > 0)
        {
            newPosition += step;
            if (newPosition < DIAL_MIN)
            {
                newPosition = DIAL_MAX;
            }
            else if (newPosition > DIAL_MAX)
            {
                newPosition = DIAL_MIN;
            }
            if (newPosition == 0)
            {
                numberOfTimesAtZero++;
            }
            amount--;
        }

        return newPosition;
    }
    private int RotateDialWithModularArithmatic(int position, int amount, out int numberOfTimesAtZero)
    {
        var N = DIAL_MAX - DIAL_MIN + 1;
        var absAmount = Math.Abs(amount);
        numberOfTimesAtZero = 0;

        // Determine how many steps until we first hit 0 (k >= 1)
        int firstK = (amount > 0) 
            ? (N - position % N) % N // For positive steps we want k such that (position + k) % N == 0
            : position % N;          // For negative steps we want k such that (position - k) % N == 0 => k % N == position % N

        if (firstK == 0)
        {
            // If already at 0 then the next time we return to 0 is after a full cycle
            firstK = N;
        }

        if (absAmount >= firstK)
        {
            numberOfTimesAtZero = 1 + ((absAmount - firstK) / N);
        }

        // Compute new position using modulo arithmetic and normalize to 0..N-1
        var normalized = ((position + (amount % N)) % N + N) % N;
        return normalized;
    }

}
