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
        FOREST, GRASS, CROP, ROCK, SNOW, CITY, WATER
    }


    public struct HexCellData
    {
        public uint Elevation { get; }
        public HexCellBiome Biome { get; }        
        public byte WaterDepth { get; }

        public HexCellData(uint elevation, HexCellBiome biome, byte waterDepth)
        {
            Elevation = elevation;
            Biome = biome;
            WaterDepth = waterDepth;
        }

        public HexCellData(uint data)
        {
            Elevation = data.toElevation();
            Biome = data.toBiome();
            WaterDepth = data.toWaterDepth();
        }

        public uint toUint()
        {
            uint data = 0;
            data = data.SetSubBits(Elevation, 0, 16);
            data = data.SetSubBits((uint)Biome, 16, 4);
            data = data.SetSubBits((uint)WaterDepth, 20, 8);
            return data;
        }

        public string toString()
        {
            return  this.Elevation + ", " + this.Biome + ", " + this.WaterDepth;
        }
    }

    internal static class HexCellDataHelper
    {
        internal static uint toElevation(this uint data)
        {
            return data.GetSubBits(0, 16);
        }

        internal static HexCellBiome toBiome(this uint data)
        {
            return (HexCellBiome)data.GetSubBits(16, 4);
        }
        internal static byte toWaterDepth(this uint data)
        {
            return (byte)data.GetSubBits(20, 8);
        }
    }
}
