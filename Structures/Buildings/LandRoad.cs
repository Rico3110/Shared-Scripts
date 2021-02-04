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
        public override byte MaxLevel => 3;

        public override byte MaxHealth => 100;

        public override Dictionary<RessourceType, int>[] Recipes
        {
            get
            {
                Dictionary<RessourceType, int>[] result = {
                    new Dictionary<RessourceType, int>{ },
                    new Dictionary<RessourceType, int>{ },
                    new Dictionary<RessourceType, int>{ }
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
            return base.IsPlaceable(cell);
        }
    }
}
