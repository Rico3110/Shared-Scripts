using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using UnityEngine;

namespace Shared.Structures
{
    abstract class Building : Structure
    {
        public byte Tribe;       
        public byte Level;
        public byte Health;

        private byte MaxLevel;
        private byte MaxHealth;
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


        public void Updgrade()
        {
            if(Level < MaxLevel)
                Level++;
        }
    }
}
