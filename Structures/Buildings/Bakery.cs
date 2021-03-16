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
    class Bakery : RefineryBuilding
    {

        public override string description => "The Bakery is used to process Wheat and Wood into Food.";
        public override byte MaxLevel => 3;
        public override byte[] MaxHealths => new byte[]{
            12,
            14,
            16
        };

        public override int[] RessourceLimits => new int[] {
            6,
            12,
            26
        };

        public override int[] MaxProgresses => new int[] {
            Constants.MinutesToGameTicks(180),
            Constants.MinutesToGameTicks(120),
            Constants.MinutesToGameTicks(50)
        };
        
        public override Dictionary<RessourceType, int> InputRecipe => new Dictionary<RessourceType, int> { { RessourceType.WHEAT, 2 }, { RessourceType.WOOD, 1 } };
        public override Dictionary<RessourceType, int> OutputRecipe => new Dictionary<RessourceType, int> { { RessourceType.FOOD, 1 } };


        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 5 }, { RessourceType.STONE, 2 }  },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 6 }, { RessourceType.STONE, 4 }, { RessourceType.LEATHER, 4 }  },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 8 }, { RessourceType.STONE, 4 }, { RessourceType.IRON, 4 } }
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
