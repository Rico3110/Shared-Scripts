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
        public int RessourceLimit { get { return RessourceLimits[Level - 1]; } }
        public abstract int[] RessourceLimits { get; }

        public BuildingInventory Inventory;
        
        public Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>> ConnectedInventories;

        public Dictionary<InventoryBuilding, Dictionary<RessourceType, bool>> AllowedRessources; 

        public List<Cart> Carts { get; set; }

        public virtual byte MaxCartCount { get; } = 1; 

        public InventoryBuilding() : base()
        {
            this.Inventory = new BuildingInventory();
            this.Inventory.RessourceLimit = this.RessourceLimit;
            this.ConnectedInventories = new Dictionary<InventoryBuilding, Tuple<HexDirection, int, int>>();
            this.AllowedRessources = new Dictionary<InventoryBuilding, Dictionary<RessourceType, bool>>();
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
            TroopInventory TroopInventory, 
            BuildingInventory Inventory 
        ) : base(Cell, Tribe, Level, Health, TroopInventory)
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

        public override void Upgrade()
        {
            base.Upgrade();
            this.Inventory.RessourceLimit = RessourceLimits[Level - 1];
        }

        private bool TrySendCart(Cart cart)
        {
            if (cart.HasMoved)
                return false;
            KeyValuePair<InventoryBuilding, Tuple<HexDirection, int, int>> destination;
            try
            {
                destination = this.FindDestination();
            }
            catch(Exception e)
            {
                return false;
            }

            if(destination.Key.FillCart(cart, this))
            {
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
            int movedRessources = cart.Inventory.MoveInto(this.Inventory, int.MaxValue);
            if (movedRessources == 0)
            {
                cart.Inventory.Clear();
            }
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
                //No connection to origin anymore -> Delete Cart and add the cart at origin
                else
                {
                    this.Carts.Remove(cart);
                    cart.Origin.AddCart(cart);
                }
                return true;
            }
            return false;
        }

        private KeyValuePair<InventoryBuilding, Tuple<HexDirection, int, int>> FindDestination()
        {
            //First check if there is a building which has an empty ressource which can be send.
            foreach (KeyValuePair<InventoryBuilding, Tuple<HexDirection, int, int>> kvp in this.ConnectedInventories)
            {
                InventoryBuilding possibleDestination = kvp.Key;
                foreach (KeyValuePair<RessourceType, bool> kvp2 in AllowedRessources[kvp.Key])
                {
                    if (kvp2.Value)
                    {
                        if (possibleDestination.Inventory.Storage[kvp2.Key] == 0 && possibleDestination.Inventory.AvailableSpace(kvp2.Key) > 0)
                        {
                            return kvp;
                        }
                    }
                }
            }
            //Then check if a building can be found that has space for any fitting ressourceType
            foreach (KeyValuePair<InventoryBuilding, Tuple<HexDirection, int, int>> kvp in this.ConnectedInventories)
            {
                InventoryBuilding possibleDestination = kvp.Key;
                foreach (KeyValuePair<RessourceType, bool> kvp2 in AllowedRessources[kvp.Key])
                {
                    if (kvp2.Value)
                    {
                        if (possibleDestination.Inventory.AvailableSpace(kvp2.Key) > 0)
                        {
                            return kvp;
                        }
                    }
                }
            }
            throw new Exception("No fitting destination found");
        }

        public virtual bool HasEmptyRessource(InventoryBuilding origin)
        {
            foreach (RessourceType ressourceType in origin.Inventory.Outgoing)
            {
                if (this.Inventory.Incoming.Contains(ressourceType))
                {
                    if (this.Inventory.GetRessourceAmount(ressourceType) == 0)
                        return true;
                }
            }
            return false;
        }

        public virtual bool FillCart(Cart cart, InventoryBuilding origin)
        {
            cart.Inventory.RessourceLimit = this.ConnectedInventories[origin].Item2;
            BuildingInventory destination = this.Inventory;
            bool ressourceAdded = true;
            while (ressourceAdded)
            {
                ressourceAdded = false;
                foreach(RessourceType ressourceType in destination.Incoming)
                {
                    if (origin.Inventory.Outgoing.Contains(ressourceType) && origin.AllowedRessources[this][ressourceType])
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
