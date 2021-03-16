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
        public override string description => "The Storage is used to store Ressources. Higher Levels will improve the Capacity of the Storage. The Storage can also be used as a distributor for the Ressources.";
        public override byte MaxLevel => 1;
        
        public override byte[] MaxHealths => new byte[]{
            12,
            14,
            16
        };

        public override int[] RessourceLimits => new int[] {
            40,
            80,
            120
        };

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 8 }, { RessourceType.STONE, 3 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 10 }, { RessourceType.STONE, 6 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 12 }, { RessourceType.STONE, 5 }, { RessourceType.IRON, 3 } }
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
            TroopInventory TroopInventory,
            BuildingInventory Inventory
            ) : base(Cell, Tribe, Level, Health, TroopInventory, Inventory)
        {

        }


        public override void DoTick()
        {
            base.DoTick();
        }
    }
}
