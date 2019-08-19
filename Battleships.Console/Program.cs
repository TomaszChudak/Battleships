using System.Runtime.CompilerServices;
using Battleships.Console.Input;
using Battleships.Console.Output;
using Battleships.Logic;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Battleships.Console.Tests")]

namespace Battleships.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceCollection = GetServiceCollection();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var dispatcher = serviceProvider.GetService<IDispatcher>();
            dispatcher.PlayGame();
        }

        private static IServiceCollection GetServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IDispatcher, Dispatcher>();
            serviceCollection.AddSingleton<IInputReader, InputReader>();
            serviceCollection.AddSingleton<IOutputFacade, OutputFacade>();
            serviceCollection.AddSingleton<IGridPainter, GridPainter>();
            serviceCollection.AddSingleton<IGridResultPainter, GridResultPainter>();
            serviceCollection.AddSingleton<ITextResultDisplayer, TextResultDisplayer>();
            serviceCollection.AddSingleton<IOutputWriter, OutputWriter>();
            serviceCollection.AddSingleton<ICursorHelper, CursorHelper>();
            serviceCollection.AddSingleton<ISoundPlayer, SoundPlayer>();

            serviceCollection.AddLogicModule();

            return serviceCollection;
        }
    }
}