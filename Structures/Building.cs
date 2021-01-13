using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    abstract class Building : Structure
    {
        public byte Tribe;       
        public byte Level;
        public byte Health;

        private byte MaxLevel;

        public Building(HexCell Cell, byte Tribe, byte Level, byte Health) : base(Cell)
        {
            this.Tribe = Tribe;
            this.Level = Level;
            this.Health = Health;
        }

        public void Updgrade()
        {
            if(Level < MaxLevel)
                Level++;
        }
    }
}
