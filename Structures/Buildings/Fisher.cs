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
        public override RessourceType ProductionType => RessourceType.WOOD;
        public override byte Gain => 4;
        public override int MaxProgress => 10;
        private const int elevationThreshold = 40;

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

        public Fisher() : base()
        {
            this.Inventory.Storage.Add(RessourceType.FISH, 0);
            this.Inventory.RessourceLimit = 20;
            this.Inventory.RessourceLimits.Add(RessourceType.FISH, 13);
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