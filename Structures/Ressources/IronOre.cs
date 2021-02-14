using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    class IronOre : Ressource
    {
        public override int MaxProgress => 4;
        public override RessourceType ressourceType => RessourceType.IRON_ORE;
        public override byte harvestReduction => 2;

        public IronOre() : base()
        {

        }

        public IronOre(HexCell Cell, byte Progress) : base(Cell, Progress)
        {

        }
    }
}
