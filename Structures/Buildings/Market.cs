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
    class Market : RefineryBuilding
    {
        public override byte MaxLevel => 3;
        public override byte[] MaxHealths => new byte[]{
            50,
            60,
            70
        };

        public override int[] RessourceLimits => new int[] {
            20,
            25,
            30
        };

        public override int MaxProgress => 10;

        private RessourceType TradeInput;

        private RessourceType TradeOutput;
        public override Dictionary<RessourceType, int> InputRecipe => new Dictionary<RessourceType, int> { { TradeInput, 12 - this.Level * 2 } };
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

            this.TradeInput = inputRessource;
            this.TradeOutput = outputRessource;
            this.Inventory.UpdateIncoming(new List<RessourceType> { inputRessource });
            this.Inventory.UpdateOutgoing(new List<RessourceType> { outputRessource });
        }
    }
}
