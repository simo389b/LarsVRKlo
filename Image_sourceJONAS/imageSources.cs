public class FirstImageSources {
    private double[,] pointVectors;
    private double[,] imageSources;
    private double[] dotNormalPointvector;
    private int noOfWalls;
    private int noOfCoords;
    private double[,] projection;
    
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
        for (int i = 0; i < noOfWalls; i++)
        {
            for (int j = 0; j < noOfCoords; j++)
            {
                imageSources[i, j] = point[j] - 2 * projection[i, j] ;
            }
        }
    }
    public Tuple<double[,],double[,]> GetFirstImageSourcesAndProj() {
        return Tuple.Create(imageSources, projection);
    }
}



public class SecondImageSources {
    private double[,] imageSources;
    private int noOfCoords;
    private int noOfProjs;
    private int noOfOldSovser;
    private double[,] imageDummy;
    private double[] checkSumSovs;
    private double[] dotNormalPointvector;
    private double[,] pointVectors;
    private double[,] projection;
    public SecondImageSources(double[,] firstImages, double[,] wallNormals, double[,,] wallVertices, double[] point) 
    {
        noOfCoords = firstImages.GetLength(1); //X, Y, Z
        noOfProjs = wallNormals.GetLength(0); //Antal normaler, vi har
        noOfOldSovser = firstImages.GetLength(0);

        List<double> checkSum = new List<double>();
        checkSumSovs = new double[noOfOldSovser*noOfProjs]; 

        for (int j = 0; j < noOfCoords; j++) // lægger OG kilde ind
        {
            checkSumSovs[0] += arbitraryFunction(point[j], j);
        }
        checkSum.Add(checkSumSovs[0]);
        //Console.WriteLine("PUNKTS CHECKSUM: {0}", checkSumSovs[0]);

        checkSumSovs = new double[noOfOldSovser*noOfProjs]; // nulstil

        for (int o = 0; o < noOfOldSovser; o++) // Tilføj gamle kilder
        {
            for (int j = 0; j < noOfCoords; j++)
            {
                checkSumSovs[o] += arbitraryFunction(firstImages[o, j], j);
            }
            //Console.WriteLine("GAMLE CHECKSUMS: {0}", checkSumSovs[o]);
            checkSum.Add(checkSumSovs[o]);
        }
        checkSumSovs = new double[noOfOldSovser*noOfProjs]; // nulstil

        imageSources = new double[noOfOldSovser*noOfProjs, noOfCoords]; // Den skal evaluere 36 
        imageDummy = new double[noOfOldSovser*noOfProjs, noOfCoords]; // Holder kilderne, før der bliver tjekket, om de allerede er i listen

        int y = 0; // indeksering
        int sovserAdded = 0;
        for (int o = 0; o < noOfOldSovser; o++)
        {
            //Nulstiller disse for hver sovs
            pointVectors = new double[noOfProjs, noOfCoords];
            dotNormalPointvector = new double[noOfProjs];

            for (int q = 0; q < noOfProjs; q++)
            {
                for (int j = 0; j < noOfCoords; j++)
                {
                    pointVectors[q, j] = firstImages[o, j] - wallVertices[q, 0, j]; 
                }
                dotNormalPointvector[q] = pointVectors[q, 0] * wallNormals[q, 0] +
                                      pointVectors[q, 1] * wallNormals[q, 1] + 
                                      pointVectors[q, 2] * wallNormals[q, 2] ;
                if (dotNormalPointvector[q] <= 0) //Flipper fortegnet, hvis vi finder normalen på den forkerte side
                {
                    for (int j = 0; j < noOfCoords; j++)
                    {
                        wallNormals[q, j] *= -1;
                        //dotNormalPointvector[q] *= -1; // hvorfor gør vi det her?
                    }
                }
            } 

            Projection projections = new Projection(wallNormals, pointVectors);
            projection = projections.GetProjections();

            ///////////////////////////////// STADIG INDE I noOfOldSovser-LOOPET!!!!

            for (int q = 0; q < noOfProjs; q++)
            {
                for (int j = 0; j < noOfCoords; j++)
                {
                    imageDummy[y, j] = firstImages[o, j] - 2 * projection[q, j];
                    checkSumSovs[y] += arbitraryFunction(imageDummy[y, j], j);
                }
                //Console.WriteLine("checkSum = {0}", checkSumSovs[y]);
                if (!imageContainedCheck(checkSumSovs[y], checkSum))
                {
                    sovserAdded++;
                    //Console.WriteLine("Sovser tilføjet: {0}", sovserAdded);
                    for(int j = 0; j < noOfCoords; j++)
                    {
                        imageSources[y, j] = Math.Round(imageDummy[y, j],3);
                    }
                    checkSum.Add(checkSumSovs[y]);
                    //Console.WriteLine("IMAGE SOURCE ADDED!!");
                }
                else
                {
                    //Console.WriteLine("IMAGE SOURCE ALREADY CONTAINED!!");
                    for(int j = 0; j < noOfCoords; j++)
                    {
                        imageSources[y, j] = Double.NaN;
                    }
                }
                y++;
                //Console.WriteLine(y);
            }  
        } // FOR-LOOP MED noOfOldSovser
        CleanUpImageSources cleanUpSovser = new CleanUpImageSources(imageSources, sovserAdded);
        imageSources = cleanUpSovser.getCleanImageSources();
    } 

