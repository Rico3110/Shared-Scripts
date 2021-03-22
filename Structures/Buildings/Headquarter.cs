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
        public override string description => "The Headquarter can be placed to found a new Tribe. Different levels of the Headquarter grant access to other Buildings. The Headquarter also includes an Inventory for the Tribe which can be accessed from anywhere. Ressources in the Inventory can be used to build other Buildings or be refined into better Ressources or Troops.";

        public override byte MaxLevel => 3;

        public override byte[] MaxHealths => new byte[]{
            25,
            30,
            35
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
                    new Dictionary<RessourceType, int>{ },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 20 }, { RessourceType.STONE, 10 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 25 }, { RessourceType.STONE, 20 } }
                };
                return result;
            }
        }

        public Headquarter() : base()
        {
            this.Inventory.Storage = BuildingInventory.GetDictionaryForAllRessources();
            this.Inventory.Incoming = BuildingInventory.GetListOfAllRessources();
            this.Inventory.Outgoing = BuildingInventory.GetListOfAllRessources();
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
