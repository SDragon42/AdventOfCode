using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Advent_of_Code.IntCodeComputer;

namespace Advent_of_Code.Day11
{
    class EHPRobot
    {
        readonly IIntCodeV3 brain = new IntCodeV3();

        public EHPRobot()
        {
            brain.OnOutput += Brain_OnOutput;
        }

        int outputCount = 0;
        Point myLocation = new Point();
        MyDirection myDir = MyDirection.Up;


        public event EventHandler<ScanHullColorEventArgs> ScanHull;
        public event EventHandler<PaintHullColorEventArgs> PaintHull;
        //public event EventHandler<MoveToHullLocationEventArgs> MoveOnHull;



        public void Init(IEnumerable<long> brainCode)
        {
            brain.Init(brainCode);
        }

        public void Start()
        {
            while (brain.State != IntCodeState.Finished || brain.State == IntCodeState.Error)
            {
                brain.Run();
                if (brain.State == IntCodeState.NeedsInput)
                {
                    var args = new ScanHullColorEventArgs(myLocation);
                    ScanHull(this, args);
                    if (args.HullColor == HullColor.Undefined)
                        throw new ApplicationException("Hull Paint color was not read!");
                    brain.AddInput((long)args.HullColor);
                }
            }
        }

        //HullColor? paintColor;
        //TurnDirection? turnDir;

        private void Brain_OnOutput(object sender, OutputEventArgs e)
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
            switch(this.myDir)
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
