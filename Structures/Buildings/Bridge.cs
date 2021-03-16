using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    public class Bridge : Road
    {
        public override string description => "The Bridge can be used to connect buildings so that those can transfer ressources between each other. A bridge needs to be placed on water.";
        public override byte MaxLevel => 3;

        public override byte[] MaxHealths => new byte[]{
            5,
            7,
            10
        };

        private int BridgeHeight = 2;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 5 }},
                    new Dictionary<RessourceType, int>{ { RessourceType.STONE, 4 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 2 }, { RessourceType.IRON, 1 } }
                };
                return result;
            }
        }

        public Bridge() : base()
        {

        }

        public Bridge(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health
        ) : base(
            Cell,
            Tribe,
            Level,
            Health
        )
        {

        }

        public override bool IsPlaceable(HexCell cell)
        {
            if(cell.Data.Biome != HexCellBiome.WATER)
            {
                return false;
            }
            return base.IsPlaceable(cell);
        }

        public override void DoTick()
        {
            base.DoTick();
        }

        public override bool HasBuilding(HexDirection direction)
        {
            return HasRoad(direction);
        }

        public override int GetElevation()
        {
            return this.Cell.Data.Elevation + this.BridgeHeight;
        }
    }
}
