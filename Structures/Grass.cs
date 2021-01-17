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
        public override int MaxProgress => 0;
        public override int gain => 0;
        public override RessourceType ressourceType => RessourceType.WOOD;

        public Grass(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }

        public override void DoTick() 
        {
            return;
        }

        public override bool Harvestable()
        {
            return false;
        }
    }
}
