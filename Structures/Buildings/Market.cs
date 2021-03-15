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

        public override string description => "The Market can be used to exchange any Ressource into any other Ressource. Higher levels of the market offer will offer a better trade ratio between those Ressources.";
        
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
            this.ChangeInputRecipe(RessourceType.WOOD);
            this.ChangeOutputRecipe(RessourceType.IRON);
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

        public void ChangeInputRecipe(RessourceType inputRessource)
        {
            this.Progress = 0;
            this.Inventory.Storage.Clear();
            
            this.Inventory.AddRessource(inputRessource);
            this.Inventory.AddRessource(TradeOutput);
            
            this.TradeInput = inputRessource;

            this.Inventory.RessourceLimits.Clear();
            this.Inventory.RessourceLimits.Add(TradeInput, this.RessourceLimit - 1);
            if (this.Inventory.RessourceLimits.ContainsKey(TradeOutput))
                this.Inventory.RessourceLimits[TradeOutput] = this.RessourceLimit;
            else
                this.Inventory.RessourceLimits.Add(TradeOutput, 1);

            this.Inventory.UpdateIncoming(new List<RessourceType> { inputRessource });
        }

        public void ChangeOutputRecipe(RessourceType outputRessource)
        {
            this.Progress = 0;
            this.Inventory.Storage.Clear();
            
            this.Inventory.AddRessource(TradeInput);
            this.Inventory.AddRessource(outputRessource);

            this.TradeOutput = outputRessource;
            
            this.Inventory.RessourceLimits.Clear();
            this.Inventory.RessourceLimits.Add(TradeInput, this.RessourceLimit - 1);
            if (this.Inventory.RessourceLimits.ContainsKey(TradeOutput))
                this.Inventory.RessourceLimits[TradeOutput] = this.RessourceLimit;
            else
                this.Inventory.RessourceLimits.Add(TradeOutput, 1);

            this.Inventory.UpdateOutgoing(new List<RessourceType> { outputRessource });
        }
    }
}
