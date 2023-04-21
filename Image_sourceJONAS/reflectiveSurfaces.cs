using System;

public class Program {
    public static void Main() {
        double[] point = new double[] {2,2,2};

        Room room = new Room();
        double[,,] wallVertices = room.GetWalls();
        
        WallVectors wallVectors = new WallVectors(wallVertices);
        double[,,] vectors = wallVectors.GetVectors();
        
        WallNormals wallNormals = new WallNormals(vectors);
        double[,] normals = wallNormals.GetNormals();
        
        FirstImageSources firstImageSource = new FirstImageSources(point, normals, wallVertices);
        Tuple<double[,],double[,],string[]> firstISMResults = firstImageSource.GetFirstImageSourcesAndProj();
        double[,] firstImageSources = firstISMResults.Item1;
        double[,] firstImageProjs = firstISMResults.Item2;
        string[] firstWallReflects = firstISMResults.Item3;

        Console.WriteLine("The first-order imagesources of the reflective surfaces are:");
        for (int i = 0; i < firstImageSources.GetLength(0); i++) {
            Console.WriteLine("Image {0}: ({1}, {2}, {3})", i + 1, firstImageSources[i, 0], firstImageSources[i, 1], firstImageSources[i,2]);
            Console.WriteLine("Image {0}'s wall IDs: {1}", i + 1, firstWallReflects[i]);
        }

        Console.WriteLine("Her kommer andenordens. aj-aj hr. kaptajn");
        SecondImageSources secondImageSource = new SecondImageSources(firstImageSources, normals, wallVertices, point, firstWallReflects);
        double[,] secondImageSources = secondImageSource.GetSecondImageSources();
        string[] secondWallReflects = secondImageSource.GetSecondReflects();
        for (int i = 0; i < secondImageSources.GetLength(0); i++) {
            Console.WriteLine("Image {0}: ({1}, {2}, {3})", i + 1, secondImageSources[i, 0], secondImageSources[i, 1], secondImageSources[i,2]);
            Console.WriteLine("Image {0}'s wall IDs: {1}", i + 1, secondWallReflects[i]);
        }

        /*
        Console.WriteLine("Her kommer tredjeordens. aj-aj hr. kaptajn");
        ThirdImageSources thirdImageSource = new ThirdImageSources(secondImageSources, firstImageProjs, point, firstImageSources);
        double[,] thirdImageSources = thirdImageSource.GetThirdImageSources();
        for (int i = 0; i < thirdImageSources.GetLength(0); i++) {
            Console.WriteLine("Image {0}: ({1}, {2}, {3})", i + 1, thirdImageSources[i, 0], thirdImageSources[i, 1], thirdImageSources[i,2]);
        }
        */
    }
}



