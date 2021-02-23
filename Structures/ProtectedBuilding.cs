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
        public TroopInventory TroopInventory;

        public ProtectedBuilding() : base()
        {
            this.TroopInventory = new TroopInventory();
        }

        public ProtectedBuilding(
            HexCell Cell, 
            byte Tribe, 
            byte Level, 
            byte Health, 
            TroopInventory TroopInventory
        ) : base(Cell, Tribe, Level, Health)
        {
            this.TroopInventory = TroopInventory;
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
