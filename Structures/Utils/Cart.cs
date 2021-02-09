using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Structures
{
    public class Cart
    {
        public bool isAvailable;
        public Inventory Inventory;
        public InventoryBuilding Origin;
        public InventoryBuilding Destination;

        public Cart()
        {
            this.Inventory = new Inventory();
            this.Inventory.RessourceLimit = 2;
            this.isAvailable = true;
        }

        public Cart(InventoryBuilding origin)
        {
            this.Inventory = new Inventory();
            this.Inventory.RessourceLimit = 2;
            this.isAvailable = true;
            this.Origin = origin;
        }

        public Cart(bool isAvailable, Inventory Inventory, InventoryBuilding Origin, InventoryBuilding Destination)
        {
            this.isAvailable = isAvailable;
            this.Inventory = Inventory;
            this.Origin = Origin;
            this.Destination = Destination;
        }

        public void Clear()
        {
            this.Inventory.Clear();
        }
    }
}
