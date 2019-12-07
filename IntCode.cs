using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Advent_of_Code
{
    class IntCode
    {
        const int NumOpCodeDigits = 2;

        enum ParamaterMode
        {
            Position = 0,
            Immediate = 1
        }

        readonly Dictionary<int, MethodInfo> OpCodes = new Dictionary<int, MethodInfo>();
        readonly bool showMemoryOnStep = false;

        public IntCode(bool showMemoryOnStep = false)
        {
            this.showMemoryOnStep = showMemoryOnStep;

            // Load all OpCodes
            GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Select(m => new { methodInfo = m, OpCodeAttribute = m.GetCustomAttribute<OpCodeAttribute>() })
                .Where(m => m.OpCodeAttribute != null)
                .ForEach(m => OpCodes.Add(m.OpCodeAttribute.OpCode, m.methodInfo));
        }



        bool continueExecution = true;
        int[] memory;
        int position;
        int opCode = 0;
        int paramValue = 0;
        readonly Queue<int> inputValues = new Queue<int>();

        public List<int> OutputValues { get; } = new List<int>();



        public void Init(int[] code)
        {
            memory = code;
            position = 0;
            opCode = 99;
            paramValue = 0;
            inputValues.Clear();
            OutputValues.Clear();
        }

        public void Run(params int[] values)
        {
            LoadInputValues(values);
            OutputValues.Clear();
            continueExecution = true;

            while (continueExecution)
                RunStep();
        }
        public void RunStep()
        {
            ReadCommand();

            if (!OpCodes.ContainsKey(opCode))
            {
                Console.WriteLine("Something went Wrong!");
                continueExecution = true;
                return;
            }

            OpCodes[opCode].Invoke(this, null);

            if (showMemoryOnStep)
                ShowMemoryDump();
        }

        public int Peek(int address)
        {
            return memory[address];
        }
        public void Poke(int address, int value)
        {
            memory[address] = value;
        }
        public void ShowMemoryDump()
        {
            Console.WriteLine($"MEM: {string.Join(',', memory)}");
        }




        void LoadInputValues(int[] values)
        {
            inputValues.Clear();
            if (values == null)
                return;
            foreach (var v in values)
                inputValues.Enqueue(v);
        }

        void ReadCommand()
        {
            var value = memory[position].ToString();
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
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            var value = param1 + param2;
            SetValue(position + 3, ParamaterMode.Position, value);
            position += 4;
        }
        [OpCode(2)]
        void OpMultiply()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            var value = param1 * param2;
            SetValue(position + 3, ParamaterMode.Position, value);
            position += 4;
        }
        [OpCode(3)]
        void OpInput()
        {
            Console.Write("INPUT: ");
            int value;
            if (inputValues.Count > 0)
            {
                value = inputValues.Dequeue();
                Console.WriteLine(value);
            }
            else
            {
                var input = Console.ReadLine();
                value = Convert.ToInt32(input);
            }
            SetValue(position + 1, ParamaterMode.Position, value);
            position += 2;
        }
        [OpCode(4)]
        void OpOutput()
        {
            var value = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            OutputValues.Add(value);
            Console.WriteLine($"OUTPUT: {value}");
            position += 2;
        }
        [OpCode(5)]
        void OpJumpIfTrue()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            if (param1 != 0)
                position = param2;
            else
                position += 3;
        }
        [OpCode(6)]
        void OpJumpIfFalse()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            if (param1 == 0)
                position = param2;
            else
                position += 3;
        }
        [OpCode(7)]
        void OpLessThan()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            var param3 = GetValue(position + 3, GetParamaterMode(paramValue, 3));

            var value = (param1 < param2) ? 1 : 0;

            SetValue(position + 3, ParamaterMode.Position, value);
            position += 4;
        }
        [OpCode(8)]
        void OpEquals()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            var param3 = GetValue(position + 3, GetParamaterMode(paramValue, 3));

            var value = (param1 == param2) ? 1 : 0;

            SetValue(position + 3, ParamaterMode.Position, value);
            position += 4;
        }

        [OpCode(99)]
        void OpQuitExecution()
        {
            continueExecution = false;
        }



    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class OpCodeAttribute : Attribute
    {
        public OpCodeAttribute(int opCode)
        {
            OpCode = opCode;
        }

        public int OpCode { get; private set; }
    }
}
