using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class RandomHelper
{
    public static T PickRandom<T>(this IList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count())];
    }

    /// <summary>
    /// If 70 then there is 70% to roll true.
    /// </summary>
    /// <param name="chanceHundredMax">Number between 0 and 100.</param>
    /// <returns></returns>
    public static bool RollPercent(float chanceHundredMax)
    {
        if (chanceHundredMax <= 0) return false;
        var rolledNumber = UnityEngine.Random.Range(0f, 100f);
        var isLucky = rolledNumber < chanceHundredMax;
        return isLucky;
    }
}

