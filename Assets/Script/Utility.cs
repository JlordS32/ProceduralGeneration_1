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
}
