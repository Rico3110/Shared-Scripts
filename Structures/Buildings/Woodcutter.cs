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
    public class Woodcutter : ProductionBuilding
    {
        public override byte MaxLevel => 3;
        public override byte MaxHealth => 100;

        public override RessourceType ProductionType => RessourceType.WOOD;

        public override byte Gain => 4;

        public override int MaxProgress => 10;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 1} },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 1} }
                };
                return result;
            }
        }

        public Woodcutter() : base()
        {
            this.Inventory.Storage.Add(RessourceType.WOOD, 0);
            this.Inventory.RessourceLimit = 20;
            this.Inventory.RessourceLimits.Add(RessourceType.WOOD, 13);
            this.Inventory.Outgoing.Add(RessourceType.WOOD);
        }

        public Woodcutter(
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
    }
}
