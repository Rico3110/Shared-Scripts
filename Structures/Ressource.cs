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
        public int Progress;

        public abstract int MaxProgress { get; }
        public abstract RessourceType ressourceType { get; }
        public abstract int harvestReduction { get; }

        public Ressource() : base()
        {
            this.Progress = MaxProgress;
        }

        public Ressource(HexCell cell) : base(cell)
        {
            this.Progress = MaxProgress;
        }

        public Ressource(HexCell Cell, int Progress) : base(Cell)
        {
            this.Progress = Progress;
        }

        public virtual bool Harvestable()
        {
            if(Progress - harvestReduction >= 0)
            {
                return true;
            }
            return false;
        }

        public void Harvest()
        {
            Progress -= harvestReduction;
            return;
        }

        public override void DoTick()
        {
            if(Progress < MaxProgress)
            {
                Progress++;
            }
        }

        public virtual bool ManuallyHarvestable()
        {
            if (Progress == MaxProgress)
                return true;
            else
                return false;
        }

        public virtual int HarvestManually()
        {
            Progress = 0;
            return 1;
        }
    }
}
