﻿using System;
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
        public override string description => "The Coalmine is used to produce Coal from a nearby Coalressource. The Coalmine needs to be placed adjacent to atleast one Coalressource. More adjacent Coalressources will improve the efficiency of the mine.";
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
            TroopInventory TroopInventory,
            BuildingInventory Inventory,
            int Progress
            ) : base(Cell, Tribe, Level, Health, TroopInventory, Inventory, Progress)
        {
            
        }
    }
}
