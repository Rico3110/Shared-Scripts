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
        public override byte MaxLevel => 1;

        public override byte MaxHealth => 100;

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

        public Headquarter() : base()
        {
            this.Inventory.Storage = BuildingInventory.GetDictionaryForAllRessources();
            this.Inventory.RessourceLimit = 50;
            // this.Inventory.Outgoing = Inventory.GetListOfAllRessources();
            this.Inventory.Incoming = BuildingInventory.GetListOfAllRessources();
        }

        public Headquarter(
            HexCell Cell,
            int Tribe,
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

            SendRessources();
        }
    }
}
