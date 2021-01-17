using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.DataTypes;
using Shared.Structures;

namespace Shared.GameLogic
{
    public class GameLogic
    {
        public HexGrid.HexGrid grid;

        public GameLogic()
        {

        }

        public bool verifyBuild(HexCoordinates coords, Structure structure) 
        {
            HexCell cell = this.grid.GetCell(coords);
            if (structure != null && structure.IsPlaceable(cell))
            { 
                return true;
            }

            return false;
        }

        public HexCell applyBuild(HexCoordinates coords, Structure structure)
        {
            HexCell cell = this.grid.GetCell(coords);
            cell.Structure = structure;
            return cell;
        }


    }
}
