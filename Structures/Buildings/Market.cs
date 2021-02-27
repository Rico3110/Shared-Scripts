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
    class Market : RefineryBuilding
    {
        public override byte MaxLevel => 3;
        public override byte[] MaxHealths => new byte[]{
            50
        };

        public override int[] RessourceLimits => new int[] {
            6,
            12,
            26
        };

        public override int MaxProgress => 10;
        public override Dictionary<RessourceType, int> InputRecipe => new Dictionary<RessourceType, int> { { RessourceType.STONE, 1 } }
        public override Dictionary<RessourceType, int> OutputRecipe => new Dictionary<RessourceType, int> { { RessourceType.IRON, 1 } };


        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 3 }  },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 5 }, { RessourceType.STONE, 4 }  },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 2 }, { RessourceType.STONE, 6 }, { RessourceType.IRON, 2 } }
                };
                return result;
            }
        }

        public Market() : base()
        {
            this.ChangeInOutputRecipes(RessourceType.WOOD, RessourceType.IRON);
        }

        public Market(
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

        public void ChangeInOutputRecipes(RessourceType inputRessource, RessourceType outputRessource)
        {
            foreach (RessourceType type in this.InputRecipe.Keys)
            {
                this.Inventory.RemoveRessource(type);
            }
            foreach (RessourceType type in this.OutputRecipe.Keys)
            {
                this.Inventory.RemoveRessource(type);
            }
            this.Inventory.AddRessource(inputRessource);
            this.Inventory.AddRessource(outputRessource);
            this.InputRecipe.Clear();
            this.InputRecipe.Add(inputRessource, 10);
            this.OutputRecipe.Clear();
            this.OutputRecipe.Add(outputRessource, 1);
        }
    }
}
