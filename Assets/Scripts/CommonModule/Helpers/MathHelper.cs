using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class MathHelper
{
    public static int GetDislikeOffset(int value, int favoriteValue, int resilienceValue)
    {
        if (favoriteValue + resilienceValue <= value && favoriteValue - resilienceValue >= value) return 0;
        if (favoriteValue + resilienceValue <= value) return value - (favoriteValue + resilienceValue);
        else return (favoriteValue - resilienceValue) - value;
    }

    public static int Bound(this int val, int minPossible, int maxPossible)
    {
        if (val > maxPossible) return maxPossible;
        if (val < minPossible) return minPossible;
        return val;
    }

    public static int BoundMin(this int val, int minPossible)
    {
        if (val < minPossible) return minPossible;
        return val;
    }

    public static int BoundMax(this int val, int maxPossible)
    {
        if (val > maxPossible) return maxPossible;
        return val;
    }

    public static float Bound(this float val, float minPossible, float maxPossible)
    {
        if (val > maxPossible) return maxPossible;
        if (val < minPossible) return minPossible;
        return val;
    }

    public static float BoundMin(this float val, float minPossible)
    {
        if (val < minPossible) return minPossible;
        return val;
    }

    public static float BoundMax(this float val, float maxPossible)
    {
        if (val > maxPossible) return maxPossible;
        return val;
    }
}

