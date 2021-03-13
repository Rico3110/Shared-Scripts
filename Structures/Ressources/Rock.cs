using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;
using Shared.Communication;

namespace Shared.Structures
{
    public class Rock : Ressource
    {
        public override int MaxProgress => Constants.HoursToGameTicks(16);
        public override RessourceType ressourceType => RessourceType.STONE;
        public override int harvestReduction => Constants.HoursToGameTicks(8);

        public Rock() : base()
        {

        }

        public Rock(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }
    }
}
