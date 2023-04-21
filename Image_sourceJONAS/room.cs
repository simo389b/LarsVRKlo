public class Room {
    private double[,,] wallVertices = new double[,,]  
            {{ {0, 0, 0}, {5, 0, 0}, {5, 0, 5}, {0, 0, 5} },    //Write the vertices of each wall of the room.     
            { {0, 0, 0}, {0, 5, 0}, {0, 5, 5}, {0, 0, 5} },    //Write the vertices of each wall of the room.     
            { {0, 5, 0}, {0, 5, 5}, {5, 5, 5}, {5, 5, 0} },    //Write the vertices of each wall of the room.     
            { {5, 0, 0}, {5, 0, 5}, {5, 5, 5}, {5, 5, 0} },    //Write the vertices of each wall of the room.     
            { {0, 0, 0}, {5, 0, 0}, {5, 5, 0}, {0, 5, 0} },    //Write the vertices of each wall of the room.     
            { {0, 0, 5}, {0, 5, 5}, {5, 5, 5}, {5, 0, 5} }};   //Write the vertices of each wall of the room.
    
    public double[,,] GetWalls() {
        return wallVertices;
    }
}


public class WallVectors {
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

/*
public class CheckWallValidity
{
    public CheckWallValidity
}*/