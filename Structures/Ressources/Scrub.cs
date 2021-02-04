using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Structures
{
    public class Scrub : Ressource
    {
        public override int MaxProgress => 10;
        public override byte harvestReduction => 10;
        public override RessourceType ressourceType => RessourceType.WOOD;

        public Scrub() : base()
        {

        }

        public Scrub(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }
    }
}
