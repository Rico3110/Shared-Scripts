using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Structures
{
    class Quarry : InventoryBuilding
    {
        public Quarry(
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
            this.Inventory = Inventory;
            this.RessourceLimits = RessourceLimits;
            this.AllowReceive = AllowReceive;
        }

        public Quarry() : base()
        {

        }

        public override void DoTick()
        {
            int count = 0;
            if (AvailableSpace() > 0)
            {
                count = Harvest();
            }
            AddRessource(RessourceType.STONE, count);

            SendRessources();
        }

        private int Harvest()
        {
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = Cell.GetNeighbor(d);
                if (neighbor != null)
                {
                    if (neighbor.Structure is Ressource)
                    {
                        Ressource ressource = (Ressource)neighbor.Structure;
                        if (ressource.Harvestable())
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
            bool hasRock = false;
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = cell.GetNeighbor(d);
                if (neighbor != null && neighbor.Structure is Ressource)
                {
                    Ressource ressource = (Ressource)neighbor.Structure;
                    if(ressource.ressourceType == RessourceType.STONE)
                    {
                        hasRock = true;
                        break;
                    }                   
                }
            }
            return hasRock;
        }
    }
}
