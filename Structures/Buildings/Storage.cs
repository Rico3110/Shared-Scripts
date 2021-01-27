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
        public override byte MaxHealth => 100;

        public Storage() : base()
        {
            this.Inventory.Storage.Add(RessourceType.WOOD, 0);
            this.Inventory.RessourceLimit = 20;
            this.Inventory.RessourceLimits.Add(RessourceType.WOOD, 20);
            this.Inventory.Incoming.Add(RessourceType.WOOD);
        }

        public Storage(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            Inventory Inventory
            ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory)
        {

        }


        public override void DoTick()
        {
            base.DoTick();
            int count = 0;
            if (this.Inventory.AvailableSpace(RessourceType.WOOD) > 0)
            {
                count = Harvest();
            }
            this.Inventory.AddRessource(RessourceType.WOOD, count);

            SendRessources();
        }
    }
}
