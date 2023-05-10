using System;

public class FirFilter {
    //private static float 
    private float[] _coefficients;
    private static float[][] filterCoefficients;
    private static string filename = "Filters.txt";

    public float[] FirFilterFunc(int[] wallsHit) 
    {
        filterCoefficients = txtReader.Read(filename);
        //Console.WriteLine(filterCoefficients[0][0]);
        if (wallsHit[1] < 0)
        {
            _coefficients = filterCoefficients[wallsHit[0]];
        }
        else
        {
            _coefficients = Convolve.Convo(filterCoefficients[wallsHit[0]], filterCoefficients[wallsHit[1]]);
        }
        return _coefficients;
    }
}



