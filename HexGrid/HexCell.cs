using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.DataTypes;

namespace Shared.HexGrid
{
    public class HexCell
    {
        public HexCoordinates coordinates;

        public HexGridChunk chunk;

        private HexCell[] neighbors;

        public HexCellData Data { get; set; }

        public BuildingData Building { get; set; }

        public uint Elevation
        {
            get
            {
                return Data.Elevation - (uint)Data.WaterDepth;
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
            int difference = (int)Data.Elevation - (int)GetNeighbor(direction).Data.Elevation;
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
    }
}

