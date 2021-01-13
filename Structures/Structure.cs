using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;

namespace Shared.Structures
{
    public abstract class Structure
    {
        public HexCell Cell { get; protected set; }

        public Structure(HexCell Cell)
        {
            this.Cell = Cell;
        }

        public abstract void DoTick();

        public virtual bool IsPlaceable(HexCell cell)
        {
            return false;
        }
    }
}
