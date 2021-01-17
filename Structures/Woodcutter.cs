using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    class Woodcutter : InventoryBuilding
    {
        public Woodcutter() : base()
        {
            this.MaxHealth = 100;
            this.MaxLevel = 3;
            this.Inventory.Add(RessourceType.WOOD, 0);
        }

        public Woodcutter(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            Dictionary<RessourceType, int> Inventory,
            Dictionary<RessourceType, int> RessourceLimits,
            bool AllowReceive
            ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory, RessourceLimits, AllowReceive)
        {

        }

        public override void DoTick()
        {
            base.DoTick();
            int count = 0;
            if(AvailableSpace() > 0)
            {
                count = Harvest();
            }
            AddRessource(RessourceType.WOOD, count);

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
            if (!base.IsPlaceable())
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
