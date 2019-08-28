namespace Battleships.Logic.Settings
{
    internal class SettingsRules
    {
        // setting file
        internal const string SettingFileName = "appsettings.json";

        // Grid / RowCount section
        internal const int MinimalGridRowCount = 5;
        internal const int MaximalGridRowCount = 20;

        // Grid / ColumnCount section
        internal const int MinimalGridColumnCount = 5;
        internal const int MaximalGridColumnCount = 20;

        // ShipTypes / Size section 
        internal const int MinimalShipTypeSize = 1;
        internal const int MaximalShipTypeSize = 6;

        // ShipTypes / Count section
        internal const int MinimalShipCount = 0;
        internal const int MaximalShipCount = 10;
    }
}