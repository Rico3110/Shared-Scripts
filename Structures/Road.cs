using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    class Road : Building
    {
        public Road() : base()
        {
            this.Tribe = 0;
            this.Level = 1;
            this.Health = 100;
            this.MaxLevel = 3;
            this.MaxHealth = 100;
        }

        public Road(HexCell Cell, byte Tribe, byte Level, byte Health) : base(Cell, Tribe, Level, Health)
        {
            this.Tribe = Tribe;
            this.Level = Level;
            this.Health = Health;
        }

        public override bool IsPlaceable(HexCell cell)
        {
            return base.IsPlaceable(cell);
        }

        public bool hasAnyRoad()
        {
            for(HexDirection d = HexDirection.NE; d < HexDirection.NW; d++)
            {
                if (hasRoad(d))
                    return true;
            }
            return false;
        }

        public bool hasRoad(HexDirection direction)
        {
            if (Cell == null)
                return false;
            HexCell neighbor = Cell.GetNeighbor(direction);
            if (neighbor == null)
                return false;
            if (Cell.Structure != null && Cell.Structure is Road)
                return true;
            return false;
        }
    }
}
