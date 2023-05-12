public class DirectivityV3
{
    private double[,] directivityScalars; 
    public DirectivityV3(double[,] ISMPositions, double[] receiverPosition, int[,] wallIds, double[] patternDir)
    {
        int noOfSovser = ISMPositions.GetLength(0);
        int noOfCoords = ISMPositions.GetLength(1);
        double[] OGPoint = new double[]{1,2,2}; //MATCH MED point FRA MAIN
        directivityScalars = new double[noOfSovser+1,2];
        double[,] pointVectors = new double[noOfSovser+1, noOfCoords];
        double[] distances = new double[noOfSovser+1];


        for (int i = 1; i < noOfSovser+1; i++) // Laver en vektor fra ISM source til receiveren
        {
            for (int j = 0; j < noOfCoords; j++)
            {
                if (i == 1)
                {
                    pointVectors[0, j] = receiverPosition[j] - OGPoint[j];
                    distances[0] += pointVectors[0, j]*pointVectors[0, j]; //
                    //pointVectors[0, j] = OGPoint[j] - receiverPosition[j];
                }
                
                pointVectors[i, j] = receiverPosition[j] - ISMPositions[i-1, j]; 

                distances[i] += pointVectors[i, j]*pointVectors[i, j]; //
            }
            if (i == 1)
            {
                distances[0] = Math.Sqrt(distances[0]); 
            }
            distances[i] = Math.Sqrt(distances[i]);

        }

    double[] azimuth = new double[pointVectors.GetLength(0)];
    double[] elevation = new double[pointVectors.GetLength(0)];
    //double[] vectorsSubtract = new double[noOfCoords];

    for (int v = 0; v < pointVectors.GetLength(0); v++)
        {
            azimuth[v] = Math.Atan2(pointVectors[v, 1],(pointVectors[v, 0]));
            elevation[v] = Math.Asin(pointVectors[v, 2]/distances[v]);
            Console.WriteLine("Source no.: {0}", v);
            Console.WriteLine("Azimuth (°): {0}, Elevation (°): {1})", azimuth[v]*180/Math.PI, elevation[v]*180/Math.PI); 

            directivityScalars[v,0] = (0.5*(1.0-Math.Cos(azimuth[v]+Math.PI))+1.0)/2.0; //
            directivityScalars[v,1] = (0.5*(1.0-Math.Cos(elevation[v]+Math.PI))+1.0)/2.0; //
            Console.WriteLine("Directivity: Azimuth: {0}, Elevation: {1})", directivityScalars[v,0], directivityScalars[v,1]);
        } 
    }
}