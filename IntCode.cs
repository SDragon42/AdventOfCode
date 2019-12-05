using System;
using System.Collections.Generic;
using System.Text;

namespace Advent_of_Code
{
    class IntCode
    {
        const int OpCode_Add = 1;
        const int OpCode_Multiply = 2;
        const int OpCode_Input = 3;
        const int OpCode_Output = 4;
        const int OpCode_Finished = 99;

        const int commandDigits = 2;

        //enum ParamaterMode
        //{
        //    Position = 0,
        //    Immediate = 1
        //}

        readonly Dictionary<int, Action> CommandList = new Dictionary<int, Action>();

        public IntCode()
        {
            CommandList.Add(1, OpAdd);
            CommandList.Add(2, OpMultiply);
            CommandList.Add(3, OpInput);
            CommandList.Add(4, OpOutput);
        }

        public void Init(int[] code)
        {
            input = code;
            position = 0;
            //op = 0;
            //opMode = ParamaterMode.Position;
            opCode = 99;
            paramValue = 0;
        }


        private int[] input;
        private int position;
        int opCode = 0;
        int paramValue = 0;

        //int op => GetDigit(opCode, 1);
        //ParamaterMode opMode => (ParamaterMode)GetDigit(opCode, 2);


        public bool RunStep()
        {
            try
            {
                ReadCommand();

                if (opCode == OpCode_Finished)
                    return false;

                if (!CommandList.ContainsKey(opCode))
                {
                    Console.WriteLine("Something went Wrong!");
                    return false;
                }

                CommandList[opCode].Invoke();

                return true;
            }
            finally
            {
                position++;
            }
        }

        private void ReadCommand()
        {
            var value = input[position].ToString();
            opCode = Convert.ToInt32(value.Substring(value.Length - 2));
            paramValue = Convert.ToInt32(value.Substring(0, value.Length - 2));
        }

        /// <summary>
        /// Gets a single digit from the specified number.
        /// </summary>
        /// <param name="value">The number value.</param>
        /// <param name="digitPosition">The digit position, from right to left.</param>
        /// <returns></returns>
        private static int GetDigit(int value, int digitPosition)
        {
            var numDigits = Convert.ToInt32(Math.Floor(Math.Log10(value) + 1));
            if (digitPosition < 1)
                digitPosition = 1;
            var offset = numDigits - digitPosition + 1;
            var result = Math.Truncate(value / Math.Pow(10, numDigits - offset))
                      - (Math.Truncate(value / Math.Pow(10, numDigits - offset + 1)) * 10);
            return Convert.ToInt32(result);
        }


        private int GetValue(int pos, int mode)
        {
            switch (mode)
            {
                case 0: return input[input[pos]];
                case 1: return input[pos];
                default: throw new InvalidOperationException();
            }
        }
        private void SetValue(int pos, int mode, int value)
        {
            switch (mode)
            {
                case 0: input[input[pos]] = value; break;
                case 1: input[pos] = value; break;
                default: throw new InvalidOperationException();
            }
        }

        public void OpAdd()
        {
            var param1 = GetValue(++position, GetDigit(paramValue, 1));
            var param2 = GetValue(++position, GetDigit(paramValue, 2));
            var value = param1 + param2;
            SetValue(++position, GetDigit(paramValue, 3), value);
        }

        public void OpMultiply()
        {
            var param1 = GetValue(++position, GetDigit(paramValue, 1));
            var param2 = GetValue(++position, GetDigit(paramValue, 2));
            var value = param1 * param2;
            SetValue(++position, GetDigit(paramValue, 3), value);
        }

        public void OpInput()
        {
            var address = input[++position];
            input[address] = paramValue;
        }

        public void OpOutput()
        {
            var address = input[++position];
            paramValue = input[address];
        }

    }
}
