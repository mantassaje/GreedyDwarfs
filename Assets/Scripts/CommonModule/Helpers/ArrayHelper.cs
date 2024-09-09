using System.Collections.Generic;
public static class ArrayHelper
{
    public static List<T> ToFlatList<T>(T[,] doubleArray)
    {
        var result = new List<T>();
        if (doubleArray != null)
        {
            for (int x = 0; x < doubleArray.GetLength(0); x++)
                for (int y = 0; y < doubleArray.GetLength(1); y++)
                    result.Add(doubleArray[x, y]);
        }
        return result;
    }
    public static List<T> ToFlatList<T>(T[,,] doubleArray)
    {
        var result = new List<T>();
        if (doubleArray != null)
        {
            for (int x = 0; x < doubleArray.GetLength(0); x++)
                for (int y = 0; y < doubleArray.GetLength(1); y++)
                    for (int z = 0; z < doubleArray.GetLength(2); z++)
                        result.Add(doubleArray[x, y, z]);
        }
        return result;
    }
}

