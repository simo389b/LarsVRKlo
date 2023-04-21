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
    private string[] wallsReflectedOn;
    public SecondImageSources(double[,] firstImages, double[,] wallNormals, double[,,] wallVertices, double[] point, string[] firstWallReflects) 
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
        wallsReflectedOn = new string[noOfOldSovser*noOfProjs];

        ////////////// HER BEGYNDER SELVE ISM
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
                if (!imageContainedCheck(checkSumSovs[y], checkSum))
                {
                    sovserAdded++;
                    //Console.WriteLine("Sovser tilføjet: {0}", sovserAdded);
                    for(int j = 0; j < noOfCoords; j++)
                    {
                        imageSources[y, j] = Math.Round(imageDummy[y, j],3);
                    }
                    checkSum.Add(checkSumSovs[y]);
                    wallsReflectedOn[y] = o.ToString() + " -> " + q.ToString();
                }
                else
                {
                    //Console.WriteLine("IMAGE SOURCE ALREADY CONTAINED!!");
                    for(int j = 0; j < noOfCoords; j++)
                    {
                        imageSources[y, j] = Double.NaN;
                    }
                }
                //Console.WriteLine(wallsReflectedOn[y]);
                y++;
            }  
        } // FOR-LOOP MED noOfOldSovser
        CleanUpImageSources cleanUpSovser = new CleanUpImageSources(imageSources, sovserAdded, wallsReflectedOn);
        imageSources = cleanUpSovser.getCleanImageSources();
        wallsReflectedOn = cleanUpSovser.getWallsReflectedOn();
    } 

    public double[,] GetSecondImageSources() {
        return imageSources;
    }

    public string[] GetSecondReflects() {
        return wallsReflectedOn;
    }
    public bool imageContainedCheck(double checkSumSovs, List<double> checkSum){ // Det her behøver ikke være en funktion for sig selv, men er et levn
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