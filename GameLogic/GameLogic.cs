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

        public List<Building> buildings;

        public List<Ressource> ressources;

        public GameLogic()
        {
            buildings = new List<Building>();
            ressources = new List<Ressource>();
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
            if (cell.Structure != null)
            {
                ressources.Remove((Ressource)cell.Structure);
            }
            cell.Structure = structure;
            structure.Cell = cell;
            AddStructureToList(structure);
            return cell;
        }

        public void Init(HexGrid.HexGrid grid)
        {
            this.grid = grid;
            foreach (HexCell cell in grid.cells)
            {
                if (cell.Structure != null) {
                    this.AddStructureToList(cell.Structure);
                }
            }
        }

        private void AddStructureToList(Structure structure)
        {           
            if (typeof(Building).IsAssignableFrom(structure.GetType()))
            {
                if (!buildings.Contains(structure))
                {
                    buildings.Add((Building)structure);
                }
            }
            else if (typeof(Ressource).IsAssignableFrom(structure.GetType()))
            {
                if (!ressources.Contains(structure))
                {
                    ressources.Add((Ressource)structure);
                }
            }
        }

        public void DoTick()
        {
            foreach(Ressource ressource in ressources)
            {
                ressource.DoTick();
            }
            foreach (Building building in buildings)
            {
                building.DoTick();
                /*
                for(HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
                {
                    HexCell neighbor = building.Cell.GetNeighbor(d);
                    if (neighbor != null && neighbor.Structure != null && neighbor.Structure is Ressource)
                    {
                        if(((Ressource)neighbor.Structure).Progress == 0)
                            this.AddStructureToList(neighbor.Structure);
                    }
                }
                */
            }
        }
    }
}
