using System;
using System.Linq;
using Battleships.Logic.Helpers;
using Microsoft.Extensions.Options;

namespace Battleships.Logic.Settings
{
    internal interface ISettingsChecker
    {
        void Check();
    }

    internal class SettingsChecker : ISettingsChecker
    {
        private readonly IOptions<AppSettings> _config;
        private readonly IFileWrapper _fileWrapper;

        public SettingsChecker(IOptions<AppSettings> config, IFileWrapper fileWrapper)
        {
            _config = config;
            _fileWrapper = fileWrapper;
        }

        public void Check()
        {
            try
            {
                CheckIsFileExists();
                CheckAutoParsing();
                CheckGridSettings();
                CheckShipTypes();
                CheckShipCount();
            }
            catch (Exception e)
            {
                throw new ApplicationException($"An issue with settings in appsettings.json file has been found. {e.Message}");
            }
        }

        private void CheckIsFileExists()
        {
            if (!_fileWrapper.Exists("appsettings.json"))
                throw new ApplicationException("No appsettings.json file was found.");
        }

        private void CheckAutoParsing()
        {
            var value = _config.Value;
        }

        private void CheckGridSettings()
        {
            if (_config.Value.Grid == null)
                throw new ApplicationException("Lack of Grid setting.");

            if (_config.Value.Grid.RowCount == null)
                throw new ApplicationException("Lack of Grid -> RowCount setting.");

            if (_config.Value.Grid.ColumnCount == null)
                throw new ApplicationException("Lack of Grid -> ColumnCount setting.");

            if (_config.Value.Grid.RowCount < SettingsRules.MinimalGridRowCount
                || _config.Value.Grid.RowCount > SettingsRules.MaximalGridRowCount)
                throw new ApplicationException($"Grid -> RowCount should be between {SettingsRules.MinimalGridRowCount} and {SettingsRules.MaximalGridRowCount}.");

            if (_config.Value.Grid.ColumnCount < SettingsRules.MinimalGridColumnCount
                || _config.Value.Grid.ColumnCount > SettingsRules.MaximalGridColumnCount)
                throw new ApplicationException($"Grid -> ColumnCount should be between {SettingsRules.MinimalGridColumnCount} and {SettingsRules.MaximalGridColumnCount}.");
        }

        private void CheckShipTypes()
        {
            if (_config.Value.ShipTypes == null)
                throw new ApplicationException("Lack of ShipTypes setting.");

            if (!_config.Value.ShipTypes.Any())
                throw new ApplicationException("Lack of any ship type in ShipTypes setting.");

            foreach (var shipType in _config.Value.ShipTypes)
            {
                if(shipType.Size < SettingsRules.MinimalShipTypeSize
                   || shipType.Size > SettingsRules.MaximalShipTypeSize)
                    throw new ApplicationException($"ShipType -> Size of '{shipType.Name}' should be between {SettingsRules.MinimalShipTypeSize} and {SettingsRules.MaximalShipTypeSize}.");
            }
        }

        private void CheckShipCount()
        {
            if (_config.Value.ShipCount == null)
                throw new ApplicationException("Lack of ShipCount setting.");

            if (!_config.Value.ShipCount.Any())
                throw new ApplicationException("Lack of any ship type in ShipCount setting.");

            foreach (var shipCount in _config.Value.ShipCount)
            {
                if(shipCount.Count < SettingsRules.MinimalShipCount
                   || shipCount.Count > SettingsRules.MaximalShipCount)
                    throw new ApplicationException($"ShipCount -> Count of '{shipCount.Name}' should be between {SettingsRules.MinimalShipCount} and {SettingsRules.MaximalShipCount}.");
            }

            var shipTypeNames = _config.Value.ShipTypes.Select(x => x.Name).ToList();

            foreach (var shipCount in _config.Value.ShipCount)
            {
                if(!shipTypeNames.Contains(shipCount.Name))
                    throw new ApplicationException($"ShipCount setting contains unknown ship name {shipCount.Name}. It should contains only these ships which are defined in ShipTypes setting.");
            }
        }
    }
}