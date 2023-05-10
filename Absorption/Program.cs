using System;

class Program {
    //private static float output = new float();
    static void Main(string[] args) 
    {   
        int[] walls = {2,-1};
        string fil = "TESTTESTTEST.txt";
        float[][] testInput = txtReader.Read(fil);
        float[] coefficients;
        Console.WriteLine(testInput[0][0]);
        //Console.WriteLine(testInput[0][1]);
        float input;
        float output;
        float[] outputArray = new float[1000];
        FirFilter filter = new FirFilter();
        coefficients = filter.FirFilterFunc(walls);
        //Console.WriteLine(coefficients[0]);
            for (int j = 0; j < testInput[0].Length; j++)
            {  
                input = testInput[0][j];
                //Console.WriteLine(input);
                output = ApplyFilter.applyFilter(input, coefficients);
                outputArray[j] = output;
            }
            Console.WriteLine(outputArray[100]);
        //float testInput = 1;
         
        
    }
}
