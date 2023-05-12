/*public class ThirdImageSources {
    private double[,] imageSources;
    private int noOfCoords;
    private int noOfProjs;
    //private int noOfSovser;
    private int noOfOldSovser;
    private double[,] imageDummy;
    private double[] checkSumSovs;
    private string[] wallsReflectedOn;
    public ThirdImageSources(double[,] secondImages, double[,] projection, double[] point, double[,] firstImages, string[] secondWallReflects) 
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
        wallsReflectedOn = new int[noOfOldSovser*noOfProjs, 2];
        for (int o = 0; o < noOfOldSovser; o++)
        {
            for (int q = 0; q < noOfProjs; q++)
            {
                for (int j = 0; j < noOfCoords; j++)
                {
                    imageDummy[y, j] = secondImages[o, j] - 2 * projection[q, j];
                    //Console.WriteLine(projection[q,j]);
                    checkSumSovs[y] += arbitraryFunction(imageDummy[y, j], j);
                }
                //Console.WriteLine("checkSum = {0}", checkSumSovs[y]);
                if (!imageContainedCheck(checkSumSovs[y], checkSum))
                {
                    sovserAdded++;
                    //Console.WriteLine("Sovser tilføjet: {0}", sovserAdded);
                    for(int j = 0; j < noOfCoords; j++)
                    {
                        imageSources[y, j] = imageDummy[y, j];
                    }
                    checkSum.Add(checkSumSovs[y]);
                    wallsReflectedOn[y] = secondWallReflects[o] + " -> " + q.ToString();
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
        CleanUpImageSources cleanUpSovser = new CleanUpImageSources(imageSources, sovserAdded, wallsReflectedOn);
        imageSources = cleanUpSovser.getCleanImageSources();
        wallsReflectedOn = cleanUpSovser.getWallsReflectedOn();
        Console.WriteLine("Der burde være 36 kilder...");
    }
    public double[,] GetThirdImageSources() {
        return imageSources;
    }
    public string[] GetThirdReflects() {
        return wallsReflectedOn;
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

} */