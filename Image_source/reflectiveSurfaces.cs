using System;

public class Program {
    public static void Main() {
        double[] point = new double[] {2,2,2}; //Kommer XYZ
        //følgende kommer XZY
        double[,] wallCenterPoints = new double[,] {{3, 3, 0}, {0, 3, 3}, {3, 6, 3},    //Dette er midlertidigt 
                                                    {6, 3, 3}, {3, 3, 6}, {3, 0, 3}};   //og skal læses fra Unity                                                                                
        double[,] wallDimensions = new double[,]   {{6, 6, 0}, {0, 6, 6}, {6, 0, 6},    //Dette er midlertidigt 
                                                    {0, 6, 6}, {6, 6, 0}, {6, 0, 6}};   //og skal læses fra Unity
        double[,] rollPitchYaw = new double[,]     {{0, 90,0}, {90, 0,0}, {0, 0, 0},
                                                    {90, 0,0}, {0, 90,0}, {0, 0, 0}};

        Room room = new Room();
        double[,,] wallVertices = room.GetWalls();
        
        WallVectors wallVectors = new WallVectors(wallVertices);
        double[,,] vectors = wallVectors.GetVectors();
        
        WallNormals wallNormals = new WallNormals(vectors);
        double[,] normals = wallNormals.GetNormals();

        CheckWallValidity valid = new CheckWallValidity(normals, wallVertices);
        
        ImageSources imageSource = new ImageSources(point, normals, wallVertices);
        double[,] imageSources = imageSource.GetImageSources();

        Console.WriteLine("The imagesources of the reflective surfaces are:");
        for (int i = 0; i < imageSources.GetLength(0); i++) {
            Console.WriteLine("Image {0}: ({1}, {2}, {3})", i + 1, imageSources[i, 0], imageSources[i, 1], imageSources[i,2]);
        }
    }
}



