using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    abstract class ProtectedBuilding : Building
    {
        public int TroopCount;

        public ProtectedBuilding(HexCell Cell, byte Tribe, byte Level, byte Health, int TroopCount) : base(Cell, Tribe, Level, Health)
        {
            this.TroopCount = TroopCount;
        }

        public override void DoTick()
        {
            base.DoTick();
        }
    }
}
