using System;
using System.IO;
using System.Collections.Generic;
public static class Convolve
{
    public static float[] Convo(float[] array1, float[] array2)
    {
        int n = array1.Length;
        int m = array2.Length;
        int resultLength = n + m - 1;
        float[] result = new float[resultLength];

        for (int i = 0; i < resultLength; i++)
        {
            float sum = 0.0f;
            int jMin = Math.Max(0, i - m + 1);
            int jMax = Math.Min(n - 1, i);
            for (int j = jMin; j <= jMax; j++)
            {
                sum += array1[j] * array2[i - j];
            }
            result[i] = sum;
            //Console.WriteLine(result[i]);
        }

        return result;
    }
}