using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Shared.DataTypes
{
    public enum HexCellBiome
    {
        FOREST, SCRUB, GRASS, CROP, ROCK, SNOW, CITY, BUILDINGS, WATER
    }


    public struct HexCellData
    {
        public int Elevation { get; }
        public HexCellBiome Biome { get; set; }        
        public byte WaterDepth { get; }      

        public HexCellData(int elevation, HexCellBiome biome, byte waterDepth)
        {
            Elevation = elevation;
            Biome = biome;
            WaterDepth = waterDepth;
        }

        public string toString()
        {
            return  this.Elevation + ", " + this.Biome + ", " + this.WaterDepth;
        }
    }
}
