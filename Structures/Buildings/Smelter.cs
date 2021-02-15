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
    class Smelter : RefineryBuilding
    {
        public override byte MaxLevel => 1;
        public override byte MaxHealth => 100;
        public override int MaxProgress => 10;
        public override Dictionary<RessourceType, int> InputRecipe => new Dictionary<RessourceType, int> { { RessourceType.COAL, 1 }, { RessourceType.IRON_ORE, 1 } };
        public override Dictionary<RessourceType, int> OutputRecipe => new Dictionary<RessourceType, int> { { RessourceType.IRON, 1 } };


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

        public Smelter() : base()
        {
            this.Inventory.Storage.Add(RessourceType.IRON_ORE, 0);
            this.Inventory.Storage.Add(RessourceType.COAL, 0);
            this.Inventory.Storage.Add(RessourceType.IRON, 0);
            this.Inventory.RessourceLimit = 26;
            this.Inventory.RessourceLimits.Add(RessourceType.IRON_ORE, 8);
            this.Inventory.RessourceLimits.Add(RessourceType.COAL, 8);
            this.Inventory.Incoming.Add(RessourceType.IRON_ORE);
            this.Inventory.Incoming.Add(RessourceType.COAL);
            this.Inventory.Outgoing.Add(RessourceType.IRON);
        }

        public Smelter(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            BuildingInventory Inventory,
            int Progress
            ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory, Progress)
        {
            
        }

        public override void DoTick()
        {
            base.DoTick();
            if (Inventory.GetRessourceAmount(RessourceType.COAL) > 0 && Inventory.GetRessourceAmount(RessourceType.IRON_ORE) > 0)
            {
                Inventory.RemoveRessource(RessourceType.COAL, 1);
                Inventory.RemoveRessource(RessourceType.IRON_ORE, 1);
                Inventory.AddRessource(RessourceType.IRON, 1);
            }
        }

        public override bool IsPlaceable(HexCell cell)
        {            
            return base.IsPlaceable(cell) ;
        }
    }
}
