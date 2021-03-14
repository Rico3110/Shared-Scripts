using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTypes
{
    public enum TroopType
    {
        ARCHER, KNIGHT, SPEARMAN
    }

    public static class TroopTypeExtenstion
    {
        public static string ToFriendlyString(this TroopType troopType)
        {
            return troopType.ToString().ToLower().FirstCharToUpper();
        }

        private static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
