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
        public override int MaxProgress => 100;
        public override int gain => 5;
        public override RessourceType ressourceType => RessourceType.STONE;

        public Rock(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }
    }
}
