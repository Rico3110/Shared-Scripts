using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GameState
{
    public class HexMap
    {
        public uint[] data;
        public int chunkCountX;
        public int chunkCountZ;

        double lat;
        double lon;

        public HexMap(uint[] data, int chunkCountX, int chunkCountZ, double lat, double lon)
        {
            this.data = data;
            this.chunkCountX = chunkCountX;
            this.chunkCountZ = chunkCountZ;
            this.lat = lat;
            this.lon = lon;
        }
    }
}
