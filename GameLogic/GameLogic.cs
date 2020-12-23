using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.GameLogic
{
    public class GameLogic
    {
        public HexGrid.HexGrid grid;

        public GameLogic()
        {

        }

        public bool verifyBuild(HexCoordinates coords, BuildingData buildingData) 
        {
            HexCell cell = this.grid.GetCell(coords);
            if (cell.Building != null && cell.Building.Type != BuildingType.NONE)
            {
                return false;
            }

            switch (buildingData.Type)
            {
                case BuildingType.NONE:
                {
                    break;
                }
                case BuildingType.HQ:
                {
                    break;
                }
                case BuildingType.WOODCUTTER:
                {
                    if (cell.Data.Biome != HexCellBiome.FOREST)
                    {
                        bool foundForest = false;
                        for (int i = 0; i < 6; i++)
                        {
                            HexCell neighbor = cell.GetNeighbor((HexDirection)i);
                            if (neighbor != null)
                            {
                                if (neighbor.Data.Biome == HexCellBiome.FOREST)
                                {
                                    foundForest = true;
                                    break;
                                }
                            }
                        }
                        if (foundForest == false)
                        {
                            return false;
                        }
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }

            return true;
        }

        public HexCell applyBuild(HexCoordinates coords, BuildingData buildingData)
        {
            HexCell cell = this.grid.GetCell(coords);
            cell.Building = buildingData;
            return cell;
        }


    }
}
