using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Structures
{
    public class Tree : Ressource
    {
        public override int MaxProgress => 10;
        public override int gain => 2;
        public override RessourceType ressourceType => RessourceType.WOOD;

        public Tree() : base()
        {
            
        }

        public Tree(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }
    }
}
