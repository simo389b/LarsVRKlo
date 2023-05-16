public class DirectivityV3
{
    private double[,] directivityScalars; 
    public DirectivityV3(double[,] ISMPositions, double[] receiverPosition, int[,] wallIds, double[] OGSourceDir)
    {
        int noOfSovser = ISMPositions.GetLength(0);
        int noOfCoords = ISMPositions.GetLength(1);
        double[] OGPoint = new double[]{1,2,2}; //MATCH MED point FRA MAIN
        directivityScalars = new double[noOfSovser+1,2];
        double[,] pointVectors = new double[noOfSovser+1, noOfCoords];
        double[] distances = new double[noOfSovser+1];


        //
        double[,] wallIDsTEMP = new double[noOfSovser+1, 2];

        for (int i = 1; i < noOfSovser+1; i++) 
        {
            for (int j = 0; j < wallIds.GetLength(1); j++)
            {
                if(i == 1)
                {
                    wallIDsTEMP[0, j] = -1;
                }
                wallIDsTEMP[i, j] = wallIds[i-1,j];
            }

        }


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

            azimuth[v] += OGSourceDir[0];
            elevation[v] += OGSourceDir[1];


            Console.WriteLine("Source no.: {0}", v);
            Console.WriteLine("Azimuth (°): {0}, Elevation (°): {1})", azimuth[v]*180/Math.PI, elevation[v]*180/Math.PI); 


            //if(wallIDsTEMP[v,1] == -1 & v != 0)
            if(v != 0)
            {
                if(ISMPositions[v-1,0] != OGPoint[0])
                {
                    directivityScalars[v,0] = (0.5*(1.0-Math.Cos(azimuth[v]))+1.0)/2.0; //
                }
                else
                {
                    directivityScalars[v,0] = (0.5*(1.0-Math.Cos(azimuth[v]+Math.PI))+1.0)/2.0;
                }
                if(ISMPositions[v-1,2] != OGPoint[2])
                {
                    directivityScalars[v,1] = (0.5*(1.0-Math.Cos(elevation[v]))+1.0)/2.0; //
                }
                else
                {
                    directivityScalars[v,1] = (0.5*(1.0-Math.Cos(elevation[v]+Math.PI))+1.0)/2.0; //
                }

            }
            else if (v == 0)
            {
                directivityScalars[v,0] = (0.5*(1.0-Math.Cos(azimuth[v]+Math.PI))+1.0)/2.0; //
                directivityScalars[v,1] = (0.5*(1.0-Math.Cos(elevation[v]+Math.PI))+1.0)/2.0; //
            }
            else 
            {
                directivityScalars[v,0] = (0.5*(1.0-Math.Cos(azimuth[v]))+1.0)/2.0; //
                directivityScalars[v,1] = (0.5*(1.0-Math.Cos(elevation[v]))+1.0)/2.0; //
            }
            Console.WriteLine("Directivity: Azimuth: {0}, Elevation: {1})", directivityScalars[v,0], directivityScalars[v,1]);
        } 
    }
}