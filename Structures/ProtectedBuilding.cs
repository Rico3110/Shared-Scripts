using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

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

        public override bool IsPlaceable()
        {
            if (!base.IsPlaceable())
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
