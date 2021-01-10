using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shared.DataTypes
{
    class Storage : Building
    {
        public override byte MAX_LEVEL => 0;

        public override byte INV_SIZE => 4;

        public Storage()
        {

        }

        public Storage(byte Level, byte Progress)
        {
            this.Level = Level;
            this.Progress = Progress;
        }

        public override void DoTick()
        {
            throw new NotImplementedException();
        }
    }
}
