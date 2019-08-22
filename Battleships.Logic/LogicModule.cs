using Battleships.Logic.Grid;
using Battleships.Logic.Helpers;
using Battleships.Logic.Settings;
using Battleships.Logic.Ships;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Battleships.Logic.Tests")]

namespace Battleships.Logic
{
    public static class LogicModule
    {
        public static IServiceCollection AddLogicModule(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISettingsChecker, SettingsChecker>();
            serviceCollection.AddSingleton<IFileWrapper, FileWrapper>();
            serviceCollection.AddSingleton<IRandomWrapper, RandomWrapper>();
            serviceCollection.AddSingleton<IGameLogic, GameLogic>();
            serviceCollection.AddSingleton<IGrid, Grid.Grid>();
            serviceCollection.AddSingleton<IGridBuilder, GridBuilder>();
            serviceCollection.AddSingleton<IShipFactory, ShipFactory>();
            serviceCollection.AddSingleton<ICoordinateParser, CoordinateParser>();

            SetConfiguration(serviceCollection);

            return serviceCollection;
        }

        private static void SetConfiguration(IServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration);
        }
    }
}