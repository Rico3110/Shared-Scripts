using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Structures
{
    public abstract class ProtectedBuilding : Building
    {
        public int TroopCount;

        public ProtectedBuilding() : base()
        {
            this.TroopCount = 0;
        }

        public ProtectedBuilding(HexCell Cell, byte Tribe, byte Level, byte Health, int TroopCount) : base(Cell, Tribe, Level, Health)
        {
            this.TroopCount = TroopCount;
        }

        public override void DoTick()
        {
            base.DoTick();
        }

        public override bool IsPlaceable(HexCell cell)
        {
            if (!base.IsPlaceable(cell))
            {
                return false;
            }
            if(cell.Data.Biome == HexCellBiome.WATER)
            {
                return false;
            }
            return true;
        }
    }
}
