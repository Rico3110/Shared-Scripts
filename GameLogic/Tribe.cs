using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Structures;

namespace Shared.Game
{
    public class Tribe
    {
        public byte Id;
        public Headquarter HQ;

        public Dictionary<Type, int> CurrentBuildings;

        public static Dictionary<Type, int>[] BuildingLimits = {
            new Dictionary<Type, int>{ { typeof(Woodcutter), 2 }, { typeof(LandRoad), 10 }, { typeof(Bridge), 10 } },
            new Dictionary<Type, int>{ { typeof(Woodcutter), 2 }, { typeof(LandRoad), 15 }, { typeof(Bridge), 10 }, { typeof(Storage), 1 }, { typeof(Quarry), 2 } },
            new Dictionary<Type, int>{ { typeof(Woodcutter), 2 }, { typeof(LandRoad), 20 }, { typeof(Bridge), 10 }, { typeof(Storage), 2 }, { typeof(Quarry), 2 }, { typeof(IronMine), 1 }, { typeof(CoalMine), 1 }, { typeof(Smelter), 1 } }
        };

        public Dictionary<Type, int> BuildingLimit { get { return BuildingLimits[this.HQ.Level - 1]; } }

        public Tribe
        (
            byte id,
            Headquarter hq
        )
        {
            this.Id = id;
            this.HQ = hq;
            this.CurrentBuildings = new Dictionary<Type, int> {
                { typeof(Woodcutter), 0 },
                { typeof(LandRoad), 0 },
                { typeof(Storage), 0 },
                { typeof(Quarry), 0 },
                { typeof(IronMine), 0 },
                { typeof(CoalMine), 0 },
                { typeof(Smelter), 0 },
            };
        }

        public Tribe
        (
            byte id,
            Headquarter hq,
            Dictionary<Type, int> currentBuildings
        )
        {
            this.Id = id;
            this.HQ = hq;
            this.CurrentBuildings = currentBuildings;
        }

        public bool BuildingPlacable(Type buildingType)
        {
            Dictionary<Type, int> limits = BuildingLimits[HQ.Level - 1];

            if (!limits.ContainsKey(buildingType))
                return false;
            if (!CurrentBuildings.ContainsKey(buildingType))
                CurrentBuildings.Add(buildingType, 0);
            if (limits[buildingType] > CurrentBuildings[buildingType])
                return true;

            return false;
        }

        public void AddBuilding(Type buildingType)
        {
            if (!CurrentBuildings.ContainsKey(buildingType))
                CurrentBuildings.Add(buildingType, 0);
            CurrentBuildings[buildingType] += 1;
        }

        public void RemoveBuilding(Type buildingType)
        {
            CurrentBuildings[buildingType] -= 1;
        }
    }
}
