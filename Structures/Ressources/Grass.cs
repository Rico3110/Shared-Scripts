using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Structures
{
    public class Grass : Ressource
    {
        public override int MaxProgress => 4;
        public override RessourceType ressourceType => RessourceType.FISH;
        public override byte harvestReduction => 2;

        public Grass() : base()
        {

        }

        public Grass(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }

        public override void DoTick() 
        {
            base.DoTick();
        }

        public override bool Harvestable()
        {
            return false;
        }
    }
}
