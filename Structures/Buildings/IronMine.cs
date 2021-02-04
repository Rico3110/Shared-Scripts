using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    class IronMine : ProductionBuilding
    {
        public override byte MaxLevel => 3;
        public override byte MaxHealth => 100;

        public override RessourceType ProductionType => RessourceType.IRON_ORE;
        
        public override byte Gain => 4;

        public override int MaxProgress => 10;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 1} },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 1} }
                };
                return result; 
            }
        }

        public IronMine() : base()
        {
            this.Inventory.Storage.Add(RessourceType.IRON_ORE, 0);
            this.Inventory.RessourceLimit = 20;

            this.Inventory.Outgoing.Add(RessourceType.IRON_ORE);
        }

        public IronMine (
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            Inventory Inventory,
            int Progress
            ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory, Progress)
        {
            
        }
    }
}
