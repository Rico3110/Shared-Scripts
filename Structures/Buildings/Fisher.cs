using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using UnityEngine;
using Shared.Communication;

namespace Shared.Structures
{
    class Fisher : ProductionBuilding
    {
        public override string description => "The Fisher is used to gain Food from a nearby Fishressource. The Fisher needs to be placed adjacent to Fishressource. More adjacent Fish will impove the efficiency of the Fisher.";
        public override byte MaxLevel => 1;
        public override byte[] MaxHealths => new byte[]{
            4,
            8,
            12
        };

        public override int[] RessourceLimits => new int[] {
            4
        };
        public override RessourceType ProductionType => RessourceType.FOOD;
        public override byte Gain => 1;
        public override int[] MaxProgresses => new int[] {
            Constants.MinutesToGameTicks(60),
            Constants.MinutesToGameTicks(25),
            Constants.MinutesToGameTicks(12)
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