using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Advent_of_Code.IntCodeComputer
{
    interface IIntCodeV2
    {
        IReadOnlyList<int> Code { get; }
        int Output { get; }
        IntCodeState State { get; }

        void Init(int[] code);
        void Run();
        int Peek(int address);
        void Poke(int address, int value);
        void AddInput(int value);
    }



    class IntCodeV2 : IIntCodeV2
    {
        const int NumOpCodeDigits = 2;

        enum ParamaterMode
        {
            Position = 0,
            Immediate = 1
        }

        readonly Dictionary<int, MethodInfo> OpCodes = new Dictionary<int, MethodInfo>();

        public IntCodeV2()
        {
            // Load all OpCodes
            GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Select(m => new { methodInfo = m, OpCodeAttribute = m.GetCustomAttribute<OpCodeAttribute>() })
                .Where(m => m.OpCodeAttribute != null)
                .ForEach(m => OpCodes.Add(m.OpCodeAttribute.OpCode, m.methodInfo));
        }
        public IntCodeV2(int[] code) : this()
        {
            Init(code);
        }



        int[] memory;
        int currentMemIdx;
        int opCode = 0;
        int paramValue = 0;

        readonly Queue<int> inputValues = new Queue<int>();


        public IReadOnlyList<int> Code => memory;
        public int Output { get; private set; }
        public IntCodeState State { get; private set; }


        public void Init(int[] code)
        {
            memory = code;
            currentMemIdx = 0;
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

        public int Peek(int address)
        {
            return memory[address];
        }
        public void Poke(int address, int value)
        {
            memory[address] = value;
        }

        public void AddInput(int value)
        {
            inputValues.Enqueue(value);
        }




        void ReadCommand()
        {
            //TODO: Made this not require the int->string->int conversion process.
            var value = memory[currentMemIdx].ToString();
            opCode = (value.Length >= NumOpCodeDigits)
                ? Convert.ToInt32(value.Substring(value.Length - NumOpCodeDigits))
                : Convert.ToInt32(value);
            paramValue = (value.Length > NumOpCodeDigits)
                ? Convert.ToInt32(value.Substring(0, value.Length - NumOpCodeDigits))
                : 0;
        }

        ParamaterMode GetParamaterMode(int value, int paramNumber)
        {
            var result = Helper.GetDigitRight(value, paramNumber);
            if (!Enum.IsDefined(typeof(ParamaterMode), result))
                throw new InvalidOperationException();
            return (ParamaterMode)result;
        }


        int GetValue(int pos, ParamaterMode mode)
        {
            switch (mode)
            {
                case ParamaterMode.Position: return memory[memory[pos]];
                case ParamaterMode.Immediate: return memory[pos];
                default: throw new InvalidOperationException();
            }
        }
        void SetValue(int pos, ParamaterMode mode, int value)
        {
            switch (mode)
            {
                case ParamaterMode.Position: memory[memory[pos]] = value; break;
                case ParamaterMode.Immediate: memory[pos] = value; break;
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

        [OpCode(99)]
        void OpQuitExecution()
        {
            State = IntCodeState.Finished;
        }

    }

}
