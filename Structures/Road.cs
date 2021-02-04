using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    public abstract class Road : Building
    {
        private const float ELEVATION_THRESHOLD = 12f;

        public List<Cart> Carts; 

        public Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>> connectedStorages;

        public Road() : base()
        {

        }

        public Road(
            HexCell Cell, 
            byte Tribe, 
            byte Level, 
            byte Health
        ) : base(
            Cell, 
            Tribe, 
            Level, 
            Health
        )
        {
            this.Tribe = Tribe;
            this.Level = Level;
            this.Health = Health;
        }


        public bool HasAnyConnection()
        {
            for (HexDirection d = HexDirection.NE; d < HexDirection.NW; d++)
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
            if (neighbor.Structure != null && neighbor.Structure is Road && Math.Abs(Cell.GetElevationDifference(direction)) < ELEVATION_THRESHOLD)
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
            if (neighbor.Structure != null && neighbor.Structure is Building && Math.Abs(Cell.GetElevationDifference(direction)) < ELEVATION_THRESHOLD)
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

