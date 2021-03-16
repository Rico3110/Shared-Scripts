using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Structures
{
    class LandRoad : Road
    {
        public override string description => "The Landroad can be used to connect Buildings with each other so that they can exchange Ressources.";

        public override byte MaxLevel => 3;

        public override byte[] MaxHealths => new byte[]{
            5,
            7,
            10
        };

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ },
                    new Dictionary<RessourceType, int>{ { RessourceType.WOOD, 1 } },
                    new Dictionary<RessourceType, int>{ { RessourceType.STONE, 1 } }
                };
                return result;
            }
        }

        public LandRoad() : base()
        {

        }

        public LandRoad(
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
            if (cell.Data.Biome == HexCellBiome.WATER)
                return false;
            return base.IsPlaceable(cell);
        }

        public override void DoTick()
        {
            base.DoTick();
        }
    }
}
