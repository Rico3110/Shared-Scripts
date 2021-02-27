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
    public class Market : RefineryBuilding
    {
        public override byte MaxLevel => 3;
        public override byte[] MaxHealths => new byte[]{
            50,
            60,
            70
        };

        public override int[] RessourceLimits => new int[] {
            11,
            9,
            7
        };

        public override int MaxProgress => 10;

        public RessourceType TradeInput;

        public RessourceType TradeOutput;
        public override Dictionary<RessourceType, int> InputRecipe => new Dictionary<RessourceType, int> { { TradeInput, RessourceLimit - 1 } };
        public override Dictionary<RessourceType, int> OutputRecipe => new Dictionary<RessourceType, int> { { TradeOutput, 1 } };


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
            int Progress,
            RessourceType TradeInput,
            RessourceType TradeOutput
            ) : base(Cell, Tribe, Level, Health, TroopInventory, Inventory, Progress)
        {
            this.TradeInput = TradeInput;
            this.TradeOutput = TradeOutput;
        }

        public override bool IsPlaceable(HexCell cell)
        {            
            return base.IsPlaceable(cell) ;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            this.Inventory.RessourceLimits.Clear();
            this.Inventory.RessourceLimits.Add(TradeInput, this.RessourceLimit - 1);
            this.Inventory.RessourceLimits.Add(TradeOutput, 1);
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

            this.Inventory.RessourceLimits.Clear();
            this.Inventory.RessourceLimits.Add(inputRessource, this.RessourceLimit - 1);
            this.Inventory.RessourceLimits.Add(outputRessource, 1);

            this.TradeInput = inputRessource;
            this.TradeOutput = outputRessource;
            this.Inventory.UpdateIncoming(new List<RessourceType> { inputRessource });
            this.Inventory.UpdateOutgoing(new List<RessourceType> { outputRessource });
        }
    }
}
