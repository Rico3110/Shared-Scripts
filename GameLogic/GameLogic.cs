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

        private static List<Cart> carts;

        public static List<Tribe> Tribes = new List<Tribe>();

        public static List<Player> Players = new List<Player>();

        public static void Init(HexGrid.HexGrid hexGrid)
        {
            initialized = true;
            grid = hexGrid;

            buildings = new List<Building>();

            ressources = new List<Ressource>();

            carts = new List<Cart>();

            foreach (HexCell cell in grid.cells)
            {
                if (cell.Structure != null) {
                    AddStructureToList(cell.Structure);
                }
            }
            ComputeConnectedStorages();

            foreach(Building building in buildings)
            {
                if (building is Headquarter)
                    AddTribe(building.Tribe, (Headquarter) building);
            }

            foreach(Building building in buildings)
            {
                Tribe tribe = GetTribe(building.Tribe);
                tribe.AddBuilding(building.GetType());
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

        public static Tribe AddTribe(byte tribeID, Headquarter hq)
        {
            Tribe newTribe = new Tribe(tribeID, hq);
            Tribes.Add(newTribe);
            return newTribe;
        }

#endregion

        public static bool Harvest(byte tribeID, HexCoordinates coords)
        {
            HexCell cell = grid.GetCell(coords);
            Tribe tribe = GetTribe(tribeID);
            if (cell != null)
            {
                if (cell.Structure is Ressource)
                {
                    Ressource ressource = (Ressource)cell.Structure;
                    if (ressource.Harvestable())
                    {
                        ressource.Harvest();
                        tribe.HQ.Inventory.AddRessource(ressource.ressourceType, 1);
                        return true;
                    }
                }
            }
            return false;
        }

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

        public static bool VerifyBuild(HexCoordinates coords, Type buildingType, Player player)
        {
            Building building = (Building)Activator.CreateInstance(buildingType);
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

            if (!player.Tribe.BuildingPlacable(building.GetType()))
                return false;

            if (!player.Tribe.HQ.Inventory.RecipeApplicable(building.Recipes[0]))
                return false;

            return true;
        }

        public static HexCell ApplyBuild(HexCoordinates coords, Type buildingType, Tribe tribe)
        {
            Building building = (Building)Activator.CreateInstance(buildingType);
            HexCell cell = grid.GetCell(coords);
            if (cell.Structure != null)
            {
                DestroyStructure(coords);
            }
            cell.Structure = building;
            building.Cell = cell;
            building.Tribe = tribe.Id;

            tribe.HQ.Inventory.ApplyRecipe(building.Recipes[0]);

            
            AddStructureToList(building);

            tribe.AddBuilding(building.GetType());

            if (building is ICartHandler)
            {
                ComputeConnectedStorages();
            }

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
                if (building.Tribe != player.Tribe.Id)
                    return false;

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
                if (structure is ICartHandler)
                {
                    foreach (Cart cart in ((ICartHandler) structure).Carts)
                    {
                        if (!carts.Contains(cart))
                        {
                            carts.Add(cart);
                        }
                    }
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
                    if(structure is ICartHandler)
                    {
                        ComputeConnectedStorages();
                    }
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

            Tribe tribe = AddTribe((byte)Tribes.Count, hq);
            hq.Tribe = tribe.Id;
            
            AddStructureToList(hq);

            ComputeConnectedStorages();

            return tribe;
        }

        public static bool MoveTroops(Player player, HexCoordinates coordinates, TroopType troopType, int amount) 
        {
            ProtectedBuilding building = (ProtectedBuilding)grid.GetCell(coordinates).Structure;
            if(amount > 0)
            {
                return building.TroopInventory.MoveTroops(player.TroopInventory, troopType, Mathf.Abs(amount));
            }
            else 
            {
                return player.TroopInventory.MoveTroops(building.TroopInventory, troopType, Mathf.Abs(amount));
            }   
        }

        public static void Fight(Player attacker, HexCoordinates defenderCoordinates)
        {
            TroopInventory attackerInventory = attacker.TroopInventory;
            Building defender = (Building)grid.GetCell(defenderCoordinates).Structure;
            if(defender is ProtectedBuilding)
            {
                if (attackerInventory.Fight(((ProtectedBuilding)defender).TroopInventory))
                    attackerInventory.Fight(defender);
            }
            else 
            {
                attackerInventory.Fight(defender);
            }
        }

        public static bool ChangeAllowedRessource(HexCoordinates origin, HexCoordinates destination, RessourceType ressourceType, bool newValue)
        {
            HexCell originCell = grid.GetCell(origin);
            HexCell destinationCell = grid.GetCell(destination);

            if (originCell.Structure is InventoryBuilding && destinationCell.Structure is InventoryBuilding)
            {
                InventoryBuilding originBuilding = (InventoryBuilding)originCell.Structure;
                InventoryBuilding destinationBuilding = (InventoryBuilding)destinationCell.Structure;

                originBuilding.AllowedRessources[destinationBuilding][ressourceType] = newValue;
                return true;
            }
            return false;
        }

        public static void ChangeTroopRecipeOfBarracks(HexCoordinates barracksCoordinates, TroopType troopType)
        {
            HexCell cell = grid.GetCell(barracksCoordinates);
            if (cell != null)
            {
                if (cell.Structure is Barracks)
                {
                    ((Barracks)cell.Structure).ChangeTroopRecipe(troopType);
                }
            }
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
                foreach (Cart cart in carts)
                {
                    cart.DoTick();
                }
            }
            
        }

#region Compute Connected Storages
        
        public static void ComputeConnectedStorages()
        {
            foreach(Building building in buildings)
            {
                if (building is Road)
                {
                    ((Road)building).connectedStorages.Clear();
                }
                if (building is InventoryBuilding)
                {
                    ((InventoryBuilding)building).ConnectedInventories.Clear();
                }
            }
            foreach(Building building in buildings)
            {
                
                if(building is InventoryBuilding)
                {
                    for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
                    {
                        HexCell neighbor = building.Cell.GetNeighbor(dir);
                        if (neighbor != null && neighbor.Structure is Road  && ((Road)neighbor.Structure).HasBuilding(dir.Opposite()))
                        {
                            
                            ComputeConnectedStorages((Road)neighbor.Structure, (InventoryBuilding)building, dir.Opposite(), int.MaxValue, 0);
                        }
                    }
                }
            }
        }

        public static void ComputeConnectedStorages(Road current, InventoryBuilding origin, HexDirection direction, int minRoadLevel, int depth)
        {
            //Check if the tribe is already in the current dictionary and add it if it isn't
            if (!current.connectedStorages.ContainsKey(origin.Tribe))
                current.connectedStorages.Add(origin.Tribe, new Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>>());
            
            //Check if the origin Building already has an entry in the dictionary
            if (current.connectedStorages[origin.Tribe].ContainsKey(origin))
            {
                //An entry of the origin Building already exists. Check if the current Route has a better flowrate.
                if (FlowRate(current.connectedStorages[origin.Tribe][origin].Item2, current.connectedStorages[origin.Tribe][origin].Item3) >= FlowRate(minRoadLevel, depth))
                {
                    return;
                }
                else 
                {
                    current.connectedStorages[origin.Tribe][origin] = new Tuple<HexDirection, int, int>(direction, Mathf.Min(minRoadLevel, current.Level), depth);
                }
            }
            else 
            {
                current.connectedStorages[origin.Tribe].Add(origin, new Tuple<HexDirection, int, int>(direction, Mathf.Min(minRoadLevel, current.Level), depth));
            }

            //Look at the neighbors of the current road
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
            {
                HexCell neighbor = current.Cell.GetNeighbor(dir);
                //Propogate the route to origin through the roads
                if (neighbor != null && neighbor.Structure is Road && current.HasRoad(dir)) 
                {
                    ComputeConnectedStorages((Road)neighbor.Structure, origin, dir.Opposite(), Mathf.Min(minRoadLevel, current.Level), depth + 1);
                }

                //Add the entry to a connected Building
                if (neighbor != null && neighbor.Structure is InventoryBuilding && current.HasBuilding(dir) && neighbor.Structure != origin && ((InventoryBuilding) neighbor.Structure).Tribe == origin.Tribe)
                {
                    InventoryBuilding inventoryBuilding = (InventoryBuilding) neighbor.Structure;
                    if (inventoryBuilding.ConnectedInventories.ContainsKey(origin))
                    {
                        if (FlowRate(inventoryBuilding.ConnectedInventories[origin].Item2, inventoryBuilding.ConnectedInventories[origin].Item3) < FlowRate(minRoadLevel, depth + 1))
                        {
                            inventoryBuilding.ConnectedInventories[origin] = new Tuple<HexDirection, int, int>(dir.Opposite(), minRoadLevel, depth + 1);
                        }
                    }
                    else 
                    {
                        inventoryBuilding.ConnectedInventories.Add(origin, new Tuple<HexDirection, int, int>(dir.Opposite(), minRoadLevel, depth + 1));
                    }

                    //Init allowed Ressources for origin Building
                    if (!origin.AllowedRessources.ContainsKey(inventoryBuilding))
                    {
                        origin.AllowedRessources.Add(inventoryBuilding, new Dictionary<RessourceType, bool>());
                    }
                    foreach (RessourceType ressourceType in origin.Inventory.Outgoing)
                    {
                        if (inventoryBuilding.Inventory.Incoming.Contains(ressourceType))
                        {
                            if (!origin.AllowedRessources[inventoryBuilding].ContainsKey(ressourceType))
                            {
                                origin.AllowedRessources[inventoryBuilding].Add(ressourceType, true);
                            }
                        }
                    }

                }
                
            }
        }

        private static float FlowRate(int minRoadLevel, int depth)
        {
            return (float)(minRoadLevel * 5) / (float)(depth * 2);
        }
#endregion

    }
}
