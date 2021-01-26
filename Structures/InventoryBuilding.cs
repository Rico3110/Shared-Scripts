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
        public Inventory Inventory;
        
        public Dictionary<InventoryBuilding, int> ConnectedInventories;

        public InventoryBuilding() : base()
        {
            this.Inventory = new Inventory();
            this.ConnectedInventories = new Dictionary<InventoryBuilding, int>();
        }

        public InventoryBuilding(
            HexCell Cell, 
            byte Tribe,
            byte Level, 
            byte Health, 
            int TroopCount, 
            Inventory Inventory 
        ) : base(Cell, Tribe, Level, Health, TroopCount)
        {
            this.Inventory = Inventory;
            this.ConnectedInventories = new Dictionary<InventoryBuilding, int>();
        }

        public override void DoTick()
        {
            base.DoTick();
        }
      

        protected void SendRessources()
        {
            foreach(KeyValuePair<InventoryBuilding, int> inventoryBuilding in ConnectedInventories)
            {
                this.Inventory.MoveInto(inventoryBuilding.Key.Inventory, inventoryBuilding.Value);
            }
        }

        public override bool IsPlaceable(HexCell cell)
        {
            return base.IsPlaceable(cell);
        }
    }
}
