
using System.Collections.Generic;

namespace Shared.GameState
{
    public class GameState
    {
        public HexMap map;
        public List<uint> buildings;       


        public GameState(HexMap map)
        {
            this.map = map;
            buildings = new List<uint>();
        }

        public GameState(HexMap map, List<uint> buildings)
        {
            this.map = map;
            this.buildings = buildings;
        }
    }
}