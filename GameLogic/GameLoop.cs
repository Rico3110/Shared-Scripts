using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.GameLogic
{
    public class GameLoop
    {
        public List<Building> buildings;

        public void Init(HexGrid.HexGrid grid)
        {
            foreach(HexCell cell in grid.cells)
            {
                if(cell.Building != null)
                {
                    //buildings.Add(cell.Building);
                }
            }
        }

        public void doTick()
        {
            foreach(Building building in buildings)
            {
                building.DoTick();
            }
        }
    }
}
