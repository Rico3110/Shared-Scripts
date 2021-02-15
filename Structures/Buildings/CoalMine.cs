using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    class CoalMine : ProductionBuilding
    {
        public override byte MaxLevel => 3;
        public override byte MaxHealth => 100;

        public override RessourceType ProductionType => RessourceType.COAL;
        
        public override byte Gain => 4;

        public override int MaxProgress => 10;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.STONE, 2 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.STONE, 5} },
                    new Dictionary<RessourceType, int>{ { RessourceType.IRON, 2} }
                };
                return result; 
            }
        }

        public CoalMine() : base()
        {
            this.Inventory.Storage.Add(RessourceType.COAL, 0);
            this.Inventory.RessourceLimit = 20;

            this.Inventory.Outgoing.Add(RessourceType.COAL);
        }

        public CoalMine (
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
