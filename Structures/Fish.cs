using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Structures
{
    public class Fish : Ressource
    {
        public override int MaxProgress => 100;
        public override int gain => 5;
        public override RessourceType ressourceType => RessourceType.FISH;

        public Fish(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }
    }
}
