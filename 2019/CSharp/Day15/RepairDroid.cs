﻿using AdventOfCode.CSharp.Year2019.IntCodeComputer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode.CSharp.Year2019.Day15
{
    enum RepairDroidStatusCode
    {
        HitWall = 0,
        MovedOneStep = 1,
        MovedOneStep_AtDestination = 2,
    }


    enum MoveDirection
    {
        North = 1,
        South = 2,
        West = 3,
        East = 4,
    }


    class RepairDroid
    {
        IntCode brain;// = new IntCode();

        public RepairDroid(IEnumerable<long> brainCode)//Point startLocation)
        {
            brain = new IntCode(brainCode);
            brain.Output += Brain_OnOutput;
            //CurrenLocation = startLocation;
            LastMoveDirection = MoveDirection.North;
        }


        //public Point CurrenLocation { get; private set; }
        public MoveDirection LastMoveDirection { get; private set; }

        public event EventHandler<ReportStatusEventArgs> ReportStatus;
        public event EventHandler<RequestMovementDirEventArgs> RequestMovement;



        //public void Init(IEnumerable<long> brainCode)
        //{
        //    brain.Init(brainCode);
        //}

        public void Start()
        {
            while (brain.State != IntCodeState.Finished)// || brain.State == IntCodeState.Error)
            {
                brain.Run();
                if (brain.State == IntCodeState.NeedsInput)
                {
                    var args = new RequestMovementDirEventArgs(LastMoveDirection);
                    RequestMovement(this, args);
                    LastMoveDirection = args.Direction;
                    brain.AddInput((long)args.Direction);
                }
            }
        }



        void Brain_OnOutput(object sender, IntCodeOutputEventArgs e)
        {
            var code = (RepairDroidStatusCode)e.OutputValue;
            //if (code != RepairDroidStatusCode.HitWall)
            //    CurrenLocation = MovePosition(CurrenLocation, LastMoveDirection);
            var args = new ReportStatusEventArgs(/*CurrenLocation, LastMoveDirection,*/ code);
            ReportStatus(this, args);
        }

        //static Point MovePosition(Point current, MoveDirection direction)
        //{
        //    switch (direction)
        //    {
        //        case MoveDirection.East: return Point.Add(current, new Size(1, 0));
        //        case MoveDirection.West: return Point.Add(current, new Size(-1, 0));
        //        case MoveDirection.North: return Point.Add(current, new Size(0, 1));
        //        case MoveDirection.South: return Point.Add(current, new Size(0, -1));
        //        default: return current;
        //    }
        //}

    }


    class ReportStatusEventArgs : EventArgs
    {
        public ReportStatusEventArgs(/*Point location, MoveDirection direction,*/ RepairDroidStatusCode statusCode) : base()
        {
            //Location = location;
            //LastMoveDirection = direction;
            StatusCode = statusCode;
        }

        //public Point Location { get; private set; }
        //public MoveDirection LastMoveDirection { get; private set; }
        public RepairDroidStatusCode StatusCode { get; private set; }
    }


    class RequestMovementDirEventArgs : EventArgs
    {
        public RequestMovementDirEventArgs() : this(MoveDirection.North) { }
        public RequestMovementDirEventArgs(MoveDirection direction) : base()
        {
            Direction = direction;
        }

        public MoveDirection Direction { get; set; } = MoveDirection.North;
    }

}
