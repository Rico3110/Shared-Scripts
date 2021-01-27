using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;
using Shared.Structures;
using UnityEngine;

namespace Shared.Game
{
    public static class GameLogic
    {
        public static bool initialized { get; private set; } = false;

        public static HexGrid.HexGrid grid { get; private set; }

        private static List<Building> buildings;

        private static List<Ressource> ressources;

        public static void Init(HexGrid.HexGrid hexGrid)
        {
            initialized = true;
            grid = hexGrid;

            buildings = new List<Building>();
            ressources = new List<Ressource>();

            foreach (HexCell cell in grid.cells)
            {
                if (cell.Structure != null) {
                    AddStructureToList(cell.Structure);
                }
            }
        }

        public static bool verifyBuild(HexCoordinates coords, Structure structure) 
        {
            HexCell cell = grid.GetCell(coords);
            if (structure != null && structure.IsPlaceable(cell))
            { 
                return true;
            }
            return false;
        }

        public static HexCell applyBuild(HexCoordinates coords, Structure structure)
        {
            HexCell cell = grid.GetCell(coords);
            if (cell.Structure != null)
            {
                DestroyStructure(coords);
            }
            cell.Structure = structure;
            structure.Cell = cell;

            if (structure is InventoryBuilding || structure is Road)
            {
                ComputeConnectedStorages();
            }
            
            AddStructureToList(structure);
            return cell;
        }

        public static void applyUpgrade(HexCoordinates coords)
        {
            HexCell cell = grid.GetCell(coords);
            if (cell.Structure is Building)
                ((Building)cell.Structure).Upgrade();
        }

        private static void AddStructureToList(Structure structure)
        {           
            if (typeof(Building).IsAssignableFrom(structure.GetType()))
            {
                if (!buildings.Contains(structure))
                {
                    buildings.Add((Building)structure);
                }
            }
            else if (typeof(Ressource).IsAssignableFrom(structure.GetType()))
            {
                if (!ressources.Contains(structure))
                {
                    ressources.Add((Ressource)structure);
                }
            }
        }

        public static void DestroyStructure(HexCoordinates coords)
        {
            HexCell cell = grid.GetCell(coords);
            Structure structure = cell.Structure;
            if (structure != null)
            {
                if (typeof(Building).IsAssignableFrom(structure.GetType()))
                {
                    buildings.RemoveAll(elem => elem == structure);
                }
                else if (typeof(Ressource).IsAssignableFrom(structure.GetType()))
                {
                    ressources.RemoveAll(elem => elem == structure);
                }
            }
        }

        public static void DoTick()
        {   
            if (initialized)
            {
                foreach (Ressource ressource in ressources)
                {
                    ressource.DoTick();
                }
                foreach (Building building in buildings)
                {
                    building.DoTick();
                    /*
                    for(HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
                    {
                        HexCell neighbor = building.Cell.GetNeighbor(d);
                        if (neighbor != null && neighbor.Structure != null && neighbor.Structure is Ressource)
                        {
                            if(((Ressource)neighbor.Structure).Progress == 0)
                                this.AddStructureToList(neighbor.Structure);
                        }
                    }
                    */
                }
            }
            
        }

        public static void ComputeConnectedStorages()
        {
            foreach (HexCell cell in grid.cells)
            {
                if (cell.Structure is InventoryBuilding)
                {
                    ComputeConnectedStorages((InventoryBuilding)cell.Structure);
                }
            }
        }

        private static void ComputeConnectedStorages(InventoryBuilding building)
        {
            bool[] visited = new bool[grid.cellCountX * grid.cellCountZ];
            visited[building.Cell.coordinates.X + building.Cell.coordinates.Z * grid.cellCountX] = true;
            
            List<Tuple<InventoryBuilding, int, int>> foundBuildings = new List<Tuple<InventoryBuilding, int, int>>();
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
            {
                HexCell neighbor = building.Cell.GetNeighbor(dir);
                if (neighbor.Structure is Road)
                {
                    foundBuildings = visitRoad(neighbor, ref visited, foundBuildings, 0, 100);
                }
            }

            Dictionary<InventoryBuilding, int> connectedStorages = new Dictionary<InventoryBuilding, int>();
            foreach (Tuple<InventoryBuilding, int, int> tpl in foundBuildings)
            {
                connectedStorages.Add(tpl.Item1, tpl.Item3);
            }
            building.ConnectedInventories = connectedStorages;
        }

        private static List<Tuple<InventoryBuilding, int, int>> visitRoad(HexCell cell, ref bool[] visited, List<Tuple<InventoryBuilding, int, int>> foundBuildings, int depth, int minimumRoadLevel)
        {
            visited[cell.coordinates.X + cell.coordinates.Z * grid.cellCountX] = true;
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
            {
                HexCell neighbor = cell.GetNeighbor(dir);
                if (visited[neighbor.coordinates.X + neighbor.coordinates.Z * grid.cellCountX])
                {
                    continue;
                }
                if (neighbor.Structure is Road)
                {
                    foundBuildings = visitRoad(neighbor, ref visited, foundBuildings, depth + 1, Mathf.Min(((Road)neighbor.Structure).Level, minimumRoadLevel));
                }
                if (neighbor.Structure is InventoryBuilding)
                {
                    InventoryBuilding building = (InventoryBuilding)neighbor.Structure;
                    int foundIndex = foundBuildings.FindIndex(elem => elem.Item1 == building);
                    if (foundIndex == -1)
                    {
                        foundBuildings.Add(new Tuple<InventoryBuilding, int, int>(building, depth, minimumRoadLevel));
                    }
                    else
                    {
                        if (minimumRoadLevel > foundBuildings[foundIndex].Item3)
                        {
                            foundBuildings[foundIndex] = new Tuple<InventoryBuilding, int, int>(foundBuildings[foundIndex].Item1, depth, minimumRoadLevel);
                        }
                        else if (minimumRoadLevel == foundBuildings[foundIndex].Item3 && depth < foundBuildings[foundIndex].Item2)
                        {
                            foundBuildings[foundIndex] = new Tuple<InventoryBuilding, int, int>(foundBuildings[foundIndex].Item1, depth, minimumRoadLevel);
                        }
                    }
                }
            }
            return foundBuildings;
        }
    }
}
