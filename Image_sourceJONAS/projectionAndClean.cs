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

public class CleanUpImageSources
{
    double[,] imageSovsRen;
    int sovsIndex;
    string[] wallReflectsRen;
    public CleanUpImageSources(double[,] imageSovsUren, int supposedSize, string[] wallReflectsUren)
    {   
        imageSovsRen = new double[supposedSize, imageSovsUren.GetLength(1)];
        wallReflectsRen = new string[supposedSize];
        sovsIndex = 0;
        for (int i = 0; i < imageSovsUren.GetLength(0); i++)
        {
            if(!Double.IsNaN(imageSovsUren[i,0]))
            {
                for (int j = 0; j < imageSovsUren.GetLength(1); j++)
                {
                    imageSovsRen[sovsIndex,j] = imageSovsUren[i,j];
                }
                wallReflectsRen[sovsIndex] = wallReflectsUren[i];
                sovsIndex++;
            }
        }
    }
    public double[,] getCleanImageSources()
    {
        return imageSovsRen;
    }

    public string[] getWallsReflectedOn()
    {
        return wallReflectsRen;
    }
}