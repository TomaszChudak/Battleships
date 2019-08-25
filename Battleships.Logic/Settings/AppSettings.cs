using System.Collections.Generic;

namespace Battleships.Logic.Settings
{
    internal class AppSettings
    {
        public GridSettings Grid { get; set; }
        public IEnumerable<ShipTypeSettings> ShipTypes { get; set; }
    }
}
