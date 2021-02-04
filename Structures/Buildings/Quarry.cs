using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Structures
{
    class Quarry : ProductionBuilding
    {
        public override byte MaxLevel => 1;
        public override byte MaxHealth => 100;
        public override RessourceType ProductionType => RessourceType.STONE;
        public override byte Gain => 4;
        public override int MaxProgress => 10;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ {RessourceType.WOOD, 5 } }
                };
                return result;
            }
        }

        public Quarry() : base()
        {
            this.Inventory.Storage.Add(RessourceType.STONE, 0);
            this.Inventory.RessourceLimit = 20;
            this.Inventory.RessourceLimits.Add(RessourceType.STONE, 13);
            this.Inventory.Outgoing.Add(RessourceType.STONE);
        }

        public Quarry(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            BuildingInventory Inventory,
            int Progress
            ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory, Progress)
        {

        }
    }
}
