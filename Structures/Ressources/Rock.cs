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
        public override int MaxProgress => Constants.HoursToGameTicks(2);
        public override RessourceType ressourceType => RessourceType.STONE;
        public override int harvestReduction => Constants.HoursToGameTicks(2);

        public Rock() : base()
        {

        }

        public Rock(HexCell cell) : base(cell)
        {

        }

        public Rock(HexCell Cell, int Progress) : base(Cell, Progress)
        {
            
        }
    }
}
