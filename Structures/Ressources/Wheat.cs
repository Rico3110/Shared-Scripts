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
    class Wheat : Ressource
    {
        public override int MaxProgress => Constants.HoursToGameTicks(2);

        public override int harvestReduction => Constants.HoursToGameTicks(1);

        public override RessourceType ressourceType => RessourceType.WHEAT;

        public Wheat() : base()
        {

        }

        public Wheat(HexCell Cell, byte Progress) : base(Cell, Progress)
        {

        }
    }
}
