using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;


namespace Shared.Structures
{
    public class Tree : Ressource
    {
        public Tree(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            MaxProgress = 10;
            gain = 3;
        }
    }
}
