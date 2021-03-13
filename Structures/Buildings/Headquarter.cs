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
            40,
            60,
            80
        };

        public override int[] RessourceLimits => new int[] {
            40,
            80,
            110
        };

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 20 }, { RessourceType.STONE, 10 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 25 }, { RessourceType.STONE, 20 }, { RessourceType.IRON, 10 } }
                };
                return result;
            }
        }

        public Headquarter() : base()
        {
            this.Inventory.Storage = BuildingInventory.GetDictionaryForAllRessources();
            this.Inventory.Incoming = BuildingInventory.GetListOfAllRessources();
            this.Inventory.Outgoing = BuildingInventory.GetListOfAllRessources();
            this.Inventory.UpdateRessourceLimits(new Dictionary<RessourceType, int> { { RessourceType.WOOD, 40 }, { RessourceType.STONE, 20 }, { RessourceType.IRON, 20 }, { RessourceType.COAL, 10 } });
        }

        public Headquarter(
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
