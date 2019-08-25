using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Battleships.Logic.Helpers;
using Microsoft.Extensions.Options;

namespace Battleships.Logic.Settings
{
    internal interface ISettingsChecker
    {
        ValidationResult Check();
    }

    internal class SettingsChecker : ISettingsChecker
    {
        private readonly IOptions<AppSettings> _config;

        public SettingsChecker(IOptions<AppSettings> config)
        {
            _config = config;
        }

        public ValidationResult Check()
        {
            var listCheckSettingRules = new List<CheckSettingRule>
            {
                CheckIfAllSettingsAreEmpty,
                CheckGridSettings,
                CheckShipTypes
            };

            foreach (var rule in listCheckSettingRules)
            {
                var validationResult = rule();
                if (validationResult != ValidationResult.Success)
                    return new ValidationResult($"An issue with settings in {SettingsRules.SettingFileName} file has been found. {validationResult.ErrorMessage}");
            }

            return ValidationResult.Success;
        }

        private ValidationResult CheckIfAllSettingsAreEmpty()
        {
            if (_config.Value.Grid == null
                && _config.Value.ShipTypes == null)
                return new ValidationResult($"All elements of setting file are missing or no {SettingsRules.SettingFileName} file has been found.");

            return ValidationResult.Success;
        }

        private ValidationResult CheckGridSettings()
        {
            if (_config.Value.Grid == null)
                return new ValidationResult("Lack of Grid setting.");

            if (_config.Value.Grid.RowCount == null)
                return new ValidationResult("Lack of Grid -> RowCount setting.");

            if (_config.Value.Grid.ColumnCount == null)
                return new ValidationResult("Lack of Grid -> ColumnCount setting.");

            if (_config.Value.Grid.RowCount < SettingsRules.MinimalGridRowCount
                || _config.Value.Grid.RowCount > SettingsRules.MaximalGridRowCount)
                return new ValidationResult($"Grid -> RowCount should be between {SettingsRules.MinimalGridRowCount} and {SettingsRules.MaximalGridRowCount}.");

            if (_config.Value.Grid.ColumnCount < SettingsRules.MinimalGridColumnCount
                || _config.Value.Grid.ColumnCount > SettingsRules.MaximalGridColumnCount)
                return new ValidationResult($"Grid -> ColumnCount should be between {SettingsRules.MinimalGridColumnCount} and {SettingsRules.MaximalGridColumnCount}.");

            return ValidationResult.Success;
        }

        private ValidationResult CheckShipTypes()
        {
            if (_config.Value.ShipTypes == null)
                return new ValidationResult("Lack of ShipTypes setting.");

            if (!_config.Value.ShipTypes.Any())
                return new ValidationResult("Lack of any ship type in ShipTypes setting.");

            foreach (var shipType in _config.Value.ShipTypes)
            {
                if (shipType.Size == null)
                    return new ValidationResult($"Lack of ShipTypes -> Size of '{shipType.Name}' setting.");

                if (shipType.Size < SettingsRules.MinimalShipTypeSize
                    || shipType.Size > SettingsRules.MaximalShipTypeSize)
                    return new ValidationResult(
                        $"ShipTypes -> Size of '{shipType.Name}' should be between {SettingsRules.MinimalShipTypeSize} and {SettingsRules.MaximalShipTypeSize}.");

                if (shipType.Count == null)
                    return new ValidationResult($"Lack of ShipTypes -> Count of '{shipType.Name}' setting.");

                if (shipType.Count < SettingsRules.MinimalShipCount
                    || shipType.Count > SettingsRules.MaximalShipCount)
                    return new ValidationResult(
                        $"ShipTypes -> Count of '{shipType.Name}' should be between {SettingsRules.MinimalShipCount} and {SettingsRules.MaximalShipCount}.");
            }

            return ValidationResult.Success;
        }

        private delegate ValidationResult CheckSettingRule();
    }
}