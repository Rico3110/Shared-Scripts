using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using Shared.HexGrid;

namespace Shared.Structures
{
    public abstract class RefineryBuilding : ProgressBuilding
    {
        public abstract Dictionary<RessourceType, int> InputRecipe { get; }
        public abstract Dictionary<RessourceType, int> OutputRecipe { get; }

        public RefineryBuilding() : base()
        {
            Progress = 0;
        }

        public RefineryBuilding(
            HexCell Cell,
            byte Tribe,
            byte Level,
            byte Health,
            int TroopCount,
            BuildingInventory Inventory,
            int Progress
        ) : base(Cell, Tribe, Level, Health, TroopCount, Inventory, Progress)
        {

        }

        public override void OnMaxProgress()
        {
            Inventory.AddRessource(OutputRecipe);            
        }

        public override void DoTick()
        {
            base.DoTick();
            if (Progress == 0 && Inventory.RecipeApplicable(InputRecipe) && Inventory.HasAvailableSpace(OutputRecipe))
            {
                Progress = 1;
                Inventory.ApplyRecipe(InputRecipe);
            }

        }
    }
}
