using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Structures;

namespace Shared.Game
{
    public class Tribe
    {
        public byte Id;
        public Headquarter HQ;

        public Tribe
        (
            byte id,
            Headquarter hq
        )
        {
            this.Id = id;
            this.HQ = hq;
        }
    }
}
