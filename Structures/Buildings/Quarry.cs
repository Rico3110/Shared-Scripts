﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;
using Shared.Communication;

namespace Shared.Structures
{
    class Quarry : ProductionBuilding
    {
        public override byte MaxLevel => 3;
        public override byte[] MaxHealths => new byte[]{
            50,
            100,
            200
        };
        public override int[] RessourceLimits => new int[] {
            4,
            10,
            20
        };
        public override int MaxProgresses => new int[] {
            Constants.MinutesToGameTicks(120),
            Constants.MinutesToGameTicks(90),
            Constants.MinutesToGameTicks(60),
        };
        public override RessourceType ProductionType => RessourceType.STONE;
        public override byte Gain => 2;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ {RessourceType.WOOD, 5 } },
                    new Dictionary<RessourceType, int>{ {RessourceType.STONE, 5 } },
                    new Dictionary<RessourceType, int>{ {RessourceType.IRON, 2 } }
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
            TroopInventory TroopInventory,
            BuildingInventory Inventory,
            int Progress
            ) : base(Cell, Tribe, Level, Health, TroopInventory, Inventory, Progress)
        {

        }
    }
}
