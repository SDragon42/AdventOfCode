using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace Advent_of_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            var asm = Assembly.GetExecutingAssembly();
            var typeList = asm.GetTypes()
                .Where(t => typeof(IPuzzle).IsAssignableFrom(t))
                .Where(t => !t.IsInterface)
                .Where(t => !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToList();

            //typeList.ForEach(RunPuzzle);
            RunPuzzle(typeList.LastOrDefault());
            //Test();
        }

        static void RunPuzzle(Type typeItem)
        {
            var puzzle = (IPuzzle)Activator.CreateInstance(typeItem);
            if (puzzle == null)
                return;
            Line();
            //var sw = new Stopwatch();
            //sw.Start();
            puzzle.Run();
            //sw.Stop();
            //Console.WriteLine($"RunTime: {sw.ElapsedMilliseconds}");
        }

        static void Line() => Console.WriteLine(string.Empty.PadRight(60, '-'));

        #region Testing

        //static void Test()
        //{
        //    var value = 1234567;
        //    var sValue = value.ToString();
        //    var opCode = Convert.ToInt32(sValue.Substring(sValue.Length - 2));
        //    var op = Convert.ToInt32(sValue.Substring(sValue.Length - 1, 1));
        //    var mode = Convert.ToInt32(sValue.Substring(sValue.Length - 2, 1));
        //    var param = Convert.ToInt32(sValue.Substring(0, sValue.Length - 2));

        //    Line();
        //    Console.WriteLine("TEST");
        //    Line();
        //    Console.WriteLine($"Value: {value}");

        //    Console.WriteLine($"OpCode: {opCode}");
        //    Console.WriteLine($"op: {op}");
        //    Console.WriteLine($"mode: {mode}");
        //    Console.WriteLine($"param: {param}");
        //    Line();
        //}
        //static void Test()
        //{
        //    var value = 123456;

        //    Func<int, int> GetRightDigits = (r) =>
        //    {
        //        double shift = Math.Pow(10, r);

        //        return 0;
        //    };

        //    var op = GetDigit(value, 1);
        //    var mode = GetDigit(value, 2);
        //    var opCode = ((value / 100) - Math.Truncate(value / 100.0)) * 100; //mode * 10 + op;
        //    var param = Convert.ToInt32(Math.Truncate(value / 100.0));

        //    Line();
        //    Console.WriteLine("TEST");
        //    Line();
        //    Console.WriteLine($"Value: {value}");

        //    Console.WriteLine($"OpCode: {opCode}");
        //    Console.WriteLine($"op: {op}");
        //    Console.WriteLine($"mode: {mode}");
        //    Console.WriteLine($"param: {param}");
        //    Line();
        //}

        //private static int GetDigit(int value, int digitPosition)
        //{
        //    var numDigits = Convert.ToInt32(Math.Floor(Math.Log10(value) + 1));
        //    if (digitPosition < 1)
        //        digitPosition = 1;
        //    var offset = numDigits - digitPosition + 1;
        //    var result = Math.Truncate(value / Math.Pow(10, numDigits - offset))
        //              - (Math.Truncate(value / Math.Pow(10, numDigits - offset + 1)) * 10);
        //    return Convert.ToInt32(result);
        //}



        #endregion
    }
}
