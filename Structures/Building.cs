using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using UnityEngine;

namespace Shared.Structures
{
    public abstract class Building : Structure
    {
        public byte Tribe;       
        public byte Level;
        public byte Health;

        public abstract byte MaxLevel { get; }
        public abstract byte MaxHealth { get; }
        public Building() : base()
        {
            this.Tribe = 0;
            this.Level = 1;
            this.Health = 100;
        }

        public Building(HexCell Cell, byte Tribe, byte Level, byte Health) : base(Cell)
        {
            this.Tribe = Tribe;
            this.Level = Level;
            this.Health = Health;
        }

        public override void DoTick()
        {
            this.increaseHealth(1);
        }

        private void increaseHealth(byte count)
        {
            this.Health = (byte)Mathf.Min(this.MaxHealth, this.Health + count);
        }

        public void Upgrade()
        {
            if(Level <= MaxLevel)
                Level++;
        }

        public override bool IsPlaceable(HexCell cell) 
        {
            if (cell.Structure != null && typeof(Building).IsAssignableFrom(cell.Structure.GetType()))
            {
                return false;
            }
            
            return true;
        }
    }
}
