using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Advent_of_Code.Day12
{
    /*
    --- Part Two ---
    All this drifting around in space makes you wonder about the nature of the universe. Does
    history really repeat itself? You're curious whether the moons will ever return to a previous
    state.

    Determine the number of steps that must occur before all of the moons' positions and 
    velocities exactly match a previous point in time.

    For example, the first example above takes 2772 steps before they exactly match a previous
    point in time; it eventually returns to the initial state:

    After 0 steps:
    pos=<x= -1, y=  0, z=  2>, vel=<x=  0, y=  0, z=  0>
    pos=<x=  2, y=-10, z= -7>, vel=<x=  0, y=  0, z=  0>
    pos=<x=  4, y= -8, z=  8>, vel=<x=  0, y=  0, z=  0>
    pos=<x=  3, y=  5, z= -1>, vel=<x=  0, y=  0, z=  0>

    After 2770 steps:
    pos=<x=  2, y= -1, z=  1>, vel=<x= -3, y=  2, z=  2>
    pos=<x=  3, y= -7, z= -4>, vel=<x=  2, y= -5, z= -6>
    pos=<x=  1, y= -7, z=  5>, vel=<x=  0, y= -3, z=  6>
    pos=<x=  2, y=  2, z=  0>, vel=<x=  1, y=  6, z= -2>

    After 2771 steps:
    pos=<x= -1, y=  0, z=  2>, vel=<x= -3, y=  1, z=  1>
    pos=<x=  2, y=-10, z= -7>, vel=<x= -1, y= -3, z= -3>
    pos=<x=  4, y= -8, z=  8>, vel=<x=  3, y= -1, z=  3>
    pos=<x=  3, y=  5, z= -1>, vel=<x=  1, y=  3, z= -1>

    After 2772 steps:
    pos=<x= -1, y=  0, z=  2>, vel=<x=  0, y=  0, z=  0>
    pos=<x=  2, y=-10, z= -7>, vel=<x=  0, y=  0, z=  0>
    pos=<x=  4, y= -8, z=  8>, vel=<x=  0, y=  0, z=  0>
    pos=<x=  3, y=  5, z= -1>, vel=<x=  0, y=  0, z=  0>

    Of course, the universe might last for a very long time before repeating. Here's a copy of
    the second example from above:

    <x=-8, y=-10, z=0>
    <x=5, y=5, z=10>
    <x=2, y=-7, z=3>
    <x=9, y=-8, z=-3>

    This set of initial positions takes 4686774924 steps before it repeats a previous state!
    Clearly, you might need to find a more efficient way to simulate the universe.

    How many steps does it take to reach the first state that exactly matches a previous state?
    */
    class Day12Puzzle2 : IPuzzle
    {
        public Day12Puzzle2()
        {
            #region Live Data
            bodies = new Body[] {
                new Body(14, 2, 8),
                new Body(7, 4, 10),
                new Body(1, 17, 16),
                new Body(-4, -1, 1),
            };
            numStepsToOriginalState = 0;
            #endregion

            #region Example 1
            //bodies = new Body[] {
            //    new Body(-1, 0, 2),
            //    new Body(2, -10, -7),
            //    new Body(4, -8, 8),
            //    new Body(3, 5, -1),
            //};
            //numStepsToOriginalState = 2772L;
            #endregion

            #region Example 2
            //bodies = new Body[] {
            //    new Body(-8, -10, 0),
            //    new Body(5, 5, 10),
            //    new Body(2, -7, 3),
            //    new Body(9, -8, -3),
            //};
            //numStepsToOriginalState = 4686774924L;
            #endregion
        }

        readonly IReadOnlyList<Body> bodies;
        readonly long numStepsToOriginalState;

        readonly List<Body> originalState = new List<Body>();

        public void Run()
        {
            Console.WriteLine("--- Day 12: The N-Body Problem (part two) ---");

            Display(0);

            originalState.Clear();
            originalState.Add(bodies[0].GetCopy());
            originalState.Add(bodies[1].GetCopy());
            originalState.Add(bodies[2].GetCopy());
            originalState.Add(bodies[3].GetCopy());

            var sw = new Stopwatch();
            sw.Start();

            var xCount = 0L;
            var yCount = 0L;
            var zCount = 0L;
            CountSteps(ref xCount, ref yCount, ref zCount);

            var i = FindLCM(xCount, yCount, zCount);
            Console.WriteLine($"i:{i}  x:{xCount}  y:{yCount}  z:{zCount}");


            //var i = 0L;
            //var match = false;

            //do
            //{
            //    i++;
            //    bodies.ForEach(ApplyGravitiy);
            //    bodies.ForEach(ApplyVelocity);

            //    match = true;
            //    for (int k = 0; k < bodies.Count; k++)
            //        match &= originalState[k].Equals(bodies[k]);

            //} while (!match);
            //sw.Stop();

            Console.WriteLine($"Number of steps to return to original state: {i}");
            if (i == numStepsToOriginalState)
                Console.WriteLine("\tCorrect!");

            Console.WriteLine();
            Console.WriteLine($"Runtime: {sw.Elapsed:c}");
            Console.WriteLine();
        }

        private long FindLCM(long xCount, long yCount, long zCount)
        {
            return Helper.FindLCM(xCount, Helper.FindLCM(yCount, zCount));
        }

        private void CountSteps(ref long xCount, ref long yCount, ref long zCount)
        {
            var piX = typeof(Point3D).GetProperties().Where(pi => pi.Name == "X").First();
            var piY = typeof(Point3D).GetProperties().Where(pi => pi.Name == "Y").First();
            var piZ = typeof(Point3D).GetProperties().Where(pi => pi.Name == "Z").First();

            var i = 0L;
            do
            {
                i++;

                bodies.ForEach(ApplyGravitiy);
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


        int calcVelocityComponent(int me, int other)
        {
            if (me < other) return 1;
            if (me > other) return -1;
            return 0;
        }

        void ApplyGravitiy(Body body)
        {
            var otherBodies = bodies.Where(b => b != body);
            foreach (var other in otherBodies)
            {
                body.Velocity.X += calcVelocityComponent(body.Position.X, other.Position.X);
                body.Velocity.Y += calcVelocityComponent(body.Position.Y, other.Position.Y);
                body.Velocity.Z += calcVelocityComponent(body.Position.Z, other.Position.Z);
            }
        }
        void ApplyVelocity(Body body)
        {
            body.Position.X += body.Velocity.X;
            body.Position.Y += body.Velocity.Y;
            body.Position.Z += body.Velocity.Z;
        }

        void Display(int i)
        {
            Console.WriteLine($"After {i} step{(i != 1 ? "s" : "")}:");
            bodies.ForEach(b => Console.WriteLine(b));
            Console.WriteLine();
        }

    }

}