    public double[,] GetSecondImageSources() {
        return imageSources;
    }
    public bool imageContainedCheck(double checkSumSovs, List<double> checkSum){ // Det her behøver ikke være en funktion fra sig selv, men er et levn
        if(checkSum.Contains(checkSumSovs))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public double arbitraryFunction(double funken, int iteratorJ)
    {
        return (funken*(iteratorJ+1)*181 + funken*(iteratorJ+2)*389 + funken*(iteratorJ+3)*137); // primtal
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
                //Console.WriteLine("Projektion, hallo {0}", projection[i, j]);
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
    public CleanUpImageSources(double[,] imageSovsUren, int supposedSize)
    {   
        imageSovsRen = new double[supposedSize, imageSovsUren.GetLength(1)];
        sovsIndex = 0;
        for (int i = 0; i < imageSovsUren.GetLength(0); i++)
        {
            if(!Double.IsNaN(imageSovsUren[i,0]))
            {
                for (int j = 0; j < imageSovsUren.GetLength(1); j++)
                {
                    imageSovsRen[sovsIndex,j] = imageSovsUren[i,j];
                }
                sovsIndex++;
            }
        
        }
        
    }
    public double[,] getCleanImageSources()
    {
        return imageSovsRen;
    }
}




public class ThirdImageSources {
    private double[,] imageSources;
    private int noOfCoords;
    private int noOfProjs;
    //private int noOfSovser;
    private int noOfOldSovser;
    private double[,] imageDummy;
    private double[] checkSumSovs;
    public ThirdImageSources(double[,] secondImages, double[,] projection, double[] point, double[,] firstImages) 
    {
        noOfCoords = secondImages.GetLength(1); //X, Y, Z
        noOfProjs = projection.GetLength(0); //Antal normaler, vi har
        noOfOldSovser = secondImages.GetLength(0);
        imageSources = new double[noOfOldSovser*noOfProjs, noOfCoords]; 
        imageDummy = new double[noOfOldSovser*noOfProjs, noOfCoords]; // Holder kilderne, før der bliver tjekket, om de allerede er i listen
        checkSumSovs = new double[noOfProjs];

        List<double> checkSum = new List<double>();
        
        //// TILFØJ GAMLE KILDER TIL CHECKLISTE
        checkSumSovs = new double[noOfOldSovser*noOfProjs]; 
        for (int j = 0; j < noOfCoords; j++) // lægger OG kilde ind
        {
            checkSumSovs[0] += arbitraryFunction(point[j], j);
        }
        checkSum.Add(checkSumSovs[0]);
        //Console.WriteLine("PUNKTS CHECKSUM: {0}", checkSumSovs[0]);
        checkSumSovs = new double[noOfOldSovser*noOfProjs]; // nulstil
        for (int o = 0; o < noOfOldSovser; o++) // Tilføj gamle andenordenskilder!!
        {
            for (int j = 0; j < noOfCoords; j++)
            {
                checkSumSovs[o] += arbitraryFunction(secondImages[o, j], j);
            }
            checkSum.Add(checkSumSovs[o]);
        }

        checkSumSovs = new double[noOfOldSovser*noOfProjs]; // nulstil
        for (int o = 0; o < firstImages.GetLength(0); o++) // Tilføj gamle førsteordenskilder!!
        {
            for (int j = 0; j < noOfCoords; j++)
            {
                checkSumSovs[o] += arbitraryFunction(firstImages[o, j], j);
            }
            checkSum.Add(checkSumSovs[o]);
        }
        // FÆRDIG MED AT TILFØJE GAMLE KILDER
        int y = 0; //indeksering
        checkSumSovs = new double[noOfOldSovser*noOfProjs]; //nustil for hvert loop
        int sovserAdded = 0;
        for (int o = 0; o < noOfOldSovser; o++)
        {
            for (int q = 0; q < noOfProjs; q++)
            {
                for (int j = 0; j < noOfCoords; j++)
                {
                    imageDummy[y, j] = secondImages[o, j] - 2 * projection[q, j];
                    Console.WriteLine(projection[q,j]);
                    checkSumSovs[y] += arbitraryFunction(imageDummy[y, j], j);
                }
                //Console.WriteLine("checkSum = {0}", checkSumSovs[y]);
                if (!imageContainedCheck(checkSumSovs[y], checkSum))
                {
                    sovserAdded++;
                    Console.WriteLine("Sovser tilføjet: {0}", sovserAdded);
                    for(int j = 0; j < noOfCoords; j++)
                    {
                        imageSources[y, j] = imageDummy[y, j];
                    }
                    checkSum.Add(checkSumSovs[y]);
                    //Console.WriteLine("IMAGE SOURCE ADDED!!");
                }
                else
                {
                    //Console.WriteLine("IMAGE SOURCE ALREADY CONTAINED!!");
                    for(int j = 0; j < noOfCoords; j++)
                    {
                        imageSources[y, j] = Double.NaN;
                    }
                }
                y++;
                //Console.WriteLine(y);
            }  
        } 
        CleanUpImageSources cleanUpSovser = new CleanUpImageSources(imageSources, sovserAdded);
        imageSources = cleanUpSovser.getCleanImageSources();
        //Console.WriteLine("Der er tilføjet antal checksums: {0}",checkSum.Count);
    }
    public double[,] GetThirdImageSources() {
        return imageSources;
    }
    public bool imageContainedCheck(double checkSumSovs, List<double> checkSum){
        if(checkSum.Contains(checkSumSovs))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public double arbitraryFunction(double funken, int iteratorJ)
    {
        return (funken*(iteratorJ+1)*181 + funken*(iteratorJ+2)*389 + funken*(iteratorJ+3)*137); // primtal
    }

}
