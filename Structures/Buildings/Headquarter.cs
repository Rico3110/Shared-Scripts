using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    public class Headquarter : InventoryBuilding
    {
        public override byte MaxLevel => 3;

        public override byte[] MaxHealths => new byte[]{
            100,
            200,
            255
        };

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 5 }, { RessourceType.STONE, 2 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 10 }, { RessourceType.STONE, 2 }, { RessourceType.IRON, 3 } }
                };
                return result;
            }
        }

        public Headquarter() : base()
        {
            this.Inventory.Storage = BuildingInventory.GetDictionaryForAllRessources();
            this.Inventory.RessourceLimit = 100;
            // this.Inventory.Outgoing = Inventory.GetListOfAllRessources();
            this.Inventory.Incoming = BuildingInventory.GetListOfAllRessources();

            this.Inventory.UpdateRessourceLimits(new Dictionary<RessourceType, int> { { RessourceType.WOOD, 40 }, { RessourceType.STONE, 20 }, { RessourceType.IRON, 20 }, { RessourceType.IRON_ORE, 10 }, { RessourceType.COAL, 10 } });
        }

        public Headquarter(
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
