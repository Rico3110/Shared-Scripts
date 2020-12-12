using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GameState
{
    public class Map
    {
        uint[] map;
        int chunkCountX;
        int chunkCountZ;

        double lat;
        double lon;

        public Map(uint[] map, int chunkCountX, int chunkCountZ, double lat, double lon)
        {
            this.map = map;
            this.chunkCountX = chunkCountX;
            this.chunkCountZ = chunkCountZ;
            this.lat = lat;
            this.lon = lon;
        }
    }
}
