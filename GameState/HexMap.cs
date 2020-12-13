using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GameState
{
    public class HexMap
    {
        public uint[] data { get; }
        public int chunkCountX { get; }
        public int chunkCountZ { get; }

        public float lat { get; }
        public float lon { get; }

        public HexMap(uint[] data, int chunkCountX, int chunkCountZ, float lat, float lon)
        {
            this.data = data;
            this.chunkCountX = chunkCountX;
            this.chunkCountZ = chunkCountZ;
            this.lat = lat;
            this.lon = lon;
        }
    }
}
