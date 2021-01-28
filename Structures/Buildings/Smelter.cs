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
    class Smelter : InventoryBuilding
    {
        public override byte MaxLevel => 3;
        public override byte MaxHealth => 100;

        public Smelter() : base()
        {
            this.Inventory.Storage.Add(RessourceType.IRON_ORE, 0);
            this.Inventory.Storage.Add(RessourceType.COAL, 0);
            this.Inventory.Storage.Add(RessourceType.IRON, 0);
            this.Inventory.RessourceLimit = 20;
            this.Inventory.RessourceLimits.Add(RessourceType.IRON_ORE, 9);
            this.Inventory.RessourceLimits.Add(RessourceType.COAL, 9);
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
            Inventory Inventory
            ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory)
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
            SendRessources();
        }

        public override bool IsPlaceable(HexCell cell)
        {            
            return base.IsPlaceable(cell) ;
        }
    }
}
