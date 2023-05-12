
public class Directivity
{
    private double[,] directivityScalars; 
    public Directivity(double[,] ISMPositions, double[] receiverPosition, int[,] wallIds, double[] patternDir)
    {
        int noOfSovser = ISMPositions.GetLength(0);
        int noOfCoords = ISMPositions.GetLength(1);
        double[] OGPoint = new double[]{1,2,3}; //MATCH MED point FRA MAIN
        directivityScalars = new double[noOfSovser+1,2];
        double[,] pointVectors = new double[noOfSovser+1, noOfCoords];
        double[] magnitudes = new double[noOfSovser+1];
        for (int i = 1; i < noOfSovser+1; i++) // Laver en vektor fra ISM source til receiveren
        {
            for (int j = 0; j < noOfCoords; j++)
            {
                if (i == 1)
                {
                    pointVectors[0, j] = receiverPosition[j] - OGPoint[j];
                    //pointVectors[0, j] = OGPoint[j] - receiverPosition[j];
                }
                
                pointVectors[i, j] = receiverPosition[j] - ISMPositions[i-1, j]; 
                //pointVectors[i, j] = ISMPositions[i-1, j] - receiverPosition[j]; 
                //Console.WriteLine(pointVectors[i, j]);
            }

        }
        double[] azimuth = new double[pointVectors.GetLength(0)];
        double[] elevation = new double[pointVectors.GetLength(0)];
        double[] vectorsSubtract = new double[noOfCoords];

        
        for (int v = 0; v < pointVectors.GetLength(0); v++)
        {
            for (int j = 0; j < noOfCoords; j++)
            {
                //vectorsSubtract[j] = pointVectors[v, j] - patternDir[j];
                vectorsSubtract[j] = patternDir[j] - pointVectors[v, j];
            }
            //azimuth[v] = Math.Atan(vectorsSubtract[0]/(vectorsSubtract[1]+Math.Pow(10,-10)));
            //elevation[v] = Math.Atan(vectorsSubtract[2]/(vectorsSubtract[1]+Math.Pow(10,-10)));

            azimuth[v] = Math.Atan2(vectorsSubtract[0],(vectorsSubtract[1]));
            elevation[v] = Math.Atan2(vectorsSubtract[2],(vectorsSubtract[1]));
            Console.WriteLine("Source no.: {0}", v);
            Console.WriteLine("Azimuth (°): {0}, Elevation (°): {1})", azimuth[v]*180/Math.PI, elevation[v]*180/Math.PI); 

            directivityScalars[v,0] = (0.5*(1.0-Math.Cos(azimuth[v]+Math.PI))+1.0)/2.0;
            directivityScalars[v,1] = (0.5*(1.0-Math.Cos(elevation[v]+Math.PI))+1.0)/2.0;
            Console.WriteLine("Directivity: Azimuth: {0}, Elevation: {1})", directivityScalars[v,0], directivityScalars[v,1]); 
        }
    }
    public double[,] getDirectivityScalars()
    {
        return directivityScalars;
    }
}