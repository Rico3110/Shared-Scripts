﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    public abstract class ProgressBuilding : InventoryBuilding
    {
        public int MaxProgress { get { return MaxProgresses[Level - 1]; } }
        public abstract int[] MaxProgresses { get; }

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
            TroopInventory TroopInventory,
            BuildingInventory Inventory,
            int Progress
            ) : base(Cell, Tribe, Level, Health, TroopInventory, Inventory)
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
