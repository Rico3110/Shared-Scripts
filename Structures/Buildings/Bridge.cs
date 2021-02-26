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
        public override byte MaxLevel => 3;

        public override byte[] MaxHealths => new byte[]{
            30,
            50,
            100,
        };

        private int BridgeHeight = 5;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 1 }},
                    new Dictionary<RessourceType, int>{ { RessourceType.STONE, 1 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.IRON, 1 } }
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

        protected override int GetElevation()
        {
            return this.Cell.Data.Elevation + this.Cell.Data.WaterDepth + this.BridgeHeight;
        }
    }
}
