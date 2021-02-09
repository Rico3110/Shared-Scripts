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
        
        public Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>> ConnectedInventories;

        public Dictionary<InventoryBuilding, Dictionary<RessourceType, bool>> allowedRessources; 

        List<Cart> Carts;

        protected virtual byte MaxCartCount { get; } = 1; 

        public InventoryBuilding() : base()
        {
            this.Inventory = new BuildingInventory();
            this.ConnectedInventories = new Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>>();
            this.Carts = new List<Cart>();
            this.Carts.Add(new Cart());
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
            this.ConnectedInventories = new Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>>();
            this.Carts = new List<Cart>();
        }

        public override void DoTick()
        {
            base.DoTick();
            SendRessources();
        }
      
        protected void SendRessources()
        {
            foreach(InventoryBuilding inventoryBuilding in ConnectedInventories.Keys)
            {
                this.Inventory.MoveInto(inventoryBuilding.Inventory, 1);
            }
             
            /*
            for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
            {
                HexCell neighbor = this.Cell.GetNeighbor(dir);
                if (neighbor != null && neighbor.Structure is Road && ((Road)neighbor.Structure).HasBuilding(dir.Opposite()))
                {
                    foreach (InventoryBuilding building in ((Road)neighbor.Structure).connectedStorages[Tribe].Keys)
                    {
                        if(building != this)
                        {
                            this.Inventory.MoveInto(building.Inventory, 1);
                        }
                    }
                }
            }
            foreach (KeyValuePair<InventoryBuilding, Tuple<HexDirection, int, int>> inventoryBuilding in ConnectedInventories)
            {
                // this.Inventory.MoveInto(inventoryBuilding.Key.Inventory, inventoryBuilding.Value);
            }
            */
        }

        private Cart GetAvailableCart()
        {
            return this.Carts.Find(cart => cart.isAvailable);
        }
        
        private void TrySendCart()
        {
            Cart cart = GetAvailableCart();

            if (cart == null)
                return;

            //Tries to send with full cart
            foreach(KeyValuePair<InventoryBuilding, Tuple<HexDirection, int, int>> kvp in ConnectedInventories)
            {
                
            }
        }
    }
}
