using System;
using System.Linq;

namespace Shared.DataTypes
{
    public enum StructureType
    {
        BIG_ROCK, ROCK, SCRUB, TREES, BIG_TREES, FISH, WHEAT
    }


    public enum RessourceType
    {
        WOOD, STONE, IRON, COAL, WHEAT, COW, FOOD, LEATHER
    }

    public static class RessourceTypeExtenstion
    {
        public static string ToFriendlyString(this RessourceType ressourceType)
        {
            return ressourceType.ToString().ToLower().FirstCharToUpper();
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
