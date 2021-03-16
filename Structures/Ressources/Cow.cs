﻿using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.Communication;

namespace Shared.Structures
{
    class Cow : Ressource
    {
        public override int MaxProgress => Constants.HoursToGameTicks(4);

        public override int harvestReduction => Constants.HoursToGameTicks(2);

        public override RessourceType ressourceType => RessourceType.COW;

        public Cow() : base()
        {

        }

        public Cow(HexCell cell) : base(cell)
        {

        }

        public Cow(HexCell Cell, int Progress) : base(Cell, Progress)
        {

        }
    }
}
