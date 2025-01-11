//using Microsoft.Extensions.DependencyInjection;
namespace AdventOfCode.CSharp.Year2024
{
    internal static class TestServices
    {
        private const string INPUT_ROOT_PATH = @"../../../../../../AdventOfCode.Input/2024";


        //private static readonly IServiceProvider _provider;
        
        //static TestServices()
        //{
        //    var services = new ServiceCollection();

        //    services.AddSingleton<IInputReader>(new InputReader(INPUT_ROOT_PATH));

        //    _provider = services.BuildServiceProvider();
        //}




        //public static T Get<T>() where T : notnull
        //{
        //    return _provider.GetRequiredService<T>();
        //}



        public static IInputReaderService Input { get; } = new InputReaderService(INPUT_ROOT_PATH);
    }
}
