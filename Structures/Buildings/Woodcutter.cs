using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using UnityEngine;

namespace Shared.Structures
{
    class Woodcutter : InventoryBuilding
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

        public Woodcutter() : base()
        {
            this.Inventory.Storage.Add(RessourceType.WOOD, 0);
            this.Inventory.RessourceLimit = 20;
            this.Inventory.RessourceLimits.Add(RessourceType.WOOD, 13);
            this.Inventory.Outgoing.Add(RessourceType.WOOD);
        }

        public Woodcutter(
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
            if (this.Inventory.AvailableSpace(RessourceType.WOOD) > 0)
            {
                count = Harvest();
            }
            this.Inventory.AddRessource(RessourceType.WOOD, count);

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
                        if (ressource.ressourceType == RessourceType.WOOD && ressource.Harvestable())
                            return ressource.Harvest();
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
            bool hasForest = false;
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = cell.GetNeighbor(d);
                if (neighbor != null)
                {
                    if (neighbor.Structure is Ressource && ((Ressource) neighbor.Structure).ressourceType == RessourceType.WOOD)
                    {
                        hasForest = true;
                        break;
                    }
                }
            }            
            return hasForest;
        }
    }
}
