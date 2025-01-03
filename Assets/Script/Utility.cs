public static class Utility
{
    public static bool WithinRange(int value, int min, int max)
    {
        return value >= min && value <= max;
    }

    public static bool WithinRange(float value, float min, float max)
    {
        return value >= min && value <= max;
    }

    public static int GetTerrainHash(TerrainObject TerrainObj)
    {
        int hash = 17;

        foreach (Terrain terrain in TerrainObj.Terrains)
        {
            hash = hash * 31 + terrain.Name.GetHashCode();
            hash = hash * 31 + terrain.MinHeight.GetHashCode();
            hash = hash * 31 + terrain.MaxHeight.GetHashCode();

            foreach (Biome biome in terrain.Biomes)
            {
                hash = hash * 31 + biome.Name.GetHashCode();
                hash = hash * 31 + biome.MinTemperature.GetHashCode();
                hash = hash * 31 + biome.MaxTemperature.GetHashCode();
                foreach (TileRule tileRule in biome.TileRules)
                {
                    hash = hash * 31 + tileRule.GetHashCode();
                }
            }
        }

        return hash;
    }

}
