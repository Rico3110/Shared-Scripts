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
    abstract class InventoryBuilding : ProtectedBuilding
    {
        public Dictionary<RessourceType, int> Inventory;

        public Dictionary<RessourceType, int> RessourceLimits;

        public int RessourceLimit;

        public bool allowReceive;

        public List<InventoryBuilding> ConnectedInventories;


        public InventoryBuilding(
            HexCell Cell, 
            byte Tribe, 
            byte Level, 
            byte Health, 
            int TroopCount, 
            Dictionary<RessourceType, int> Inventory, 
            Dictionary<RessourceType, int> RessourceLimits, 
            bool allowReceive
            ) : base(Cell, Tribe, Level, Health, TroopCount)
        {
            this.Inventory = Inventory;
            this.RessourceLimits = RessourceLimits;
            this.allowReceive = allowReceive;
        }

        public override void DoTick()
        {
            base.DoTick();
        }

        protected int AvailableSpace()
        {
            int totalCount = 0;
            foreach(int count in Inventory.Values)
            {
                totalCount += count;
            }
            return RessourceLimit - totalCount;
        }

        protected int AvailableSpace(RessourceType ressourceType)
        {
            int totalRessourceSpace = AvailableSpace();
            int localRessourceSpace = totalRessourceSpace;            
            if (RessourceLimits.TryGetValue(ressourceType, out localRessourceSpace))
            {
                int count;
                if (Inventory.TryGetValue(ressourceType, out count))              
                    localRessourceSpace = localRessourceSpace - count;
            }
            return Mathf.Min(totalRessourceSpace, localRessourceSpace);
        }

        protected int AddRessource(RessourceType ressourceType, int count)
        {
            int availableSpace = 0;
            if (Inventory.ContainsKey(ressourceType))
            {
                availableSpace = AvailableSpace(ressourceType);
                if (availableSpace > 0)
                {
                    Inventory[ressourceType] += Mathf.Min(availableSpace, count);
                }
            }
            
            return Mathf.Min(availableSpace, count);
        }

        public void ReceiveRessources(Dictionary<RessourceType, int> inventory, int count)
        {
            foreach(RessourceType ressourceType in inventory.Keys)
            {
                if(count < 0)
                {
                    return;
                }
                int ressourceCount = 0;
                inventory.TryGetValue(ressourceType, out ressourceCount);
                
                int added = AddRessource(ressourceType, Mathf.Min(count, inventory[ressourceType]));
                inventory[ressourceType] -= added;
                count -= added;
            }
            return;
        }

        protected void SendRessources()
        {
            foreach(InventoryBuilding inventoryBuilding in ConnectedInventories)
            {
                if (inventoryBuilding.allowReceive)
                    inventoryBuilding.ReceiveRessources(Inventory, 2);
            }
        }
    }
}
