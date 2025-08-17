using Godot;
using System;
using System.Collections.Generic;

public static class Utils
{
    public static void ShuffleList<T>(List<T> list)
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.RandiRange(0, i);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
