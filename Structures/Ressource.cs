using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    public abstract class Ressource : Structure
    {
        public byte Progress;

        public abstract int MaxProgress { get; }
        public abstract int gain { get; }
        public abstract RessourceType ressourceType { get; }

        public Ressource() : base()
        {
            this.Progress = 0;
        }

        public Ressource(HexCell Cell, byte Progress) : base(Cell)
        {
            this.Progress = Progress;
        }

        public virtual bool Harvestable()
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
