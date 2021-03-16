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
    public class Tree : Ressource
    {
        public override int MaxProgress => Constants.HoursToGameTicks(4);
        public override RessourceType ressourceType => RessourceType.WOOD;
        public override int harvestReduction => Constants.HoursToGameTicks(1);

        public Tree() : base()
        {
            
        }

        public Tree(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }
    }
}
