public class FirstImageSources {
    private double[,] pointVectors;
    private double[,] imageSources;
    private double[] dotNormalPointvector;
    private int noOfWalls;
    private int noOfCoords;
    private double[,] projection;
    private string[] wallsReflectedOn;
    
    public FirstImageSources(double[] point, double[,] wallNormals, double[,,] wallVertices) 
    {
        noOfWalls = wallNormals.GetLength(0); //Førsteordens generer én kilde per væg
        noOfCoords = wallNormals.GetLength(1); //X, Y, Z
        pointVectors = new double[noOfWalls, noOfCoords];
        imageSources = new double[noOfWalls, noOfCoords];
        dotNormalPointvector = new double[noOfWalls];

        for (int i = 0; i < noOfWalls; i++)
        {
            for (int j = 0; j < noOfCoords; j++)
            {
                pointVectors[i, j] = point[j] - wallVertices[i, 0, j]; 
            }
            dotNormalPointvector[i] = pointVectors[i, 0] * wallNormals[i, 0] +
                                      pointVectors[i, 1] * wallNormals[i, 1] + 
                                      pointVectors[i, 2] * wallNormals[i, 2] ;
            if (dotNormalPointvector[i] <= 0) //Flipper fortegnet, hvis vi finder normalen på den forkerte side
            {
                for (int j = 0; j < noOfCoords; j++)
                {
                    wallNormals[i, j] = -1 * wallNormals[i, j];
                    dotNormalPointvector[i] = -1 * dotNormalPointvector[i]; // hvorfor gør vi det her?
                }
            }
        }
        Projection projections = new Projection(wallNormals, pointVectors);
        projection = projections.GetProjections();
        wallsReflectedOn = new string[noOfWalls];
        for (int i = 0; i < noOfWalls; i++)
        {
            for (int j = 0; j < noOfCoords; j++)
            {
                imageSources[i, j] = point[j] - 2 * projection[i, j];
            }
            wallsReflectedOn[i] = i.ToString(); 
        }
    }
    public Tuple<double[,],double[,], string[]> GetFirstImageSourcesAndProj() {
        return Tuple.Create(imageSources, projection, wallsReflectedOn);
    }
}












