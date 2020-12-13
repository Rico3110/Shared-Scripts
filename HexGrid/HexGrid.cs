﻿using System.Collections;
using System.Collections.Generic;
using Shared.GameState;
using Shared.DataTypes;

namespace Shared.HexGrid
{
    public class HexGrid
    {
        public int chunkCountX, chunkCountZ;
        public int cellCountX, cellCountZ;

        public HexCell[] cells;

        public HexGridChunk chunkPrefab;

        public HexGridChunk[] chunks;

        public HexMap map;

        public HexGrid(HexMap map)
        {
            this.map = map;

            cellCountX = map.chunkCountX;
            cellCountZ = map.chunkCountZ;

            CreateChunks();
            CreateCells();
        }

        void CreateChunks()
        {
            chunks = new HexGridChunk[chunkCountX * chunkCountZ];

            for (int z = 0, i = 0; z < chunkCountZ; z++)
            {
                for (int x = 0; x < chunkCountX; x++)
                {
                    HexGridChunk chunk = chunks[i++] = new HexGridChunk();
                }
            }
        }

        void CreateCells()
        {
            cells = new HexCell[cellCountX * cellCountZ];
            for (int z = 0, i = 0; z < cellCountZ; z++)
            {
                for (int x = 0; x < cellCountX; x++)
                {
                    CreateCell(x, z, i++);
                }
            }
        }

        void CreateCell(int x, int z, int i)
        {
            HexCell cell = cells[i] = new HexCell();
            cell.Data = new HexCellData(map.data[i]);
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

            if (x > 0)
            {
                cell.setNeighbor(HexDirection.W, cells[i - 1]);
            }

            if (z > 0)
            {
                if ((z & 1) == 0)
                {
                    cell.setNeighbor(HexDirection.SE, cells[i - cellCountX]);
                    if (x > 0)
                    {
                        cell.setNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                    }
                }
                else
                {
                    cell.setNeighbor(HexDirection.SW, cells[i - cellCountX]);
                    if (x < cellCountX - 1)
                    {
                        cell.setNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                    }
                }
            }

            AddCellToChunk(x, z, cell);
        }

        void AddCellToChunk(int x, int z, HexCell cell)
        {
            int chunkX = x / HexMetrics.chunkSizeX;
            int chunkZ = z / HexMetrics.chunkSizeZ;
            HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

            int localX = x - chunkX * HexMetrics.chunkSizeX;
            int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
            chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
        }

        public HexCell GetCell(HexCoordinates coordinates)
        {
            int z = coordinates.Z;
            int x = coordinates.X + z / 2;
            return cells[x + z * cellCountX];
        }
    }
}


