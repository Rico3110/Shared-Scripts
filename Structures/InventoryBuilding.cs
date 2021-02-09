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
    public abstract class InventoryBuilding : ProtectedBuilding, ICartHandler
    {
        public BuildingInventory Inventory;
        
        public Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>> ConnectedInventories;

        public Dictionary<InventoryBuilding, Dictionary<RessourceType, bool>> allowedRessources; 

        public List<Cart> Carts { get; set; }

        protected virtual byte MaxCartCount { get; } = 1; 

        public InventoryBuilding() : base()
        {
            this.Inventory = new BuildingInventory();
            this.ConnectedInventories = new Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>>();
            this.Carts = new List<Cart>();
            this.Carts.Add(new Cart(this));
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
            HandleCarts();
        }
      
        protected void SendRessources()
        {
            TrySendCart();
            foreach(InventoryBuilding inventoryBuilding in ConnectedInventories.Keys)
            {
                // this.Inventory.MoveInto(inventoryBuilding.Inventory, 1);
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

            KeyValuePair<InventoryBuilding, Tuple<HexDirection, int, int>> destination;
            try
            {
                destination = this.ConnectedInventories.First();
            }
            catch(Exception e)
            {
                return;
            }
            // if (destination == null)
            //     return;

            destination.Key.FillCart(cart, this);
            HexCell neighbor = this.Cell.GetNeighbor(destination.Value.Item1);
            if (neighbor != null && neighbor.Structure is Road)
            {
                this.Carts.Remove(cart);
                cart.isAvailable = false;
                ((Road)neighbor.Structure).Carts.Add(cart);
            }
        }

        private void ReceiveCart()
        {
            foreach (Cart cart in this.Carts)
            {
                if(this == cart.Origin)
                {
                    if (cart.isAvailable)
                        continue;
                    cart.isAvailable = true;
                }
                else
                {
                    //unload cart
                    cart.Inventory.MoveInto(this.Inventory, int.MaxValue);
                    cart.Destination = cart.Origin;
                    if (this.ConnectedInventories.ContainsKey(cart.Destination))
                    {
                        HexCell neighbor = this.Cell.GetNeighbor(this.ConnectedInventories[cart.Destination].Item1);
                        if (neighbor != null && neighbor.Structure is Road)
                        {
                            this.Carts.Remove(cart);
                            ((Road)neighbor.Structure).Carts.Add(cart);
                        }
                    }
                }
            }
        }

        public virtual bool FillCart(Cart cart, InventoryBuilding origin)
        {
            BuildingInventory destination = this.Inventory;
            bool ressourceAdded = true;
            while (ressourceAdded)
            {
                ressourceAdded = false;
                foreach(RessourceType ressourceType in destination.Incoming)
                {
                    if (cart.Inventory.AvailableSpace() > 0)
                    {
                        if (origin.Inventory.GetRessourceAmount(ressourceType) > 0)
                        {
                            cart.Inventory.AddRessource(ressourceType, 1);
                            origin.Inventory.RemoveRessource(ressourceType, 1);
                            ressourceAdded = true;
                        }
                    }
                }
            }
            return !cart.Inventory.IsEmpty();
        }

        public void HandleCarts()
        {
            ReceiveCart();
            TrySendCart();
            // throw new NotImplementedException();
        }
    }
}
