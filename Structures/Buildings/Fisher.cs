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
    class Fisher : InventoryBuilding
    {
        public override byte MaxLevel => 1;
        public override byte MaxHealth => 100;

        private const int elevationThreshold = 40;

        public Fisher() : base()
        {
            this.Inventory.Storage.Add(RessourceType.FISH, 0);
            this.Inventory.RessourceLimit = 20;
            this.Inventory.RessourceLimits.Add(RessourceType.FISH, 13);
        }

        public Fisher(
            HexCell Cell,
            int Tribe,
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
            if (this.Inventory.AvailableSpace() > 0)
            {
                count = Harvest();
            }
            this.Inventory.AddRessource(RessourceType.FISH, count);

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
                        if (ressource.ressourceType == RessourceType.FISH && ressource.Harvestable())
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
            ushort currentElevation = cell.Data.Elevation;
            bool hasSuitableWater = false;
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = cell.GetNeighbor(d);
                if (neighbor != null)
                {
                    if (neighbor.Structure is Ressource && ((Ressource) neighbor.Structure).ressourceType == RessourceType.FISH)
                    {
                        if (Mathf.Abs(neighbor.Data.Elevation - currentElevation) <= Fisher.elevationThreshold)
                        {
                            hasSuitableWater = true;
                            break;
                        }
                    }
                }
            }
            return hasSuitableWater;
        }
    }
}