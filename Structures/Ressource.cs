using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    public class Ressource : Structure    
    {
        public byte Progress;
        public byte MaxProgress;
        public int gain;
        public RessourceType ressourceType;


        public Ressource(HexCell Cell, byte Progress) : base(Cell)
        {
            this.Progress = Progress;
        }

        public bool Harvestable()
        {
            if(Progress == MaxProgress)
            {
                return true;
            }
            return false;
        }

        public int Harvest()
        {
            Progress = 0;
            return gain;
        }

        public override void DoTick()
        {
            if(Progress < MaxProgress)
            {
                Progress++;
            }
        }
    }
}
