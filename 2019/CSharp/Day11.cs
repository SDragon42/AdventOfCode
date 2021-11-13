using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;
using AdventOfCode.CSharp.Year2019.IntCodeComputer;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/11
    /// </summary>
    class Day11 : PuzzleBase
    {
        const int DAY = 11;

        
        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 11: Space Police";

            yield return string.Empty;
            yield return Run(Part1);

            yield return string.Empty;
            yield return Run(Part2);
        }

        string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"));
        string Part2() => "Part 2) " + RunPart2(GetPuzzleData(2, "input"));



        class InputAnswer : IntCodeInputAnswer<int?>
        {
            public InputAnswer(List<string> input, int? expectedAnswer1 = null, IEnumerable<string> expectedAnswer2 = null)
            {
                Input = input;
                ExpectedAnswer = expectedAnswer1;
                if (expectedAnswer2 != null)
                    ExpectedAnswer2 = string.Join("\r\n", expectedAnswer2);
            }

            public string ExpectedAnswer2 { get; private set; }
        }
        InputAnswer GetPuzzleData(int part, string name)
        {
            var result = part switch
            {
                1 => new InputAnswer(
                    InputHelper.LoadInputFile(DAY, name).ToList(),
                    expectedAnswer1: InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt32()
                    ),
                2 => new InputAnswer(
                    InputHelper.LoadInputFile(DAY, name).ToList(),
                    expectedAnswer2: InputHelper.LoadAnswerFile(DAY, part, name)
                    ),
                _ => throw new ApplicationException($"Invalid part ({part}) value")
            };
            return result;
        }



        string RunPart1(InputAnswer puzzleData)
        {
            var hull = new Dictionary<Point, HullColor>();
            var robot = new HullPaintingRobot(puzzleData.Code);
            robot.ScanHull += (s, e) => e.HullColor = GetHullColor(hull, e.Location);
            robot.PaintHull += (s, e) => SetHullColor(hull, e.Location, e.HullColor);

            robot.Start();
            var result = hull.Count();

            return Helper.GetPuzzleResultText($"Hull panels painted: {result}", result, puzzleData.ExpectedAnswer);
        }

        string RunPart2(InputAnswer puzzleData)
        {
            var hull = new Dictionary<Point, HullColor>();
            var robot = new HullPaintingRobot(puzzleData.Code);

            SetHullColor(hull, robot.CurrentLocation, HullColor.White);

            robot.ScanHull += (s, e) => e.HullColor = GetHullColor(hull, e.Location);
            robot.PaintHull += (s, e) => SetHullColor(hull, e.Location, e.HullColor);

            robot.Start();
            var compositeImage = Helper.DrawPointGrid2D(hull, DrawPanel);//.TrimEnd();


            return Environment.NewLine + Helper.GetPuzzleResultText(compositeImage, compositeImage, puzzleData.ExpectedAnswer2);
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

                var args = new MoveToHullLocationEventArgs(myLocation);
                //MoveOnHull(this, args);
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
