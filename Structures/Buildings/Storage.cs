using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    class Storage : InventoryBuilding
    {
        public override byte MaxLevel => 1;
        
        public override byte[] MaxHealths => new byte[]{
            100,
            200,
            255
        };

        public override int[] RessourceLimits => new int[] {
            80,
            140,
            200
        };

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ }
                };
                return result;
            }
        }

        public Storage() : base()
        {
            this.Inventory.Storage = BuildingInventory.GetDictionaryForAllRessources();
            this.Inventory.Incoming = BuildingInventory.GetListOfAllRessources();
            this.Inventory.Outgoing = BuildingInventory.GetListOfAllRessources();
        }

        public Storage(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            BuildingInventory Inventory
            ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory)
        {

        }


        public override void DoTick()
        {
            base.DoTick();
        }
    }
}
