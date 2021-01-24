using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    class CoalOre : Ressource
    {
        public override int MaxProgress => 4;

        public override int gain => 2;

        public override RessourceType ressourceType => RessourceType.COAL;

        public CoalOre() : base()
        {

        }

        public CoalOre(HexCell Cell, byte Progress) : base(Cell, Progress)
        {

        }
    }
}
