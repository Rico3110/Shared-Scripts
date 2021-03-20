using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Communication
{
    class Constants
    {
        public const int TICKS_PER_SEC = 30;
        public const int MS_PER_TICK = 1000 / TICKS_PER_SEC;

        public const int COUNTER_MAX = 150;

        public const int SEC_PER_GAMETICK = COUNTER_MAX / TICKS_PER_SEC;

        public static int MinutesToGameTicks(int minutes)
        {
            return (int)(minutes * (1f / (float)SEC_PER_GAMETICK));
        }

        public static int HoursToGameTicks(int hours)
        {
            return ((hours * 60) / SEC_PER_GAMETICK);
        }
    }
}
