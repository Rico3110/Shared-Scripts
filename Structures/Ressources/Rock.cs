using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Structures
{
    public class Rock : Ressource
    {
        public override int MaxProgress => 10;
        public override RessourceType ressourceType => RessourceType.STONE;
        public override byte harvestReduction => 5;

        public Rock() : base()
        {

        }

        public Rock(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }
    }
}
