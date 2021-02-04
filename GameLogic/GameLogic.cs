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

        private static List<List<Road>> roadClusters;

        private static List<Ressource> ressources;

        public static List<Tribe> Tribes = new List<Tribe>();

        public static List<Player> Players = new List<Player>();

        public static void Init(HexGrid.HexGrid hexGrid)
        {
            initialized = true;
            grid = hexGrid;

            buildings = new List<Building>();

            roadClusters = new List<List<Road>>();

            ressources = new List<Ressource>();

            foreach (HexCell cell in grid.cells)
            {
                if (cell.Structure != null) {
                    AddStructureToList(cell.Structure);
                }
            }
        }
        
#region PLAYERS

        public static Player AddPlayer(string name, Tribe tribe)
        {
            Player newPlayer = new Player(name, tribe);
            Players.Add(newPlayer);
            return newPlayer;
        }

        public static Player GetPlayer(string name)
        {
            foreach(Player player in Players)
            {
                if (player.Name == name)
                    return player;
            }
            return null;
        }

        public static Tribe GetTribe(int id)
        {
            foreach(Tribe tribe in Tribes)
            {
                if (tribe.Id == id)
                    return tribe;
            }
            return null;
        }

        public static Tribe AddTribe(int tribeID, Headquarter hq)
        {
            Tribe newTribe = new Tribe(tribeID, hq);
            Tribes.Add(newTribe);
            return newTribe;
        }

#endregion

