using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using Shared.Structures;

namespace Shared.Game
{
    public class Player
    {
        public string Name;

        public Tribe Tribe;

        public HexCoordinates Position;

        public TroopInventory TroopInventory;

        public Player(string name)
        {
            this.Tribe = null;
            this.TroopInventory = new TroopInventory();
        }

        public Player(string name, Tribe tribe)
        {
            this.Name = name;
            this.Tribe = tribe;
            this.TroopInventory = new TroopInventory();
        }

        public Player
        (
            string name,
            Tribe tribe,
            TroopInventory TroopInventory
        )
        {
            this.Name = name;
            this.Tribe = tribe;
            this.TroopInventory = TroopInventory;
        }
    }
}
