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
    public class Barracks: ProgressBuilding
    {
        public override string description => "Barracks are used to produce Troops which are needed to attack other Tribes or to defend your own Buildings from being attacked. Different Troops need different Ressources. An Archer requires 4 Wood, 1 Leather and 1 Food. A Knight requires 2 Iron and 1 Food and a Spearman requires 1 Iron, 1 Leather and 1 Food.";
        public override byte MaxLevel => 3;
        public override byte[] MaxHealths => new byte[]{
            12,
            14,
            16
        };

        public override int[] RessourceLimits => new int[] {
            12,
            12,
            24
        };

        public override int[] MaxProgresses => new int[]
        {
            Constants.MinutesToGameTicks(480),
            Constants.MinutesToGameTicks(300),
            Constants.MinutesToGameTicks(120)
        };

        public Dictionary<RessourceType, int> InputRecipe;

        public TroopType OutputTroop;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 4 }, { RessourceType.STONE, 8 }, { RessourceType.IRON, 3 }},
                    new Dictionary<RessourceType, int>{ { RessourceType.STONE, 10 }, { RessourceType.IRON, 5 }, { RessourceType.LEATHER, 2 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.STONE, 14 }, { RessourceType.IRON, 6 }, { RessourceType.LEATHER, 4 } }
                };
                return result;
            }
        }

        public Barracks() : base()
        {
            this.TroopInventory.TroopLimit = 60;
            this.ChangeTroopRecipe(TroopType.ARCHER);
        }

        public Barracks(
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

        public override void DoTick()
        {
            base.DoTick();
            if (Progress == 0 && Inventory.RecipeApplicable(InputRecipe) && TroopInventory.GetAvailableSpace() > 0)
            {
                Progress = 1;
                Inventory.ApplyRecipe(InputRecipe);
            }
        }

        public override void OnMaxProgress()
        {
            this.TroopInventory.AddUnit(this.OutputTroop, 2);
        }

        public void ChangeTroopRecipe(TroopType troopType)
        {
            switch (troopType)
            {
                case TroopType.ARCHER:
                {
                    this.InputRecipe = new Dictionary<RessourceType, int> { { RessourceType.WOOD, 4 }, { RessourceType.LEATHER, 1 }, { RessourceType.FOOD, 1 } };
                    break;
                }
                case TroopType.KNIGHT:
                {
                    this.InputRecipe = new Dictionary<RessourceType, int> { { RessourceType.IRON, 2 }, { RessourceType.FOOD, 1 } };
                    break;
                }
                case TroopType.SPEARMAN:
                {
                    this.InputRecipe = new Dictionary<RessourceType, int> { { RessourceType.IRON, 1 }, { RessourceType.LEATHER, 1 }, { RessourceType.FOOD, 1 } };
                    break;
                }
            }
            this.OutputTroop = troopType;

            this.Inventory.Storage.Clear();
            this.Inventory.Incoming.Clear();
            this.Inventory.RessourceLimits.Clear();
            foreach (RessourceType ressourceType in this.InputRecipe.Keys)
            {
                this.Inventory.AddRessource(ressourceType);
                this.Inventory.RessourceLimits[ressourceType] = this.RessourceLimit / this.InputRecipe.Count();
                this.Inventory.Incoming.Add(ressourceType);
            }
        }
    }
}
