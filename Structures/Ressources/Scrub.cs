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
    public class Scrub : Ressource
    {
        public override int MaxProgress => Constants.HoursToGameTicks(4);
        public override int harvestReduction => Constants.HoursToGameTicks(4);
        public override RessourceType ressourceType => RessourceType.WOOD;

        public Scrub() : base()
        {

        }

        public Scrub(HexCell Cell, int Progress) : base(Cell, Progress)
        {
            
        }
    }
}
