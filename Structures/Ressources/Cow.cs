using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    class Cow : Ressource
    {
        public override int MaxProgress => 10;

        public override int harvestReduction => 5;

        public override RessourceType ressourceType => RessourceType.COW;

        public Cow() : base()
        {

        }

        public Cow(HexCell Cell, byte Progress) : base(Cell, Progress)
        {

        }
    }
}
