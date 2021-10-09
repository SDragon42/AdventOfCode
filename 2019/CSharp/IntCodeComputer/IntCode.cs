using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using AdventOfCode.CSharp.Common;

namespace AdventOfCode.CSharp.Year2019.IntCodeComputer
{
    class IntCode
    {
        const int NumOpCodeDigits = 2;



        enum ParameterMode
        {
            Positional = 0,
            Immediate = 1,
            Relative = 2,
        }



        readonly Dictionary<int, MethodInfo> OpCodesDict;
        readonly Dictionary<long, long> memory = new Dictionary<long, long>();

        int opCode = -1;
        readonly List<ParameterMode> paramModes = new List<ParameterMode>();
        readonly Queue<long> inputBuffer = new Queue<long>();
        long relativeBase = 0;


        public long AddressPointer { get; private set; } = 0;
        public IntCodeState State { get; private set; } = IntCodeState.Running;



        public IntCode(IEnumerable<long> input)
        {
            OpCodesDict = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Select(m => new { methodInfo = m, OpCodeAttribute = m.GetCustomAttribute<OpCodeAttribute>() })
                .Where(m => m.OpCodeAttribute != null)
                .ToDictionary(k => k.OpCodeAttribute.OpCode, v => v.methodInfo);

            //for (var i = 0; i < input.Length; i++)
            //    memory.Add(i, input[i]);
            var i = 0;
            input.ForEach(v => memory.Add(i++, v));

            AddressPointer = 0;
        }



        public event EventHandler<IntCodeOutputEventArgs> Output;
        public event EventHandler AfterRunStep;



        /// <summary>Runs the IntCode in memory.
        /// </summary>
        public void Run()
        {
            if (State == IntCodeState.Finished)
                throw new ApplicationException("Already finished");
            if (State == IntCodeState.NeedsInput && inputBuffer.Count == 0)
                throw new ApplicationException("Needs Input");

            State = IntCodeState.Running;

            while (State == IntCodeState.Running)
            {
                ReadInstruction();

                if (!OpCodesDict.ContainsKey(opCode))
                    throw new ApplicationException($"Unknown OpCode '{opCode}'.");

                var opCodeAction = OpCodesDict[opCode];
                opCodeAction.Invoke(this, null);

                AfterRunStep?.Invoke(this, null);
            }
        }

        /// <summary>Reads the value as the specified memory address.
        /// </summary>
        /// <param name="address">The memory address index.</param>
        /// <returns></returns>
        public long Peek(long address)
        {
            return ReadMemory(address);
        }

        /// <summary>Writes a value to the specified memory address.
        /// </summary>
        /// <param name="address">The memory address index.</param>
        /// <param name="value">The value to write.</param>
        public void Poke(long address, long value)
        {
            WriteMemory(address, value);
        }


        /// <summary>Dumps the IntCode memory as a comma-separated string of values.
        /// </summary>
        /// <returns></returns>
        public string Dump()
        {
            var result = string.Join(",", memory);
            return result;
        }

        public void AddInput(params long[] data)
        {
            foreach (var value in data)
                inputBuffer.Enqueue(value);
        }



        void ReadInstruction()
        {
            var value = ReadMemory(AddressPointer).ToString();

            var opCodeStr = (value.Length >= NumOpCodeDigits)
                ? value.Substring(value.Length - NumOpCodeDigits)
                : value;
            opCode = Convert.ToInt32(opCodeStr);

            var pValues = value.Substring(0, value.Length - opCodeStr.Length)
                .PadLeft(3, '0')
                .Reverse()
                .Select(c => (ParameterMode)Char.GetNumericValue(c));
            paramModes.Clear();
            paramModes.AddRange(pValues);
        }
        void MoveToNext(long increment)
        {
            AddressPointer += increment;
            opCode = -1;
            for (var i = 0; i < paramModes.Count; i++)
                paramModes[i] = ParameterMode.Positional;
        }

        long ReadMemory(long address, ParameterMode mode = ParameterMode.Immediate)
        {
            ValidateAddress(address);
            switch (mode)
            {
                case ParameterMode.Positional: return ReadMemory(memory[address]);
                case ParameterMode.Relative: return ReadMemory(memory[address] + relativeBase);
                case ParameterMode.Immediate: return memory[address];
                default: throw new InvalidOperationException();
            }
        }

