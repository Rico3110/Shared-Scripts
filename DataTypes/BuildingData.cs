using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.DataTypes
{
    public enum BuildingType
    {
        NONE, HQ, FARM, WOODCUTTER
    }

    public class BuildingData
    {
        public BuildingType Type { get; set; }
        public byte Level { get; set; }
        public byte TeamID { get; set; }
    }
}
