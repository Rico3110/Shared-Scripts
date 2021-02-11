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

        public virtual byte MaxCartCount { get; } = 1; 

        public InventoryBuilding() : base()
        {
            this.Inventory = new BuildingInventory();
            this.ConnectedInventories = new Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>>();
            this.Carts = new List<Cart>();
            for (int i = 0; i < this.MaxCartCount; i++)
            {
                this.Carts.Add(new Cart(this));
            }
        }

        public InventoryBuilding(
            HexCell Cell, 
            byte Tribe,
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
      
        
        private bool TrySendCart(Cart cart)
        {
            if (cart.HasMoved)
                return false;
            KeyValuePair<InventoryBuilding, Tuple<HexDirection, int, int>> destination;
            try
            {
                destination = this.ConnectedInventories.First();
            }
            catch(Exception e)
            {
                return false;
            }

            if(destination.Key.FillCart(cart, this))
            {
                Console.WriteLine("cart was filled");
                HexCell neighbor = this.Cell.GetNeighbor(destination.Value.Item1);
                if (neighbor != null && neighbor.Structure is Road)
                {
                    this.Carts.Remove(cart);
                    cart.Destination = destination.Key;
                    ((Road)neighbor.Structure).AddCart(cart);
                    return true;
                }
            }
            return false;
        }
      

        public bool UnloadCart(Cart cart)
        {
            cart.Inventory.MoveInto(this.Inventory, int.MaxValue);
            if (cart.Inventory.IsEmpty())
            {
                cart.Destination = cart.Origin;
                if (this.ConnectedInventories.ContainsKey(cart.Destination))
                {
                    HexCell neighbor = this.Cell.GetNeighbor(this.ConnectedInventories[cart.Destination].Item1);
                    if (neighbor != null && neighbor.Structure is Road)
                    {
                        this.Carts.Remove(cart);
                        ((Road)neighbor.Structure).AddCart(cart);
                    }
                }
                return true;
            }
            return false;
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
                    if (origin.Inventory.Outgoing.Contains(ressourceType))
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
            }
            return !cart.Inventory.IsEmpty();
        }

        public void HandleCarts()
        {
            for(int i = 0; i < this.Carts.Count; i++)
            {
                if (this.Carts[i].Origin == this)
                {
                    //SendCart
                    if (TrySendCart(this.Carts[i]))
                        break;
                }
                else
                {
                    if (UnloadCart(this.Carts[i]))
                        break;
                }
            }
            // throw new NotImplementedException();
        }

        public void AddCart(Cart cart)
        {
            this.Carts.Add(cart);
            cart.HasMoved = true;
        }
    }
}
