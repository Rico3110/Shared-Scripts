using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using UnityEngine;

namespace Shared.Structures
{
    class Fisher : ProductionBuilding
    {
        public override byte MaxLevel => 1;
        public override byte[] MaxHealths => new byte[]{
            50
        };

        public override int[] RessourceLimits => new int[] {
            4
        };
        public override RessourceType ProductionType => RessourceType.FOOD;
        public override byte Gain => 4;
        public override int MaxProgresses => new int[] {
            2,
            2,
            2
        };
        private const int elevationThreshold = 40;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 4}, { RessourceType.STONE, 1} },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 10}, { RessourceType.IRON, 2} },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 5}, { RessourceType.IRON, 4 } }
                };
                return result;
            }
        }

        public Fisher() : base()
        {
            this.Inventory.Storage.Add(RessourceType.FOOD, 0);
            this.Inventory.RessourceLimit = 20;
        }

        public Fisher(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            TroopInventory TroopInventory,
            BuildingInventory Inventory,
            int Progress
            ) : base(Cell, Tribe, Level, Health, TroopInventory, Inventory, Progress)
        {

        }
    }
}