using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Game
{
    public class Player
    {
        public string Name;

        public Tribe Tribe;

        public HexCoordinates Position;

        public Player
        (
            string Name 
        )
        {
            this.Name = Name;
        }

    }
}
