using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    class CoalOre : Ressource
    {
        public override int MaxProgress => 4;
        public override RessourceType ressourceType => RessourceType.COAL;
        public override byte harvestReduction => 4;

        public CoalOre() : base()
        {

        }

        public CoalOre(HexCell Cell, byte Progress) : base(Cell, Progress)
        {

        }
    }
}
