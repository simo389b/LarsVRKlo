public class ImageSources {
    private double[,] pointVectors;
    private double[,] imageSources;
    private double[] dotNormalPointvector;
    public ImageSources(double[] point, double[,] wallNormals, double[,,] wallVertices) 
    {
        pointVectors = new double[wallNormals.GetLength(0), wallNormals.GetLength(1)];
        imageSources = new double[wallNormals.GetLength(0), wallNormals.GetLength(1)];
        dotNormalPointvector = new double[wallNormals.GetLength(0)];
        
        for (int i = 0; i < wallNormals.GetLength(0); i++)
        {
            for (int j = 0; j < wallNormals.GetLength(1); j++)
            {
                pointVectors[i, j] = point[j] - wallVertices[i, 0, j]; 
            }
            dotNormalPointvector[i] = pointVectors[i, 0] * wallNormals[i, 0] +
                                      pointVectors[i, 1] * wallNormals[i, 1] + 
                                      pointVectors[i, 2] * wallNormals[i, 2] ;
            if (dotNormalPointvector[i] <= 0)
            {
                for (int j = 0; j < wallNormals.GetLength(1); j++)
                {
                    wallNormals[i, j] = -1 * wallNormals[i, j];
                    dotNormalPointvector[i] = -1 * dotNormalPointvector[i];
                }
            }
        }
        Projection projections = new Projection(wallNormals, pointVectors);
        double[,] projection = projections.GetProjections();
        for (int i = 0; i < wallNormals.GetLength(0); i++)
        {
            for (int j = 0; j < wallNormals.GetLength(1); j++)
            {
                imageSources[i, j] = point[j] - 2 * projection[i, j] ;
            }
        }
        
    }
    public double[,] GetImageSources() {
        return imageSources;
    }
}

public class Projection
{
    private double[] dotNormVect;
    private double[] dotNormNorm;
    private double[,] projection;
    public Projection(double[,] normals, double[,] vectors)
    {
        dotNormVect = new double[normals.GetLength(0)];
        dotNormNorm = new double[normals.GetLength(0)];
        projection = new double[normals.GetLength(0), normals.GetLength(1)];
        
        Console.WriteLine(vectors.GetLength(0));
        Console.WriteLine(vectors.GetLength(1));

        for(int i = 0; i < normals.GetLength(0); i++)
        {
            dotNormVect[i] =    vectors[i, 0] * normals[i, 0] +
                                vectors[i, 1] * normals[i, 1] + 
                                vectors[i, 2] * normals[i, 2] ;
            dotNormNorm[i] =    normals[i, 0] * normals[i, 0] +
                                normals[i, 1] * normals[i, 1] + 
                                normals[i, 2] * normals[i, 2] ;
            for(int j = 0; j < normals.GetLength(1); j++)
            {
                projection[i, j] = (dotNormVect[i]/dotNormNorm[i])*normals[i, j];
            }
            
        }
        
    }

    public double[,] GetProjections()
    {
        return projection;
    }
}
