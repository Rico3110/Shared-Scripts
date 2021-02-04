using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using UnityEngine;
using Shared.HexGrid;

namespace Shared.Structures
{
    public abstract class InventoryBuilding : ProtectedBuilding
    {
        public BuildingInventory Inventory;
        
        public Dictionary<InventoryBuilding, int> ConnectedInventories;

        public InventoryBuilding() : base()
        {
            this.Inventory = new BuildingInventory();
            this.ConnectedInventories = new Dictionary<InventoryBuilding, int>();
        }

        public InventoryBuilding(
            HexCell Cell, 
            int Tribe,
            byte Level, 
            byte Health, 
            int TroopCount, 
            BuildingInventory Inventory 
        ) : base(Cell, Tribe, Level, Health, TroopCount)
        {
            this.Inventory = Inventory;
            this.ConnectedInventories = new Dictionary<InventoryBuilding, int>();
        }

        public override void DoTick()
        {
            base.DoTick();
            SendRessources();
        }
      
        protected void SendRessources()
        {
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
            {
                HexCell neighbor = this.Cell.GetNeighbor(dir);
                if (neighbor != null && neighbor.Structure is Road && ((Road)neighbor.Structure).HasBuilding(dir.Opposite()))
                {
                    foreach (InventoryBuilding building in ((Road)neighbor.Structure).connectedStorages.Keys)
                    {
                        this.Inventory.MoveInto(building.Inventory, 1);
                    }
                }
            }
            foreach (KeyValuePair<InventoryBuilding, int> inventoryBuilding in ConnectedInventories)
            {
                this.Inventory.MoveInto(inventoryBuilding.Key.Inventory, inventoryBuilding.Value);
            }
        }
    }
}
