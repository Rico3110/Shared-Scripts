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
    class Smelter : RefineryBuilding
    {
        public override string description => "The Smelter is used to process Stone and Coal into Iron.";
        public override byte MaxLevel => 1;
        public override byte[] MaxHealths => new byte[]{
            10,
            12,
            16
        };

        public override int[] RessourceLimits => new int[] {
            6,
            12,
            26
        };

        public override int[] MaxProgresses => new int[] {
            Constants.MinutesToGameTicks(240),
            Constants.MinutesToGameTicks(150),
            Constants.MinutesToGameTicks(60)
        };
        public override Dictionary<RessourceType, int> InputRecipe => new Dictionary<RessourceType, int> { { RessourceType.COAL, 1 }, { RessourceType.STONE, 1 } };
        public override Dictionary<RessourceType, int> OutputRecipe => new Dictionary<RessourceType, int> { { RessourceType.IRON, 1 } };


        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 6 }, { RessourceType.STONE, 4 }  },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 8 }, { RessourceType.STONE, 6 }, { RessourceType.IRON, 2 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 8 }, { RessourceType.STONE, 8 }, { RessourceType.IRON, 5 } }
                };
                return result;
            }
        }

        public Smelter() : base()
        {
            
        }

        public Smelter(
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
