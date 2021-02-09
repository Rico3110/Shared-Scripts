﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    public abstract class Road : Building, ICartHandler 
    {
        private const float ELEVATION_THRESHOLD = 12f;

        public Dictionary<int, Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>>> connectedStorages;

        public List<Cart> Carts { get; set; }

        public Road() : base()
        {
            connectedStorages = new Dictionary<int, Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>>>();
            Carts = new List<Cart>();
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
            connectedStorages = new Dictionary<int, Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>>>();
            Carts = new List<Cart>();
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

        public override void DoTick()
        {
            base.DoTick();
            HandleCarts();
        }

        private void MoveCarts()
        {
            foreach (Cart cart in this.Carts)
            {
                HexCell neighbor = this.Cell.GetNeighbor(this.connectedStorages[cart.Destination.Tribe][cart.Destination].Item1);
                ((Road)neighbor.Structure).Carts.Add(cart);
            }
            this.Carts.Clear();
        }

        public void HandleCarts()
        {
            MoveCarts();
        }
    }
}

