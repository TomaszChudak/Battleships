using Battleships.Logic.Grid;
using Battleships.Logic.Helpers;
using Battleships.Logic.Settings;
using Battleships.Logic.Ships;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Runtime.CompilerServices;
using Battleships.Logic.Coordinates;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Battleships.Logic.Tests")]

namespace Battleships.Logic
{
    public static class LogicModule
    {
        public static IServiceCollection AddLogicModule(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISettingsChecker, SettingsChecker>();
            serviceCollection.AddSingleton<IRandomWrapper, RandomWrapper>();
            serviceCollection.AddSingleton<IGameLogic, GameLogic>();
            serviceCollection.AddSingleton<IGrid, Grid.Grid>();
            serviceCollection.AddSingleton<IGridBuilder, GridBuilder>();
            serviceCollection.AddSingleton<IShipFactory, ShipFactory>();
            serviceCollection.AddSingleton<ICoordinateParser, CoordinateParser>();
            serviceCollection.AddSingleton<ICoordinateValidator, CoordinateValidator>();

            SetConfiguration(serviceCollection);

            return serviceCollection;
        }

        private static void SetConfiguration(IServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(SettingsRules.SettingFileName, optional: true, reloadOnChange: true)
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration);
        }
    }
}