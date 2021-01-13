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
        public Woodcutter(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            Dictionary<RessourceType, int> Inventory,
            Dictionary<RessourceType, int> RessourceLimits,
            bool allowReceive
            ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory, RessourceLimits, allowReceive)
        {
            this.Inventory = Inventory;
            this.RessourceLimits = RessourceLimits;
            this.allowReceive = allowReceive;
        }

        public override void DoTick()
        {
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
                    if(neighbor.Structure is Tree)
                    {
                        Tree tree = (Tree)neighbor.Structure;
                        if (tree.Harvestable())
                            return tree.Harvest();
                    }
                }
            }
            return 0;
        }

        public override bool IsPlaceable(HexCell cell)
        {
            if(cell.Data.Biome == HexCellBiome.WATER)
            {
                return false;
            }
            bool hasForrest = false;
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = cell.GetNeighbor(d);
                if (neighbor != null && neighbor.Data.Biome == HexCellBiome.FOREST)
                {
                    hasForrest = true;
                    break;
                }
            }            
            return hasForrest;
        }
    }
}
