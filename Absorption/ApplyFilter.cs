using System;

public static class ApplyFilter {
    private static float[] internalBuffer = new float[30];
    private static float[] internalBuffer2 = new float[60];
    //private static string filename = "Filters.txt";
    
    //private static float[] twoFilters;
    //private static float[] coefficients; 
    private static float output = new float();
    public static float applyFilter(float input, float[] coefficients) {
        //Console.WriteLine();
        output = 0;
        if (coefficients.Length > 30 )
        {
            for (int i = 1; i < internalBuffer2.Length; i++ )
            {
                //Console.WriteLine(internalBuffer.Length-i-1);
                internalBuffer2[internalBuffer2.Length-i] = internalBuffer2[internalBuffer2.Length-i-1];
            }
            internalBuffer2[0] = input;
            Console.WriteLine(internalBuffer2[0]);

            int n = coefficients.Length;
            int m = internalBuffer2.Length;
            //float output = 0.0f; 
            for (int i = 0; i < m-1; i++) 
            {
                output += internalBuffer2[i]*coefficients[i];
            }
            
        }
        else
        {
            for (int i = 1; i < internalBuffer.Length; i++ )
            {
                //Console.WriteLine(internalBuffer.Length-i-1);
                internalBuffer[internalBuffer.Length-i] = internalBuffer[internalBuffer.Length-i-1];

            }
            internalBuffer[0] = input;
            //Console.WriteLine(coefficients[1]);
            //Console.WriteLine(internalBuffer[1]);

            int n = coefficients.Length;
            int m = internalBuffer.Length;
            //Console.WriteLine(m);
            //float output = 0.0f; 
            for (int i = 0; i < m; i++) 
            {
                //Console.WriteLine(i);
                output += internalBuffer[i]*coefficients[i];
                //Console.WriteLine(output);
            }
            
        }
        //output = 1.3f;
        return output;
    }
}

