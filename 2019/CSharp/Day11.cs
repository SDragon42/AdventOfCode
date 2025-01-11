using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode.Common.Extensions;
using AdventOfCode.Common.Helpers;
using AdventOfCode.CSharp.Year2019.IntCodeComputer;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/11
    /// </summary>
    public class Day11 : TestBase
    {
        public Day11(ITestOutputHelper output) : base(output, 11) { }


        private List<long> GetInput(string name)
        {
            var input = Input.ReadLines(DAY, name)
                .First()
                .Split(',')
                .Select(v => v.ToInt64())
                .ToList();
            return input;
        }

        private int? GetPart1Expected(string name)
        {
            var expected = Input.ReadLines(DAY, $"{name}-answer1")
                ?.FirstOrDefault()
                ?.ToInt32();
            return expected;
        }

        private string GetPart2Expected(string name)
        {
            var expected = Input.ReadLines(DAY, $"{name}-answer2");
            return string.Join("\r\n", expected);
        }



        [Fact]
        public void Part1()
        {
            var input = GetInput("input");
            var expected = GetPart1Expected("input");

            var hull = new Dictionary<Point, HullColor>();
            var robot = new HullPaintingRobot(input);
            robot.ScanHull += (s, e) => e.HullColor = GetHullColor(hull, e.Location);
            robot.PaintHull += (s, e) => SetHullColor(hull, e.Location, e.HullColor);

            robot.Start();
            var result = hull.Count;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2()
        {
            var input = GetInput("input");
            var expected = GetPart2Expected("input");

            var hull = new Dictionary<Point, HullColor>();
            var robot = new HullPaintingRobot(input);

            SetHullColor(hull, robot.CurrentLocation, HullColor.White);

            robot.ScanHull += (s, e) => e.HullColor = GetHullColor(hull, e.Location);
            robot.PaintHull += (s, e) => SetHullColor(hull, e.Location, e.HullColor);

            robot.Start();
            var compositeImage = GridHelper.DrawPointGrid2D(hull, DrawPanel);

            Assert.Equal(expected, compositeImage);
        }

        HullColor GetHullColor(IDictionary<Point, HullColor> hull, Point location)
        {
            if (!hull.ContainsKey(location))
                hull.Add(location, HullColor.Black);
            return hull[location];
        }

        void SetHullColor(IDictionary<Point, HullColor> hull, Point location, HullColor color)
        {
            if (hull.ContainsKey(location))
                hull[location] = color;
            else
                hull.Add(location, color);
        }

        string DrawPanel(HullColor panelColor)
        {
            var result = panelColor switch
            {
                HullColor.White => "#",
                _ => " "
            };
            return result;
        }



        enum HullColor
        {
            Undefined = -1,
            Black = 0,
            White = 1,
        }



        class HullPaintingRobot
        {
            readonly IntCode cpu;
            int outputCount = 0;
            Point myLocation = new Point();
            MyDirection myDir = MyDirection.Up;



            public HullPaintingRobot(IEnumerable<long> code)
            {
                cpu = new IntCode(code);
                cpu.Output += Cpu_Output;
            }



            public event EventHandler<ScanHullColorEventArgs> ScanHull;
            public event EventHandler<PaintHullColorEventArgs> PaintHull;



            public Point CurrentLocation => myLocation;



            public void Start()
            {
                while (cpu.State != IntCodeState.Finished)
                {
                    cpu.Run();
                    if (cpu.State == IntCodeState.NeedsInput)
                    {
                        var args = new ScanHullColorEventArgs(myLocation);
                        ScanHull(this, args);
                        if (args.HullColor == HullColor.Undefined)
                            throw new ApplicationException("Hull Paint color was not read!");
                        cpu.AddInput((long)args.HullColor);
                    }
                }
            }

            void Cpu_Output(object sender, IntCodeOutputEventArgs e)
            {
                outputCount++;
                switch (outputCount)
                {
                    case 1: DoPaintHull((HullColor)e.OutputValue); break;
                    case 2: DoRotateAndMove((TurnDirection)e.OutputValue); break;
                    default: break;
                }

                if (outputCount == 2)
                    outputCount = 0;
            }

            void DoPaintHull(HullColor color)
            {
                var args = new PaintHullColorEventArgs(myLocation, color);
                PaintHull(this, args);
            }
            void DoRotateAndMove(TurnDirection direction)
            {
                Rotate(direction);
                Move();
            }
            void Rotate(TurnDirection direction)
            {
                var offset = (int)direction;
                if (offset == 0)
                    offset = -1;

                var newDir = (int)myDir + offset;
                if (newDir < (int)MyDirection.Up)
                    myDir = MyDirection.Left;
                else if (newDir > (int)MyDirection.Left)
                    myDir = MyDirection.Up;
                else
                    myDir = (MyDirection)newDir;
            }
            void Move()
            {
                Size shift;
                switch (this.myDir)
                {
                    case MyDirection.Up: shift = new Size(0, 1); break;
                    case MyDirection.Right: shift = new Size(1, 0); break;
                    case MyDirection.Down: shift = new Size(0, -1); break;
                    case MyDirection.Left: shift = new Size(-1, 0); break;
                    default: throw new ApplicationException("Direction not implemented!");
                }

                myLocation = Point.Add(myLocation, shift);

                //var args = new MoveToHullLocationEventArgs(myLocation);
            }


            enum MyDirection
            {
                Up = 0,
                Right = 1,
                Down = 2,
                Left = 3,
            }


            enum TurnDirection
            {
                Left = 0,
                Right = 1,
            }
        }



        class ScanHullColorEventArgs : EventArgs
        {
            public ScanHullColorEventArgs(Point location) : base()
            {
                Location = location;
                HullColor = HullColor.Undefined;
            }

            public Point Location { get; private set; }
            public HullColor HullColor { get; set; }
        }



        class PaintHullColorEventArgs : EventArgs
        {
            public PaintHullColorEventArgs(Point location, HullColor color) : base()
            {
                Location = location;
                HullColor = color;
            }

            public Point Location { get; private set; }
            public HullColor HullColor { get; private set; }
        }



        class MoveToHullLocationEventArgs : EventArgs
        {
            public MoveToHullLocationEventArgs(Point moveToPoint) : base()
            {
                MoveTo = moveToPoint;
            }

            public Point MoveTo { get; private set; }
        }

    }
}