        void WriteMemory(long address, long value, ParameterMode mode = ParameterMode.Immediate)
        {
            ValidateAddress(address);
            switch (mode)
            {
                case ParameterMode.Positional: WriteMemory(memory[address], value); break;
                case ParameterMode.Relative: WriteMemory(memory[address] + relativeBase, value); break;
                case ParameterMode.Immediate: memory[address] = value; break;
                default: throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        void ValidateAddress(long address)
        {
            if (memory.ContainsKey(address))
                return;
            memory.Add(address, default(long));
        }

        /// <summary>Gets the Parameter Mode for the specified parameter.
        /// </summary>
        /// <param name="paramNumber">The number of the parameter.</param>
        /// <returns></returns>
        ParameterMode GetParameterMode(int paramNumber)
        {
            var index = paramNumber - 1;
            if (index >= paramModes.Count)
                return ParameterMode.Positional;
            return paramModes[index];
        }



        [OpCode(1)]
        void OP_Add()
        {
            var param1 = ReadMemory(AddressPointer + 1, GetParameterMode(1));
            var param2 = ReadMemory(AddressPointer + 2, GetParameterMode(2));
            var result = param1 + param2;
            WriteMemory(AddressPointer + 3, result, GetParameterMode(3));
            MoveToNext(4);
        }

        [OpCode(2)]
        void OP_Multiply()
        {
            var param1 = ReadMemory(AddressPointer + 1, GetParameterMode(1));
            var param2 = ReadMemory(AddressPointer + 2, GetParameterMode(2));
            var value = param1 * param2;
            WriteMemory(AddressPointer + 3, value, GetParameterMode(3));
            MoveToNext(4);
        }

        [OpCode(3)]
        void OP_Input()
        {
            var check = inputBuffer.TryDequeue(out long value);
            if (!check)
            {
                State = IntCodeState.NeedsInput;
                return;
            }

            WriteMemory(AddressPointer + 1, value, GetParameterMode(1));
            MoveToNext(2);
        }
        [OpCode(4)]
        void OP_Output()
        {
            var param1 = ReadMemory(AddressPointer + 1, GetParameterMode(1));
            Output?.Invoke(this, new IntCodeOutputEventArgs(param1));
            MoveToNext(2);
        }

        [OpCode(5)]
        void OP_JumpIfTrue()
        {
            var param1 = ReadMemory(AddressPointer + 1, GetParameterMode(1));
            var param2 = ReadMemory(AddressPointer + 2, GetParameterMode(2));
            if (param1 != 0)
            {
                AddressPointer = param2;
                return;
            }
            MoveToNext(3);
        }

        [OpCode(6)]
        void OP_JumpIfFalse()
        {
            var param1 = ReadMemory(AddressPointer + 1, GetParameterMode(1));
            var param2 = ReadMemory(AddressPointer + 2, GetParameterMode(2));
            if (param1 == 0)
            {
                AddressPointer = param2;
                return;
            }
            MoveToNext(3);
        }

        [OpCode(7)]
        void OP_LessThan()
        {
            var param1 = ReadMemory(AddressPointer + 1, GetParameterMode(1));
            var param2 = ReadMemory(AddressPointer + 2, GetParameterMode(2));
            var value = param1 < param2 ? 1 : 0;
            WriteMemory(AddressPointer + 3, value, GetParameterMode(3));
            MoveToNext(4);
        }

        [OpCode(8)]
        void OP_Equals()
        {
            var param1 = ReadMemory(AddressPointer + 1, GetParameterMode(1));
            var param2 = ReadMemory(AddressPointer + 2, GetParameterMode(2));
            var value = param1 == param2 ? 1 : 0;
            WriteMemory(AddressPointer + 3, value, GetParameterMode(3));
            MoveToNext(4);
        }

        [OpCode(9)]
        void OP_AdjustRelativeBase()
        {
            var param1 = ReadMemory(AddressPointer + 1, GetParameterMode(1));
            relativeBase += param1;
            MoveToNext(2);
        }

        [OpCode(99)]
        void OP_Halt()
        {
            State = IntCodeState.Finished;
            MoveToNext(1);
        }

    }

}
