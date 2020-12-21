using System.Collections;
using System.Collections.Generic;
using Shared.GameState;
using Shared.DataTypes;
using UnityEngine;

namespace Shared.HexGrid
{
    public class HexGrid
    {
        public int chunkCountX, chunkCountZ;
        public int cellCountX, cellCountZ;

        public HexCell[] cells;

        public HexGridChunk chunkPrefab;

        public HexGridChunk[] chunks;
        

        private GameState.GameState gameState;


        public HexGrid(int chunkCountX, int chunkCountZ)
        {
            this.chunkCountX = chunkCountX;
            this.chunkCountZ = chunkCountZ;

            cellCountX = chunkCountX * HexMetrics.chunkSizeX;
            cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

            CreateChunks();
            CreateCells();
        }

        public HexGrid(GameState.GameState gameState)
        {
            this.gameState = gameState;

            chunkCountX = gameState.map.chunkCountX;
            chunkCountZ = gameState.map.chunkCountZ;

            cellCountX = chunkCountX * HexMetrics.chunkSizeX;
            cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

            CreateChunks();
            CreateCells();
        }

        public HexGrid(HexMap map)
        {
            this.gameState = new GameState.GameState(map);

            chunkCountX = map.chunkCountX;
            chunkCountZ = map.chunkCountZ;

            cellCountX = chunkCountX * HexMetrics.chunkSizeX;
            cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

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
            cell.Data = new HexCellData(gameState.map.data[i]);
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
            cell.Building = new BuildingData();

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

        
        public void ChangeData(HexCellData data, HexCoordinates coordinate)
        {
            GetCell(coordinate).Data = data;
            gameState.map.data[coordinate.ToOffsetX() + coordinate.ToOffsetZ() * cellCountX] = data.toUint();
        }

        public void AddBuilding(BuildingData data, HexCoordinates coordinate)
        {
            GetCell(coordinate).Building = data;           
        }

        public HexCell GetCell(int x, int z)
        {
            return GetCell(HexCoordinates.FromOffsetCoordinates(x, z));
        }

        public HexCell GetCell(HexCoordinates coordinates)
        {
            int z = coordinates.Z;
            int x = coordinates.X + z / 2;
            return cells[x + z * cellCountX];
        }

        public HexCell GetCell(Vector3 position)
        {
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);
            return GetCell(coordinates);
        }

        public uint[] SerializeData()
        {
            uint[] data = new uint[cellCountX * cellCountZ];
            for(int i = 0; i < cellCountX * cellCountZ; i++)
            {
                data[i] = cells[i].Data.toUint();
            }
            return data;
        }
    }
}


