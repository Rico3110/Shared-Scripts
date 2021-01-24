using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Structures
{
    public class Scrub : Ressource
    {
        public override int MaxProgress => 5;
        public override int gain => 1;
        public override RessourceType ressourceType => RessourceType.WOOD;

        public Scrub() : base()
        {

        }

        public Scrub(HexCell Cell, byte Progress) : base(Cell, Progress)
        {
            
        }
    }
}
