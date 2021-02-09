using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Structures
{
    public class Cart
    {
        public Inventory Inventory;
        public InventoryBuilding Origin;
        public InventoryBuilding Destination;

        public Cart()
        {
            this.Inventory = new Inventory();
            this.Inventory.RessourceLimit = 2;
        }

        public Cart(InventoryBuilding origin)
        {
            this.Inventory = new Inventory();
            this.Inventory.Storage = Inventory.GetDictionaryForAllRessources();
            this.Inventory.RessourceLimit = 2;
            this.Origin = origin;
            this.Destination = origin;
        }

        public Cart(Inventory Inventory, InventoryBuilding Origin, InventoryBuilding Destination)
        {
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
