using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    class Wheat : Ressource
    {
        public override int MaxProgress => 3;

        public override int gain => 2;

        public override RessourceType ressourceType => RessourceType.WHEAT;

        public Wheat() : base()
        {

        }

        public Wheat(HexCell Cell, byte Progress) : base(Cell, Progress)
        {

        }
    }
}
