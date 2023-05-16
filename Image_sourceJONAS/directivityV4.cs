public class DirectivityV4
{
    private double[] directivityScalars; 
    public DirectivityV4(double[] ISMPosition, double[] receiverPosition, int[] wallIds, double[] OGSourceDir, double[] OGPoint)
    {
        int noOfCoords = ISMPosition.GetLength(0);
        double[] pointVector = new double[noOfCoords];
        directivityScalars = new double[2];
        double distance = 0;

        for (int j = 0; j < noOfCoords; j++)
        {
            pointVector[j] = receiverPosition[j] - ISMPosition[j];
            distance += pointVector[j]*pointVector[j]; 
        }
        distance = Math.Sqrt(distance);

        double azimuth = 0;
        double elevation = 0;

        azimuth = Math.Atan2(pointVector[1],(pointVector[0]));
        elevation = Math.Asin(pointVector[2]/distance);

        azimuth += OGSourceDir[0];
        elevation += OGSourceDir[1];

        //Console.WriteLine("Source no.: {0}", v);
        Console.WriteLine("Azimuth (°): {0}, Elevation (°): {1})", azimuth*180/Math.PI, elevation*180/Math.PI);

        if(wallIds[0] == -1 & wallIds[1] == -1) // Original kilde
        {
            directivityScalars[0] = (0.5*(1.0-Math.Cos(azimuth+Math.PI))+1.0)/2.0;
            directivityScalars[1] = (0.5*(1.0-Math.Cos(elevation+Math.PI))+1.0)/2.0;
        } 
        else //if (wallIds[1] == -1)
        {
            if(ISMPosition[0] != OGPoint[0])
            {
                directivityScalars[0] = (0.5*(1.0-Math.Cos(azimuth))+1.0)/2.0; //
            }
            else
            {
                directivityScalars[0] = (0.5*(1.0-Math.Cos(azimuth+Math.PI))+1.0)/2.0;
            }

            if(ISMPosition[2] != OGPoint[2])
            {
                directivityScalars[1] = (0.5*(1.0-Math.Cos(elevation+Math.PI))+1.0)/2.0; //
            }
            else
            {
                directivityScalars[1] = (0.5*(1.0-Math.Cos(elevation+Math.PI))+1.0)/2.0; //
            }
        }
        Console.WriteLine("Directivity: Azimuth: {0}, Elevation: {1})", directivityScalars[0], directivityScalars[1]);
        //else{}
    }

    public double getDirectivityScalar()
    {
        return (directivityScalars[0]+directivityScalars[1])/2;
    }
}