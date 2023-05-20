// See https://aka.ms/new-console-template for more information
using System;
using System.Media;


public class modelParameters {

    public static double absorptionTotal(double[] absorptionCoefficients)
    {
        double absorpTotal = 0;
        for (int i = 0; i < absorptionCoefficients.Length; i++) {
            absorpTotal += absorptionCoefficients[i];
        }
        return absorpTotal;
    }

    public static double reverbTime(double volume, double absorption)
    {
        double T60 = 0;
        T60 = volume * absorption;
        return T60;
    }

    public static double decayCoefficient(double reverbTime)
    {
        double decayCoefficient = 0;
        decayCoefficient = Math.Log(10, Math.E)/(reverbTime * 6);
        return decayCoefficient;
    }
}


public class GuassianProcess 
{

    private static Random rand = new Random();

    public static double Next(double amplitude, double standardDeviation)
    {
        double SampleOne = rand.NextDouble();
        double SampleTwo = rand.NextDouble();
        double randomSignal = Math.Sqrt(-2.0 * Math.Log(SampleOne)) * Math.Cos(2.0 * Math.PI * SampleTwo);
        double whiteNoise = amplitude + randomSignal * standardDeviation;
        return whiteNoise;
    }
}



public class Reverb
{
    public static void Main()
    {
        double[] absorptionCoefficients = { 0.1, 0.2, 0.3, 0.4, 0.5 };
        double volume = 100;
        double absorption = modelParameters.absorptionTotal(absorptionCoefficients);
        double reverbTime = modelParameters.reverbTime(volume, absorption);
        double decayCoefficient = modelParameters.decayCoefficient(reverbTime);
        double[] reverbSignal = new double[1000];
        for (int i = 0; i < reverbSignal.Length; i++)
        {
            reverbSignal[i] = GuassianProcess.Next(0, 1);
        }
        for (int i = 0; i < reverbSignal.Length; i++)
        {
            reverbSignal[i] = reverbSignal[i] * Math.Exp(-decayCoefficient * i);
        }
        for (int i = 0; i < reverbSignal.Length; i++)
        {
            Console.WriteLine(reverbSignal[i]);
            //Save the array reverbsignal to a .wav file
            
            
        }
    }
}






