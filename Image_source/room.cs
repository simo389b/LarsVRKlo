public class Room 
{
    private double[,,] wallVertices;
    public Room(double[,] centerPoints, double[,] dimensions, double[,] rotations)
    {
        wallVertices = new double[centerPoints.GetLength(0), centerPoints.GetLength(1), centerPoints.GetLength(1)];
        for (int i = 0; i < centerPoints.GetLength(0); i++)
        {   
            for (int j = 0; j < centerPoints.GetLength(1); j++)
            {
                wallVertices[i, 0, j] = centerPoints[i, j] - 0.5 * dimensions[i, 0];
                wallVertices[i, 1, j] = centerPoints[i, j] + 0.5 * dimensions[i, 1];
                wallVertices[i, 2, j] = centerPoints[i, j] + 0.5 * dimensions[i, 1] - 0.5 * dimensions[i, 0];
            }
        }
    }
    
    public double[,,] GetWalls() {
        return wallVertices;
    }
}


public class WallVectors 
{
    private double[,,] vectors;
    
    public WallVectors(double[,,] walls) {
        vectors = new double[walls.GetLength(0), 2, walls.GetLength(2)];
        for (int i = 0; i < walls.GetLength(0); i++) 
        {
            for (int j = 0; j < walls.GetLength(2); j++)
            {
                vectors[i, 0, j] = walls[i, 1, j] - walls[i, 0, j];
                vectors[i, 1, j] = walls[i, 2, j] - walls[i, 0, j];
            }
        }
    }
    
    public double[,,] GetVectors() {
        return vectors;
    }
}

public class WallNormals {
    private double[,] normals;
    
    public WallNormals(double[,,] vectors) {
        normals = new double[vectors.GetLength(0), vectors.GetLength(2)];
        for (int i = 0; i < vectors.GetLength(0); i++) {
            normals[i, 0] = vectors[i, 0, 1] * vectors[i, 1, 2] - vectors[i, 0, 2] * vectors[i, 1, 1];
            normals[i, 1] = vectors[i, 0, 2] * vectors[i, 1, 0] - vectors[i, 0, 0] * vectors[i, 1, 2];
            normals[i, 2] = vectors[i, 0, 0] * vectors[i, 1, 1] - vectors[i, 0, 1] * vectors[i, 1, 0];

        }
    }
    public double[,] GetNormals() {
        return normals;
    }
}


public class CheckWallValidity
{
    private double[,] cornerPointVector;
    private double[] dotValidity;
    public CheckWallValidity(double[,] normals, double[,,] walls)
    {
        cornerPointVector = new double[normals.GetLength(0), normals.GetLength(1)];
        dotValidity = new double[normals.GetLength(0)];
        for (int i = 0; i < normals.GetLength(0); i++)
        {
            for (int j = 0; j < normals.GetLength(1); j++)
            {
                cornerPointVector[i, j] = walls[i, 3, j] - walls[i, 0, j];
            }
            dotValidity[i] =    cornerPointVector[i, 0] * normals[i, 0] +
                                cornerPointVector[i, 1] * normals[i, 1] +
                                cornerPointVector[i, 2] * normals[i, 2];
            if (dotValidity[i] != 0 )
            {
                Console.WriteLine("Wall {0} is invalid", i+1);
                return;
            }
        }
        
    }
}