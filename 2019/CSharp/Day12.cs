namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/12
/// </summary>
public class Day12 : TestBase
{
    public Day12(ITestOutputHelper output) : base(output, 12) { }


    private (List<string>, long?) GetTestData(string name, int part)
    {
        var input = InputHelper.LoadInputFile(DAY, name)
            .ToList();

        var expected = InputHelper.LoadAnswerFile(DAY, part, name)
            ?.FirstOrDefault()
            ?.ToInt64();

        return (input, expected);
    }


    [Theory]
    [InlineData("example1", 10)]
    [InlineData("example2", 100)]
    [InlineData("input", 1000)]
    public void Part1(string inputName, int numOfSteps)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var result = RunPart1(input, numOfSteps);
        output.WriteLine($"Total Energy: {result}");

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("example1")]
    [InlineData("example2")]
    [InlineData("input")]
    public void Part2(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 2);

        var result = RunPart2(input);
        output.WriteLine($"Number of steps to return to original state: {result}");

        Assert.Equal(expected, result);
    }


    long RunPart1(List<string> input, int numOfSteps)
    {
        var bodies = BuildBodyList(input);

        for (var step = 1; step <= numOfSteps; step++)
        {
            bodies.ForEach(b => ApplyGravity(b, bodies));
            bodies.ForEach(ApplyVelocity);
        }

        var result = bodies.Sum(b => b.TotalEnergy);
        return result;
    }

    long RunPart2(List<string> input)
    {
        var bodies = BuildBodyList(input);
        var originalState = bodies.Select(b => b.GetCopy()).ToList();

        var xCount = 0L;
        var yCount = 0L;
        var zCount = 0L;
        CountSteps(bodies, originalState, ref xCount, ref yCount, ref zCount);

        var result = Helper.FindLeastCommonMultiple(
            xCount,
            Helper.FindLeastCommonMultiple(yCount, zCount));

        return result;
    }


    List<Body> BuildBodyList(IEnumerable<string> input)
    {
        var rx = new Regex(@"<x=(?<x>.*), y=(?<y>.*), z=(?<z>.*)>");

        var bodies = new List<Body>();
        foreach (var item in input)
        {
            var match = rx.Match(item);
            var p3D = new Point3D(
                (int)Convert.ChangeType(match.Groups[1].Value, typeof(int)),
                (int)Convert.ChangeType(match.Groups[2].Value, typeof(int)),
                (int)Convert.ChangeType(match.Groups[3].Value, typeof(int))
            );
            bodies.Add(new Body(p3D));
        }

        return bodies;
    }

    void ApplyGravity(Body current, List<Body> input)
    {
        var others = input.Where(b => !b.Equals(current));
        foreach (var o in others)
        {
            current.Velocity.X += CalculateVelocity(current.Position.X, o.Position.X);
            current.Velocity.Y += CalculateVelocity(current.Position.Y, o.Position.Y);
            current.Velocity.Z += CalculateVelocity(current.Position.Z, o.Position.Z);
        }
    }

    void ApplyVelocity(Body current)
    {
        current.Position.X += current.Velocity.X;
        current.Position.Y += current.Velocity.Y;
        current.Position.Z += current.Velocity.Z;
    }

    int CalculateVelocity(int a, int b)
    {
        if (a < b) return 1;
        if (a > b) return -1;
        return 0;
    }

    void ShowBodyInfo(Body b)
    {
        Console.WriteLine($"pot={b.Position}, vel={b.Velocity}");
    }

    private void CountSteps(List<Body> bodies, List<Body> originalState, ref long xCount, ref long yCount, ref long zCount)
    {
        var piX = typeof(Point3D).GetProperties().Where(pi => pi.Name == "X").First();
        var piY = typeof(Point3D).GetProperties().Where(pi => pi.Name == "Y").First();
        var piZ = typeof(Point3D).GetProperties().Where(pi => pi.Name == "Z").First();

        var i = 0L;
        do
        {
            i++;

            bodies.ForEach(b => ApplyGravity(b, bodies));
            bodies.ForEach(ApplyVelocity);

            var matches = new List<Tuple<bool, bool, bool>>();
            for (int k = 0; k < bodies.Count; k++)
            {
                matches.Add(new Tuple<bool, bool, bool>(
                    IsMatch(piX, originalState[k], bodies[k]),
                    IsMatch(piY, originalState[k], bodies[k]),
                    IsMatch(piZ, originalState[k], bodies[k])
                    ));
            }

            if (xCount == 0 && matches.All(m => m.Item1 == true))
                xCount = i;
            if (yCount == 0 && matches.All(m => m.Item2 == true))
                yCount = i;
            if (zCount == 0 && matches.All(m => m.Item3 == true))
                zCount = i;

        } while (xCount == 0 || yCount == 0 || zCount == 0);
    }

    bool IsMatch(PropertyInfo pi, Body original, Body current)
    {
        var p0 = (int)pi.GetValue(original.Position);
        var pN = (int)pi.GetValue(current.Position);

        var v0 = (int)pi.GetValue(original.Velocity);
        var vN = (int)pi.GetValue(current.Velocity);

        return (p0 == pN) && (v0 == vN);
    }



    class Body
    {
        public Body() : this(new Point3D()) { }
        public Body(Point3D position)
        {
            Position = position;
            Velocity = new Point3D();
        }

        public Point3D Position { get; private set; }
        public Point3D Velocity { get; private set; }


        public int TotalEnergy => PotentialEnergy * KineticEnergy;

        int PotentialEnergy => GetEnergy(Position);
        int KineticEnergy => GetEnergy(Velocity);

        int GetEnergy(Point3D p)
        {
            return Math.Abs(p.X)
                 + Math.Abs(p.Y)
                 + Math.Abs(p.Z);
        }

        public Body GetCopy()
        {
            var item = new Body();
            item.Position = Position.GetCopy();
            item.Velocity = Velocity.GetCopy();
            return item;
        }

    }



    class Point3D
    {
        public Point3D() : this(0, 0, 0) { }
        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int Energy => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);


        public override bool Equals(object obj)
        {
            return Equals(obj as Point3D);
        }
        public bool Equals(Point3D other)
        {
            if (other == null)
                return false;

            return X == other.X
                && Y == other.Y
                && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public override string ToString()
        {
            return $"<x={X,3}, y={Y,3}, z={Z,3}>";
        }

        public Point3D GetCopy()
        {
            return new Point3D(X, Y, Z);
        }
    }

}
