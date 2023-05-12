using System;

public class Program {
    public double[,] firstImageSources;
    public double[,] secondImageSources;
    public int[,] secondWallReflects;
    public int[,] firstWallReflects;
    public double[,] ISMPositions;
    private int[,] ISMWallReflects;

    public static void Main() {
        double[] point = new double[] {1,2,2};

        Room room = new Room();
        double[,,] wallVertices = room.GetWalls();
        
        WallVectors wallVectors = new WallVectors(wallVertices);
        double[,,] vectors = wallVectors.GetVectors();
        
        WallNormals wallNormals = new WallNormals(vectors);
        double[,] normals = wallNormals.GetNormals();
        
        FirstImageSources firstImageSource = new FirstImageSources(point, normals, wallVertices);
        Tuple<double[,],double[,],int[,]> firstISMResults = firstImageSource.GetFirstImageSourcesAndProj();
        double[,] firstImageSources = firstISMResults.Item1;
        double[,] firstImageProjs = firstISMResults.Item2;
        int[,] firstWallReflects = firstISMResults.Item3;

        Console.WriteLine("The first-order imagesources of the reflective surfaces are:");
        for (int i = 0; i < firstImageSources.GetLength(0); i++) {
            Console.WriteLine("Image {0}: ({1}, {2}, {3})", i + 1, firstImageSources[i, 0], firstImageSources[i, 1], firstImageSources[i,2]);
            Console.WriteLine("Image {0}'s wall IDs: {1} and {2}", i + 1, firstWallReflects[i,0], firstWallReflects[i,1]);
        }

        Console.WriteLine("Her kommer andenordens. aj-aj hr. kaptajn");
        SecondImageSources secondImageSource = new SecondImageSources(firstImageSources, normals, wallVertices, point, firstWallReflects);
        double[,] secondImageSources = secondImageSource.GetSecondImageSources();
        int[,] secondWallReflects = secondImageSource.GetSecondReflects();
        for (int i = 0; i < secondImageSources.GetLength(0); i++) {
            Console.WriteLine("Image {0}: ({1}, {2}, {3})", i + 1 + 6, secondImageSources[i, 0], secondImageSources[i, 1], secondImageSources[i,2]);
            Console.WriteLine("Image {0}'s wall IDs: {1} and {2}", i + 1 + 6, secondWallReflects[i,0], secondWallReflects[i,1]);
        }

        /*
        Console.WriteLine("Her kommer tredjeordens. aj-aj hr. kaptajn");
        ThirdImageSources thirdImageSource = new ThirdImageSources(secondImageSources, firstImageProjs, point, firstImageSources, secondWallReflects);
        double[,] thirdImageSources = thirdImageSource.GetThirdImageSources();
        string[] thirdWallReflects = thirdImageSource.GetThirdReflects();
        for (int i = 0; i < thirdImageSources.GetLength(0); i++) {
            Console.WriteLine("Image {0}: ({1}, {2}, {3})", i + 1, thirdImageSources[i, 0], thirdImageSources[i, 1], thirdImageSources[i,2]);
            Console.WriteLine("Image {0}'s wall IDs: {1}", i + 1, thirdWallReflects[i]);
        }
        */

        Console.WriteLine("Her kommer directivities. aj-aj hr. kaptajn");
        //////ISM LISTE //////
        int y = 0;
        double[,] ISMPositions = new double[firstImageSources.GetLength(0)+secondImageSources.GetLength(0), firstImageSources.GetLength(1)];
        for (int i = 0; i < firstImageSources.GetLength(0); i++)
        {
            for (int j = 0; j < firstImageSources.GetLength(1); j++)
            {
                ISMPositions[y, j] = firstImageSources[i, j];
            }
            y++;
        }
        for (int i = 0; i < secondImageSources.GetLength(0); i++)
        {   
            for (int j = 0; j < secondImageSources.GetLength(1); j++)
            {
                ISMPositions[y, j] = secondImageSources[i, j];
            }
            y++;
        }
        ///////WALLIDS LISTE ///////
        int z = 0;
        int[,] ISMWallReflects = new int[firstWallReflects.GetLength(0)+secondWallReflects.GetLength(0),2];
        for (int i = 0; i < firstWallReflects.GetLength(0); i++)
        {
            ISMWallReflects[z,0] = firstWallReflects[i,0];
            ISMWallReflects[z,1] = firstWallReflects[i,1];
            z++;
        }
        for (int i = 0; i < secondWallReflects.GetLength(0); i++)
        {   
            ISMWallReflects[z,0] = secondWallReflects[i,0];
            ISMWallReflects[z,1] = secondWallReflects[i,1];
            z++;
        }
        double[] recPos = new double[] {2,2,2};
        double[] patternDir = new double[] {1, 0, 0};
        DirectivityV3 directivity = new DirectivityV3(ISMPositions, recPos, ISMWallReflects, patternDir);
    }

/*
    public double[,] GetISMs() {
        int y = 0;
        ISMPositions = new double[firstImageSources.GetLength(0)+secondImageSources.GetLength(0), firstImageSources.GetLength(1)];
        for (int i = 0; i < firstImageSources.GetLength(0); i++)
        {
            for (int j = 0; j < firstImageSources.GetLength(1); j++)
            {
                ISMPositions[y, j] = firstImageSources[i, j];
            }
            y++;
        }
        for (int i = 0; i < secondImageSources.GetLength(0); i++)
        {   
            for (int j = 0; j < secondImageSources.GetLength(1); j++)
            {
                ISMPositions[y, j] = firstImageSources[i, j];
            }
            y++;
        }
        return ISMPositions;
    }

    public double[] GetISMWallReflects()
    {
        int y = 0;
        ISMWallReflects = new double[firstWallReflects.GetLength(0)+secondWallReflects.GetLength(0)];
        for (int i = 0; i < firstWallReflects.GetLength(0); i++)
        {
            ISMWallReflects[y] = Convert.ToDouble(firstWallReflects[i]);
            y++;
        }
        for (int i = 0; i < secondWallReflects.GetLength(0); i++)
        {   
            ISMWallReflects[y] = Convert.ToDouble(secondWallReflects[i]);
            y++;
        }
        return ISMWallReflects;
    }*/
}



