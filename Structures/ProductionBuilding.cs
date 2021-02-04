using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    public abstract class ProductionBuilding : ProgressBuilding
    {
        public abstract RessourceType ProductionType { get; }
        public abstract byte Gain { get; }

        public ProductionBuilding() : base()
        {
            Progress = 0;
        }

        public ProductionBuilding(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            BuildingInventory Inventory,
            int Progress
        ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory, Progress)
        {

        }

        private void Harvest()
        {
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = Cell.GetNeighbor(d);
                if (neighbor != null)
                {
                    if (neighbor.Structure is Ressource)
                    {
                        Ressource ressource = (Ressource)neighbor.Structure;
                        if (ressource.ressourceType == ProductionType && ressource.Harvestable())
                        {
                            ressource.Harvest();
                            Progress = 1;
                            return; 
                        }
                    }
                }
            }
        }

        public override bool IsPlaceable(HexCell cell)
        {
            if (!base.IsPlaceable(cell))
            {
                return false;
            }
            bool hasRessource = false;
            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = cell.GetNeighbor(d);
                if (neighbor != null)
                {
                    if (neighbor.Structure is Ressource && ((Ressource)neighbor.Structure).ressourceType == ProductionType)
                    {
                        hasRessource = true;
                        break;
                    }
                }
            }
            return hasRessource;
        }

        public override void OnMaxProgress()
        {
            Inventory.AddRessource(ProductionType, Gain);
        }

        public override void DoTick()
        {
            base.DoTick();
            if (Progress == 0 && Inventory.AvailableSpace(ProductionType) > 0)
            {
                Harvest();
            }
        }
    }
}
