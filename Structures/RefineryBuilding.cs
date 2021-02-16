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
            foreach(RessourceType ressourceType in InputRecipe.Keys)
            {
                this.Inventory.AddRessource(ressourceType);
            }
            foreach (RessourceType ressourceType in OutputRecipe.Keys)
            {
                this.Inventory.AddRessource(ressourceType);
            }
            this.Inventory.UpdateIncoming(InputRecipe.Keys.ToList());
            this.Inventory.UpdateOutgoing(OutputRecipe.Keys.ToList());
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

        public override void Upgrade()
        {
            base.Upgrade();
            foreach (RessourceType ressourceType in this.Inventory.Storage.Keys)
            {
                this.Inventory.RessourceLimits[ressourceType] = this.RessourceLimit / this.Inventory.Storage.Keys.Count;

            }

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
