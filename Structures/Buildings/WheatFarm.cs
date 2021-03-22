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
    public class WheatFarm : ProductionBuilding
    {
        public override string description => "The Wheatfarm is used to produce Wheat from nearby crops. The Wheatfarm needs to be placed adjacent to atleast one Crop. More adjacent Crops will increase the efficiency of the farm.";
        
        public override byte MaxLevel => 3;

        public override byte[] MaxHealths => new byte[]{
            4,
            8,
            12
        };

        public override int[] RessourceLimits => new int[] {
            4,
            10,
            20
        };

        public override int[] MaxProgresses => new int[] {
            Constants.MinutesToGameTicks(90),
            Constants.MinutesToGameTicks(60),
            Constants.MinutesToGameTicks(25)
        };
        
        public override RessourceType ProductionType => RessourceType.WHEAT;

        public override byte Gain => 4;



        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 5 }, { RessourceType.STONE, 2 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 8 }, { RessourceType.STONE, 4 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 10 }, { RessourceType.STONE, 5 }, { RessourceType.LEATHER, 3 } }
                };
                return result;
            }
        }

        public WheatFarm() : base()
        {
            this.Inventory.Storage.Add(RessourceType.WHEAT, 0);
            this.Inventory.Outgoing.Add(RessourceType.WHEAT);
        }

        public WheatFarm(
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
