using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    class Mine : InventoryBuilding
    {
        public override byte MaxLevel => 3;
        public override byte MaxHealth => 100;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 1} },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 1} }
                };
                return result; 
            }
        }

        public Mine() : base()
        {
            this.Inventory.Storage.Add(RessourceType.IRON_ORE, 0);
            this.Inventory.Storage.Add(RessourceType.COAL, 0);
            this.Inventory.RessourceLimit = 20;

            this.Inventory.Outgoing.Add(RessourceType.IRON_ORE);
            this.Inventory.Outgoing.Add(RessourceType.COAL);
        }

        public Mine (
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            Inventory Inventory
            ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory)
        {
            
        }

        public override void DoTick()
        {
            base.DoTick();
            int count = 0;
            
            Harvest();
            

            SendRessources();
        }

        private int Harvest()
        {
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = Cell.GetNeighbor(d);
                if(neighbor != null)
                {
                    if(neighbor.Structure is Ressource)
                    {
                        Ressource ressource = (Ressource)neighbor.Structure;
                        if (ressource.Harvestable())
                        {
                            if (ressource.ressourceType == RessourceType.IRON_ORE && this.Inventory.AvailableSpace(RessourceType.IRON_ORE) > 0)
                            {
                                int count = ressource.Harvest();
                                this.Inventory.AddRessource(RessourceType.IRON_ORE, count);
                                return count;
                            }
                            else if (ressource.ressourceType == RessourceType.COAL && this.Inventory.AvailableSpace(RessourceType.COAL) > 0) 
                            {
                                int count = ressource.Harvest();
                                this.Inventory.AddRessource(RessourceType.COAL, count);
                                return count;
                            }
                        }
                    }
                }
            }
            return 0;
        }

        public override bool IsPlaceable(HexCell cell)
        {            
            if (!base.IsPlaceable(cell))
            {
                return false;
            }
            bool hasCoalOrIronOre = false;
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = cell.GetNeighbor(d);
                if (neighbor != null)
                {
                    if (neighbor.Structure is Ressource && (((Ressource) neighbor.Structure).ressourceType == RessourceType.IRON_ORE || ((Ressource) neighbor.Structure).ressourceType == RessourceType.COAL))
                    {
                        hasCoalOrIronOre = true;
                        break;
                    }
                }
            }            
            return hasCoalOrIronOre;
        }
    }
}
