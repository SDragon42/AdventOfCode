using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Advent_of_Code.Day09
{
    interface IIntCodeV3
    {
        IReadOnlyList<long> Code { get; }
        long Output { get; }
        IntCodeState State { get; }

        void Init(long[] code);
        void Run();
        long Peek(long address);
        void Poke(long address, long value);
        void AddInput(long value);
    }

    enum IntCodeState
    {
        Running,
        Finished,
        NeedsInput,
        Error,
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
        public IntCodeV3(long[] code) : this()
        {
            Init(code);
        }



        long[] memory;
        long currentMemIdx;
        long relativeBaseMemIndex;

        long opCode = 0;
        long paramValue = 0;

        readonly Queue<long> inputValues = new Queue<long>();


        public IReadOnlyList<long> Code => memory;
        public long Output { get; private set; }
        public IntCodeState State { get; private set; }


        public void Init(long[] code)
        {
            memory = code;
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

        public long Peek(long address)
        {
            return memory[address];
        }
        public void Poke(long address, long value)
        {
            memory[address] = value;
        }

        public void AddInput(long value)
        {
            inputValues.Enqueue(value);
        }




        void ReadCommand()
        {
            //TODO: Made this not require the long->string->long conversion process.
            var value = memory[currentMemIdx].ToString();
            opCode = (value.Length >= NumOpCodeDigits)
                ? Convert.ToInt32(value.Substring(value.Length - NumOpCodeDigits))
                : Convert.ToInt32(value);
            paramValue = (value.Length > NumOpCodeDigits)
                ? Convert.ToInt32(value.Substring(0, value.Length - NumOpCodeDigits))
                : 0;
        }

        ParamaterMode GetParamaterMode(long value, int paramNumber)
        {
            var result = Helper.GetDigitRight(value, paramNumber);
            if (!Enum.IsDefined(typeof(ParamaterMode), result))
                throw new InvalidOperationException();
            return (ParamaterMode)result;
        }


        long GetValue(long pos, ParamaterMode mode)
        {
            switch (mode)
            {
                case ParamaterMode.Position: return memory[memory[pos]];
                case ParamaterMode.Immediate: return memory[pos];
                case ParamaterMode.Relative: return memory[memory[pos] + relativeBaseMemIndex];
                default: throw new InvalidOperationException();
            }
        }
        void SetValue(long pos, ParamaterMode mode, long value)
        {
            switch (mode)
            {
                case ParamaterMode.Position: memory[memory[pos]] = value; break;
                case ParamaterMode.Immediate: memory[pos] = value; break;
                case ParamaterMode.Relative: memory[memory[pos + relativeBaseMemIndex]] = value; break;
                default: throw new InvalidOperationException();
            }
        }

        [OpCode(1)]
        void OpAdd()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            var value = param1 + param2;
            SetValue(currentMemIdx + 3, ParamaterMode.Position, value);
            currentMemIdx += 4;
        }
        [OpCode(2)]
        void OpMultiply()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            var value = param1 * param2;
            SetValue(currentMemIdx + 3, ParamaterMode.Position, value);
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
            SetValue(currentMemIdx + 1, ParamaterMode.Position, value);
            currentMemIdx += 2;
        }
        [OpCode(4)]
        void OpOutput()
        {
            Output = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            currentMemIdx += 2;
        }
        [OpCode(5)]
        void OpJumpIfTrue()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            if (param1 != 0)
                currentMemIdx = param2;
            else
                currentMemIdx += 3;
        }
        [OpCode(6)]
        void OpJumpIfFalse()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            if (param1 == 0)
                currentMemIdx = param2;
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

            SetValue(currentMemIdx + 3, ParamaterMode.Position, value);
            currentMemIdx += 4;
        }
        [OpCode(8)]
        void OpEquals()
        {
            var param1 = GetValue(currentMemIdx + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(currentMemIdx + 2, GetParamaterMode(paramValue, 2));
            var param3 = GetValue(currentMemIdx + 3, GetParamaterMode(paramValue, 3));

            var value = (param1 == param2) ? 1 : 0;

            SetValue(currentMemIdx + 3, ParamaterMode.Position, value);
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
            relativeBaseMemIndex += param1;
            currentMemIdx += 2;
        }

        [OpCode(99)]
        void OpQuitExecution()
        {
            State = IntCodeState.Finished;
        }

    }

}
