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
    public class Fish : Ressource
    {
        public override int MaxProgress => Constants.HoursToGameTicks(1);
        public override RessourceType ressourceType => RessourceType.FOOD;
        public override int harvestReduction => Constants.MinutesToGameTicks(30);

        public Fish() : base()
        {

        }

        public Fish(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }
    }
}
