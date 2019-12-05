using System;
using System.Collections.Generic;
using System.Text;

namespace Advent_of_Code
{
    class IntCode
    {
        const int OpCode_Finished = 99;
        const int NumOpCodeDigits = 2;

        enum ParamaterMode
        {
            Position = 0,
            Immediate = 1
        }

        readonly Dictionary<int, Action> OpCodes = new Dictionary<int, Action>();

        public IntCode()
        {
            OpCodes.Add(1, OpAdd);
            OpCodes.Add(2, OpMultiply);
            OpCodes.Add(3, OpInput);
            OpCodes.Add(4, OpOutput);
            OpCodes.Add(5, OpJumpIfTrue);
            OpCodes.Add(6, OpJumpIfFalse);
            OpCodes.Add(7, OpLessThan);
            OpCodes.Add(8, OpEquals);
        }

        public void Init(int[] code)
        {
            memory = code;
            position = 0;
            opCode = 99;
            paramValue = 0;
        }


        private int[] memory;
        private int position;
        int opCode = 0;
        int paramValue = 0;


        public bool RunStep()
        {
            ReadCommand();

            if (opCode == OpCode_Finished)
                return false;

            if (!OpCodes.ContainsKey(opCode))
            {
                Console.WriteLine("Something went Wrong!");
                return false;
            }

            OpCodes[opCode].Invoke();

            return true;
        }

        private void ReadCommand()
        {
            var value = memory[position].ToString();
            opCode = (value.Length >= NumOpCodeDigits)
                ? Convert.ToInt32(value.Substring(value.Length - NumOpCodeDigits))
                : Convert.ToInt32(value);
            paramValue = (value.Length > NumOpCodeDigits)
                ? Convert.ToInt32(value.Substring(0, value.Length - NumOpCodeDigits))
                : 0;
        }

        private ParamaterMode GetParamaterMode(int value, int paramNumber)
        {
            var result = Helper.GetDigitRight(value, paramNumber);
            if (!Enum.IsDefined(typeof(ParamaterMode), result))
                throw new InvalidOperationException();
            return (ParamaterMode)result;
        }


        private int GetValue(int pos, ParamaterMode mode)
        {
            switch (mode)
            {
                case ParamaterMode.Position: return memory[memory[pos]];
                case ParamaterMode.Immediate: return memory[pos];
                default: throw new InvalidOperationException();
            }
        }
        private void SetValue(int pos, ParamaterMode mode, int value)
        {
            switch (mode)
            {
                case ParamaterMode.Position: memory[memory[pos]] = value; break;
                case ParamaterMode.Immediate: memory[pos] = value; break;
                default: throw new InvalidOperationException();
            }
        }

        private void OpAdd()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            var value = param1 + param2;
            SetValue(position + 3, ParamaterMode.Position, value);
            position += 4;
        }
        private void OpMultiply()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            var value = param1 * param2;
            SetValue(position + 3, ParamaterMode.Position, value);
            position += 4;
        }
        private void OpInput()
        {
            Console.Write("INPUT (int): ");
            var input = Console.ReadLine();
            var value = Convert.ToInt32(input);
            SetValue(position + 1, ParamaterMode.Position, value);
            position += 2;
        }
        private void OpOutput()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            Console.WriteLine($"OUTPUT: {param1}");
            position += 2;
        }
        private void OpJumpIfTrue()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            if (param1 != 0)
                position = param2;
            else
                position += 3;
        }
        private void OpJumpIfFalse()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            if (param1 == 0)
                position = param2;
            else
                position += 3;
        }
        private void OpLessThan()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            var param3 = GetValue(position + 3, GetParamaterMode(paramValue, 3));

            var value = (param1 < param2) ? 1 : 0;

            SetValue(position + 3, ParamaterMode.Position, value);
            position += 4;
        }
        private void OpEquals()
        {
            var param1 = GetValue(position + 1, GetParamaterMode(paramValue, 1));
            var param2 = GetValue(position + 2, GetParamaterMode(paramValue, 2));
            var param3 = GetValue(position + 3, GetParamaterMode(paramValue, 3));

            var value = (param1 == param2) ? 1 : 0;

            SetValue(position + 3, ParamaterMode.Position, value);
            position += 4;
        }


        public void ShowMemoryDump()
        {
            Console.WriteLine($"MEM: {string.Join(',', memory)}");
        }

    }
}
