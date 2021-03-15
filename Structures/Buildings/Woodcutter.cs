using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.Communication;
using UnityEngine;

namespace Shared.Structures
{
    public class Woodcutter : ProductionBuilding
    {
        public override byte MaxLevel => 3;

        public override byte[] MaxHealths => new byte[]{
            10,
            20,
            25
        };

        public override int[] RessourceLimits => new int[] {
            4,
            8,
            12
        };

        public override int[] MaxProgresses => new int[] {
            Constants.MinutesToGameTicks(90),
            Constants.MinutesToGameTicks(50),
            Constants.MinutesToGameTicks(20)
        };

        public override RessourceType ProductionType => RessourceType.WOOD;

        public override byte Gain => 1;



        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 1} },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 5} }
                };
                return result;
            }
        }

        public Woodcutter() : base()
        {
            this.Inventory.Storage.Add(RessourceType.WOOD, 0);
            this.Inventory.Outgoing.Add(RessourceType.WOOD);
        }

        public Woodcutter(
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
