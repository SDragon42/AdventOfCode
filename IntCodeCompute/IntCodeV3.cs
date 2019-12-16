using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Advent_of_Code.IntCodeComputer
{
    interface IIntCodeV3
    {
        event EventHandler<OutputEventArgs> OnOutput;

        IReadOnlyList<long> Code { get; }
        long Output { get; }
        IntCodeState State { get; }
        Action<string> Log { get; set; }

        void Init(IEnumerable<long> code);
        void Run();
        long Peek(int address);
        void Poke(int address, long value);
        void AddInput(long value);
    }

    //enum IntCodeState
    //{
    //    Running,
    //    Finished,
    //    NeedsInput,
    //    Error,
    //}

    class OutputEventArgs : EventArgs
    {
        public OutputEventArgs(long value) : base()
        {
            OutputValue = value;
        }
        public long OutputValue { get; private set; }
    }

    class IntCodeV3 : IIntCodeV3
    {
        const int NumOpCodeDigits = 2;

        enum ParamaterMode
        {
            Position = 0,
            Immediate = 1,
            Relative = 2,
        }

        readonly Dictionary<long, MethodInfo> OpCodes = new Dictionary<long, MethodInfo>();

        public IntCodeV3()
        {
            // Load all OpCodes
            GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Select(m => new { methodInfo = m, OpCodeAttribute = m.GetCustomAttribute<OpCodeAttribute>() })
                .Where(m => m.OpCodeAttribute != null)
                .ForEach(m => OpCodes.Add(m.OpCodeAttribute.OpCode, m.methodInfo));
        }
        public IntCodeV3(IEnumerable<long> code) : this()
        {
            Init(code);
        }



        readonly List<long> memory = new List<long>();
        int currentMemIdx;
        int relativeBaseMemIndex;

        long opCode = 0;
        long paramValue = 0;

        readonly Queue<long> inputValues = new Queue<long>();



        public IReadOnlyList<long> Code => memory;
        public long Output { get; private set; }
        public IntCodeState State { get; private set; }
        public Action<string> Log { get; set; } = null;



        public event EventHandler<OutputEventArgs> OnOutput;



        public void Init(IEnumerable<long> code)
        {
            memory.Clear();
            memory.AddRange(code);

            currentMemIdx = 0;
            relativeBaseMemIndex = 0;

            opCode = 99;
            paramValue = 0;
            inputValues.Clear();
            Output = 0;
        }

        public void Run()
        {
            State = IntCodeState.Running;
            while (State == IntCodeState.Running)
                RunStep();
        }
        void RunStep()
        {
            ReadCommand();

            if (!OpCodes.ContainsKey(opCode))
            {
                Console.WriteLine("Something went Wrong!");
                State = IntCodeState.Error;
                return;
            }

            OpCodes[opCode].Invoke(this, null);
        }

        public long Peek(int address)
        {
            return GetValue(address, ParamaterMode.Immediate);
        }
        public void Poke(int address, long value)
        {
            SetValue(address, ParamaterMode.Immediate, value);
        }

        public void AddInput(long value)
        {
            inputValues.Enqueue(value);
        }




        void ReadCommand()
        {
            //TODO: Maybe make this not require the long->string->long conversion process??
            var value = GetValue(currentMemIdx, ParamaterMode.Immediate).ToString();
            Log?.Invoke($"[{currentMemIdx,-5}] CMD: {value}");
            opCode = (value.Length >= NumOpCodeDigits)
                ? Convert.ToInt32(value.Substring(value.Length - NumOpCodeDigits))
                : Convert.ToInt32(value);
            paramValue = (value.Length > NumOpCodeDigits)
                ? Convert.ToInt32(value.Substring(0, value.Length - NumOpCodeDigits))
                : 0;
        }

        ParamaterMode GetParamaterMode(long value, int paramNumber, bool forOutput = false)
        {
            var result = Helper.GetDigitRight(value, paramNumber);
            if (!Enum.IsDefined(typeof(ParamaterMode), result))
                throw new InvalidOperationException();
            var mode = (ParamaterMode)result;
            if (mode == ParamaterMode.Immediate && forOutput)
                mode = ParamaterMode.Position;
            return mode;
        }


        long GetValue(int address, ParamaterMode mode)
        {
            ValidateAddress(address);
            switch (mode)
            {
                case ParamaterMode.Position: return GetValue((int)memory[address], ParamaterMode.Immediate);
                case ParamaterMode.Relative: return GetValue((int)memory[address] + relativeBaseMemIndex, ParamaterMode.Immediate);
                case ParamaterMode.Immediate: return memory[address];
                default: throw new InvalidOperationException();
            }
        }
        void SetValue(int address, ParamaterMode mode, long value)
        {
            ValidateAddress(address);
            switch (mode)
            {
                case ParamaterMode.Position: SetValue((int)memory[address], ParamaterMode.Immediate, value); break;
                case ParamaterMode.Relative: SetValue((int)memory[address] + relativeBaseMemIndex, ParamaterMode.Immediate, value); break;
                case ParamaterMode.Immediate: memory[address] = value; break;
                default: throw new InvalidOperationException();
            }
        }
        void ValidateAddress(int address)
        {
            if (address < memory.Count())
                return;
            var toAdd = address - (memory.Count() - 1);
            memory.AddRange(Enumerable.Repeat(0L, toAdd));
        }

        //void LogParam(int address, int paramNum, ParamaterMode? paramOverride = null)
        //{
        //    var value = GetValue(address, ParamaterMode.Immediate);
        //    var realValue = (paramOverride.HasValue)
        //        ? GetValue(address, paramOverride.Value)
        //        : GetValue(address, GetParamaterMode(paramValue, paramNum));
        //    switch (GetParamaterMode(paramValue, 1))
        //    {
        //        case ParamaterMode.Position:
        //            Log.Invoke($"\t{paramNum}: [{address}]={value}  =>  [{value}]={realValue}");
        //            break;
        //        case ParamaterMode.Relative:
        //            Log.Invoke($"\t{paramNum}: [{address}]={value}  =>  [{relativeBaseMemIndex} + {value} | {relativeBaseMemIndex + value}]={realValue}");
        //            break;
        //        case ParamaterMode.Immediate:
        //            Log.Invoke($"\t{paramNum}: [{address}]={value}");
        //            break;
        //    }
        //}

        [OpCode(1)]
        void OpAdd()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            var value = param1 + param2;
            SetValue(currentMemIdx + 3, GetParamaterMode(paramValue, 3, forOutput: true), value);
            currentMemIdx += 4;
        }
        [OpCode(2)]
        void OpMultiply()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            var value = param1 * param2;
            SetValue(currentMemIdx + 3, GetParamaterMode(paramValue, 3, forOutput: true), value);
            currentMemIdx += 4;
        }
        [OpCode(3)]
        void OpInput()
        {
            if (inputValues.Count == 0)
            {
                State = IntCodeState.NeedsInput;
                return;
            }

            var value = inputValues.Dequeue();
            SetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1, forOutput: true), value);
            currentMemIdx += 2;
        }
        [OpCode(4)]
        void OpOutput()
        {
            Output = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            OnOutput?.Invoke(this, new OutputEventArgs(Output));
            currentMemIdx += 2;
        }
        [OpCode(5)]
        void OpJumpIfTrue()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            if (param1 != 0)
                currentMemIdx = (int)param2;
            else
                currentMemIdx += 3;
        }
        [OpCode(6)]
        void OpJumpIfFalse()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            if (param1 == 0)
                currentMemIdx = (int)param2;
            else
                currentMemIdx += 3;
        }
        [OpCode(7)]
        void OpLessThan()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            var param3 = GetValue(currentMemIdx + 3, GetParamaterMode(paramValue, 3));

            var value = (param1 < param2) ? 1 : 0;

            SetValue(currentMemIdx + 3, GetParamaterMode(paramValue, 3, forOutput: true), value);
            currentMemIdx += 4;
        }
        [OpCode(8)]
        void OpEquals()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            var param3 = GetValue(currentMemIdx + 3, GetParamaterMode(paramValue, 3));

            var value = (param1 == param2) ? 1 : 0;

            SetValue(currentMemIdx + 3, GetParamaterMode(paramValue, 3, forOutput: true), value);
            currentMemIdx += 4;
        }

        /// <summary>
        /// Opcode 9 adjusts the relative base by the value of its only parameter. 
        /// The relative base increases (or decreases, if the value is negative) by the value of the parameter.
        /// </summary>
        [OpCode(9)]
        void OpAdjustRelativeBase()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            relativeBaseMemIndex += (int)param1;
            currentMemIdx += 2;
        }

        [OpCode(99)]
        void OpQuitExecution()
        {
            State = IntCodeState.Finished;
        }

    }

}
