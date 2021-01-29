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
            this.Inventory.Storage.Add(RessourceType.WOOD, 0);
            this.Inventory.RessourceLimit = 20;
            this.Inventory.RessourceLimits.Add(RessourceType.WOOD, 20);
            this.Inventory.Incoming.Add(RessourceType.WOOD);
        }

        public Headquarter(
            HexCell Cell,
            int Tribe,
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

            SendRessources();
        }
    }
}
