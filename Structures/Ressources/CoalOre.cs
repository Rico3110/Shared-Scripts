using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.Communication;

namespace Shared.Structures
{
    class CoalOre : Ressource
    {
        public override int MaxProgress => Constants.HoursToGameTicks(4);
        public override RessourceType ressourceType => RessourceType.COAL;
        public override int harvestReduction => Constants.HoursToGameTicks(3);

        public CoalOre() : base()
        {

        }

        public CoalOre(HexCell cell) : base(cell)
        {

        }

        public CoalOre(HexCell Cell, int Progress) : base(Cell, Progress)
        {

        }
    }
}
