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
        public override byte MaxLevel => 3;

        public override byte MaxHealth => 100;

        public Road() : base()
        {
            this.Tribe = 0;
            this.Level = 1;
            this.Health = 100;
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

        public bool HasAnyConnection()
        {
            for(HexDirection d = HexDirection.NE; d < HexDirection.NW; d++)
            {
                if (HasBuilding(d))
                    return true;
            }
            return false;
        }

        public bool HasRoad(HexDirection direction)
        {
            if (Cell == null)
                return false;
            HexCell neighbor = Cell.GetNeighbor(direction);
            if (neighbor == null)
                return false;
            if (neighbor.Structure != null && neighbor.Structure is Road && Math.Abs(Cell.GetElevationDifference(direction)) < 12)
                return true;
            return false;
        }

        public bool HasBuilding(HexDirection direction)
        {
            if (Cell == null)
                return false;
            HexCell neighbor = Cell.GetNeighbor(direction);
            if (neighbor == null)
                return false;
            if (neighbor.Structure != null && neighbor.Structure is Building && Math.Abs(Cell.GetElevationDifference(direction)) < 12)
                return true;
            return false;
        }

        public bool HasStraightLine(HexDirection direction)
        {
            if (HasBuilding(direction) && !HasBuilding(direction.Next()) && !HasBuilding(direction.Next().Next()) && HasBuilding(direction.Opposite()))
                return true;
            return false;
        }

        public bool IsSmoothCorner(HexDirection direction)
        {
            if (HasBuilding(direction.Previous()) && !HasBuilding(direction) && HasBuilding(direction.Next()))
                return true;
            return false;
        }

        public bool IsEmpty(HexDirection direction)
        {
            return !(IsSmoothCorner(direction) || HasStraightLine(direction.Previous()) || HasBuilding(direction));
        }
    }
}
