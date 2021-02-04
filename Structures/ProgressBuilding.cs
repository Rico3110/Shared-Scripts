using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    public abstract class ProgressBuilding : InventoryBuilding
    {
        public abstract int MaxProgress { get; }
        public int Progress;
        

        public ProgressBuilding() : base()
        {
            Progress = 0;
        }

        public ProgressBuilding(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            BuildingInventory Inventory,
            int Progress
            ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory)
        {
            this.Progress = Progress;
        }

        public abstract void OnMaxProgress();

        public override void DoTick()
        {
            base.DoTick();
            if (Progress > 0) 
            {
                Progress++;
            }
            if (Progress == MaxProgress)
            {
                OnMaxProgress();
                Progress = 0;
            }
        }
    }
}
