﻿using Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using UnityEngine;

namespace Shared.Structures
{
    public class CowFarm : ProductionBuilding
    {
        public override string description => "The Cowfarm is used to get Cows from a nearby Cowressource. The Cowfarm needs to be placed adjacent to atleast one Cow. More adjacent Cows will improve the efficiency of the farm.";
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

        public override int MaxProgress => 2;
        
        public override RessourceType ProductionType => RessourceType.COW;

        public override byte Gain => 4;



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

        public CowFarm() : base()
        {
            this.Inventory.Storage.Add(RessourceType.COW, 0);
            this.Inventory.Outgoing.Add(RessourceType.COW);
        }

        public CowFarm(
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
