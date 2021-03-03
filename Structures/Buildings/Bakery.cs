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
    class Bakery : RefineryBuilding
    {
        public override byte MaxLevel => 1;
        public override byte[] MaxHealths => new byte[]{
            50
        };

        public override int[] RessourceLimits => new int[] {
            6,
            12,
            26
        };

        public override int MaxProgress => 10;
        public override Dictionary<RessourceType, int> InputRecipe => new Dictionary<RessourceType, int> { { RessourceType.WHEAT, 2 }, { RessourceType.WOOD, 1 } };
        public override Dictionary<RessourceType, int> OutputRecipe => new Dictionary<RessourceType, int> { { RessourceType.FOOD, 1 } };


        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 3 }, { RessourceType.STONE, 2 }  }
                };
                return result;
            }
        }

        public Bakery() : base()
        {
            
        }

        public Bakery(
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

        public override bool IsPlaceable(HexCell cell)
        {            
            return base.IsPlaceable(cell) ;
        }
    }
}