#region BUILDINGS

        public static bool PlayerInRange(HexCoordinates coords, Player player)
        {
            if (coords == player.Position)
            {
                return true;
            }
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
            {
                if (coords != player.Position.InDirection(dir))
                    continue;
                return true;
            }
            return false;            
        }

        public static bool VerifyBuild(HexCoordinates coords, Building building, Player player)
        {
            if (building == null)
                return false;
            if (player.Tribe == null)
                return false;
            //check if the player is adjacent to the position where the building is supposed to placed
            if (!PlayerInRange(coords, player))
                return false;
            
            HexCell cell = grid.GetCell(coords);

            //check if the building can be placed at the position
            if (!building.IsPlaceable(cell))
            {
                return false;
            }

            if (!player.Tribe.HQ.Inventory.RecipeApplicable(building.Recipes[0]))
                return false;

            return true;
        }

        public static HexCell ApplyBuild(HexCoordinates coords, Building building, Tribe tribe)
        {
            HexCell cell = grid.GetCell(coords);
            if (cell.Structure != null)
            {
                DestroyStructure(coords);
            }
            cell.Structure = building;
            building.Cell = cell;
            building.Tribe = tribe.Id;

            tribe.HQ.Inventory.ApplyRecipe(building.Recipes[0]);

            if (building is InventoryBuilding || building is Road)
            {
                ComputeConnectedStorages1();
            }
            
            AddStructureToList(building);
            return cell;
        }

        public static bool VerifyUpgrade(HexCoordinates coords, Player player)
        {
            HexCell cell = grid.GetCell(coords);

            if (cell == null)
            {
                return false;
            }

            if (player.Tribe == null)
            {
                return false;
            }

            if (!PlayerInRange(coords, player))
            {
                return false;
            }

            if(cell.Structure is Building)
            {
                Building building = (Building)cell.Structure;
                if (!building.IsUpgradable())
                    return false;

                if (player.Tribe.HQ.Inventory.RecipeApplicable(building.Recipes[building.Level]))
                    return true;
            }
            return false;
        }

        public static void ApplyUpgrade(HexCoordinates coords, Tribe tribe)
        {
            HexCell cell = grid.GetCell(coords);
            if (cell.Structure is Building)
            {
                Building building = (Building)cell.Structure;
                if (!building.IsUpgradable())
                    return;

                tribe.HQ.Inventory.ApplyRecipe(building.Recipes[building.Level]);
                ((Building)cell.Structure).Upgrade();
            }
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

        public static bool VerifyBuildHQ(HexCoordinates coords, Headquarter hq, Player player)
        {
            HexCell cell = grid.GetCell(coords);

            if (hq == null)
            {
                return false;
            }

            if (!PlayerInRange(coords, player))
            {
                return false;
            }

            //check if the building can be placed at the position
            if (!hq.IsPlaceable(cell))
            { 
                return false;
            }

            if(player.Tribe != null)
            {
                return false;
            }

            return true;
        }

        public static Tribe ApplyBuildHQ(HexCoordinates coords, Headquarter hq)
        {
            HexCell cell = grid.GetCell(coords);
            if (cell.Structure != null)
            {
                DestroyStructure(coords);
            }
            cell.Structure = hq;
            hq.Cell = cell;

            Tribe tribe = AddTribe(Tribes.Count, hq);
            hq.Tribe = tribe.Id;
            
            AddStructureToList(hq);
            return tribe;
        }

#endregion

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

#region Compute Connected Storages
        
        public static void ComputeConnectedStorages1()
        {
            foreach(Building building in buildings)
            {
                if(building is InventoryBuilding)
                {
                    for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
                    {
                        HexCell neighbor = building.Cell.GetNeighbor(dir);
                        if (neighbor != null && neighbor.Structure is Road  && ((Road)neighbor.Structure).HasBuilding(dir.Opposite()))
                            ComputeConnectedStorages((Road)neighbor.Structure, (InventoryBuilding)building, dir.Opposite(), int.MaxValue, 0);
                    }
                }
            }
        }

        public static void ComputeConnectedStorages(Road current, InventoryBuilding origin, HexDirection direction, int minRoadLevel, int depth)
        {
            if (current.connectedStorages.ContainsKey(origin))
            {
                if (FlowRate(current.connectedStorages[origin].Item2, current.connectedStorages[origin].Item3) >= FlowRate(minRoadLevel, depth))
                {
                    return;
                }
                else 
                {
                    current.connectedStorages.Remove(origin);
                    current.connectedStorages.Add(origin, new Tuple<HexDirection, int, int>(direction, Mathf.Min(minRoadLevel, current.Level), depth));
                }
            }
            else 
            {
                current.connectedStorages.Add(origin, new Tuple<HexDirection, int, int>(direction, Mathf.Min(minRoadLevel, current.Level), depth));
            }
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
            {
                HexCell neighbor = current.Cell.GetNeighbor(dir);
                if (neighbor != null && neighbor.Structure is Road && ((Road)neighbor.Structure).HasRoad(dir.Opposite()))
                    ComputeConnectedStorages((Road)neighbor.Structure, origin, dir.Opposite(), Mathf.Min(minRoadLevel, current.Level), depth + 1);
            }
        }

        private static float FlowRate(int minRoadLevel, int depth)
        {
            return (float)(minRoadLevel * 5) / (float)(depth * 2);
        }



        // public static void ComputeConnectedStorages()
        // {
        //     foreach (HexCell cell in grid.cells)
        //     {
        //         if (cell.Structure is InventoryBuilding)
        //         {
        //             ComputeConnectedStorages((InventoryBuilding)cell.Structure);
        //         }
        //     }
        // }

        // private static void ComputeConnectedStorages(InventoryBuilding building)
        // {
        //     float[] visited = new float[grid.cellCountX * grid.cellCountZ];
        //     visited[building.Cell.coordinates.X + building.Cell.coordinates.Z * grid.cellCountX] = float.MaxValue;
            
        //     List<Tuple<InventoryBuilding, int>> foundBuildings = new List<Tuple<InventoryBuilding, int>>();
        //     for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
        //     {
        //         HexCell neighbor = building.Cell.GetNeighbor(dir);
        //         if (neighbor != null && neighbor.Structure is Road && ((Road)neighbor.Structure).HasBuilding(dir.Opposite()))
        //         {
        //             foundBuildings = visitRoad(neighbor, visited, foundBuildings, 1, ((Road) neighbor.Structure).Level);
        //         }
        //     }

        //     Dictionary<InventoryBuilding, int> connectedStorages = new Dictionary<InventoryBuilding, int>();
        //     foreach (Tuple<InventoryBuilding, int> tpl in foundBuildings)
        //     {
        //         connectedStorages.Add(tpl.Item1, tpl.Item2);
        //     }
        //     building.ConnectedInventories = connectedStorages;
        // }

        // private static List<Tuple<InventoryBuilding, int>> visitRoad(HexCell cell, float[] visited, List<Tuple<InventoryBuilding, int>> foundBuildings, int depth, int minimumRoadLevel)
        // {
        //     float currentRoadValue = ItemAmountForConnection(depth, minimumRoadLevel);
        //     visited[cell.coordinates.X + cell.coordinates.Z * grid.cellCountX] = currentRoadValue;
        //     for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
        //     {
        //         HexCell neighbor = cell.GetNeighbor(dir);
        //         if (neighbor == null)
        //         {
        //             continue;
        //         }
        //         if (visited[neighbor.coordinates.X + neighbor.coordinates.Z * grid.cellCountX] >= currentRoadValue)
        //         {
        //             continue;
        //         }
        //         if (neighbor.Structure is Road && ((Road)cell.Structure).HasRoad(dir))
        //         {
        //             foundBuildings = visitRoad(neighbor, visited, foundBuildings, depth + 1, Mathf.Min(((Road)neighbor.Structure).Level, minimumRoadLevel));
        //         }
        //         if (neighbor.Structure is InventoryBuilding && ((Road)cell.Structure).HasBuilding(dir))
        //         {
        //             InventoryBuilding building = (InventoryBuilding)neighbor.Structure;
        //             int foundIndex = foundBuildings.FindIndex(elem => elem.Item1 == building);
        //             if (foundIndex == -1)
        //             {
        //                 foundBuildings.Add(new Tuple<InventoryBuilding, int>(building, (int)currentRoadValue));
        //             }
        //             else
        //             {
        //                 if (currentRoadValue > foundBuildings[foundIndex].Item2)
        //                 {
        //                     foundBuildings[foundIndex] = new Tuple<InventoryBuilding, int>(foundBuildings[foundIndex].Item1, (int)currentRoadValue);
        //                 }
        //             }
        //         }
        //     }
        //     return foundBuildings;
        // }

        // private static float ItemAmountForConnection(int roadLength, int minimumRoadLevel)
        // {
        //     float roadBaseAmount = (minimumRoadLevel + 1.0f) * (minimumRoadLevel + 1.0f);
        //     float lossDueToLength = (roadLength / (3.0f * minimumRoadLevel));
        //     float minAmount = 1;
        //     return Mathf.Max(minAmount, roadBaseAmount - lossDueToLength); 
        // }
#endregion

    }
}
