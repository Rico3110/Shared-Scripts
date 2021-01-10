using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;

namespace Shared.DataTypes
{   
    public abstract class Building
    {
        public abstract byte MAX_LEVEL { get; }

        public abstract byte INV_SIZE { get; }

        public byte Level { get; protected set; }

        public byte Progress { get; protected set; }

        //(int, int) : (currentAmount, limit)
        public Dictionary<HexCellBiome, int> Inventory { get; protected set; }


        public void Upgrade()
        {
            Level++;
        }

        public abstract void DoTick();
    }
}
