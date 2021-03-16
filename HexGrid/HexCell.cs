using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.DataTypes;
using Shared.Structures;
using System;

namespace Shared.HexGrid
{
    public class HexCell
    {
        public HexCoordinates coordinates;

        public HexGridChunk chunk;

        private HexCell[] neighbors;

        public HexCellData Data { get; set; }

        public Structure Structure { get; set; }

        public int Elevation
        {
            get
            {
                return Data.Elevation - (int)Data.WaterDepth;
            }
        }
       
        public Vector3 Position
        {
            get
            {
                Vector3 position;
                position.x = (coordinates.X + coordinates.Z * 0.5f) * (HexMetrics.innerRadius * 2f);
                position.y = 0;
                position.z = coordinates.Z * (HexMetrics.outerRadius * 1.5f);

                return position;
            }
        }

        public HexCell()
        {
            neighbors = new HexCell[6];
        }


        public int GetElevationDifference(HexDirection direction)
        {
            HexCell neighbor = GetNeighbor(direction);
            int difference = 0;
            if(neighbor != null)
            {
                difference = (int)Elevation - (int)GetNeighbor(direction).Elevation;
            }
            
            return difference;
        }

        public int GetElevationDifference(HexCell neighbor)
        {
            int difference = 0;
            if (neighbor != null)
            {
                difference = Elevation - neighbor.Elevation;
            }

            return difference;
        }

        public HexCell GetNeighbor(HexDirection direction)
        {
            return neighbors[(int)direction];
        }

        public void setNeighbor(HexDirection direction, HexCell cell)
        {
            neighbors[(int)direction] = cell;
            cell.neighbors[(int)direction.Opposite()] = this;
        } 

        public List<T> GetNeighborStructures<T>(int depth) where T : Structure
        {
            return this.GetNeighbors(depth).FindAll(elem => elem.Structure is T).ConvertAll<T>(elem => (T)elem.Structure);
        }

        public List<HexCell> GetNeighbors(int depth)
        {
            return GetNeighbors(depth, new List<HexCell>());
        }

        protected List<HexCell> GetNeighbors(int depth, List<HexCell> cells)
        {
            if (!cells.Contains(this))
                cells.Add(this);
            if (depth <= 0)
                return cells;

            for(HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = this.GetNeighbor(d);
                if (neighbor == null)
                    continue;
                neighbor.GetNeighbors(depth - 1, cells);
            }
            return cells;
        }
    }
}

