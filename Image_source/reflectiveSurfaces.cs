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
        
        ImageSources imageSource = new ImageSources(point, normals, wallVertices);
        double[,] imageSources = imageSource.GetImageSources();

        Console.WriteLine("The imagesources of the reflective surfaces are:");
        for (int i = 0; i < imageSources.GetLength(0); i++) {
            Console.WriteLine("Image {0}: ({1}, {2}, {3})", i + 1, imageSources[i, 0], imageSources[i, 1], imageSources[i,2]);
        }
    }
}



