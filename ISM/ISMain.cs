using System;
using System.Collections.Generic;
using System.Numerics;

class Wall
{
    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }

    public Wall(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
    }

    public Vector2 Normal()
    {
        Vector2 direction = End - Start;
        Vector2 normal = new Vector2(-direction.Y, direction.X);
        return Vector2.Normalize(normal);
    }
}

class ImageSourceGenerator
{
    private int _roomWidth;
    private int _roomHeight;
    private int _sourcePosX;
    private int _sourcePosY;
    private Wall[] _walls;
    private int _reflectionOrder;

    public ImageSourceGenerator(int roomWidth, int roomHeight, int sourcePosX, int sourcePosY, Wall[] walls, int reflectionOrder)
    {
        _roomWidth = roomWidth;
        _roomHeight = roomHeight;
        _sourcePosX = sourcePosX;
        _sourcePosY = sourcePosY;
        _walls = walls;
        _reflectionOrder = reflectionOrder;
    }

    public string[] GenerateImageSources()
    {
        List<string> imageSources = new List<string>();

        for (int wallIndex = 0; wallIndex < _walls.Length; wallIndex++)
        {
            Wall wall = _walls[wallIndex];
            Vector2 wallNormal = wall.Normal();
            Vector2 sourcePos = new Vector2(_sourcePosX, _sourcePosY);

            for (int i = 0; i < _reflectionOrder; i++)
            {
                float distanceToWall = Vector2.Dot(wallNormal, sourcePos - wall.Start);
                Vector2 reflection = sourcePos - 2 * distanceToWall * wallNormal;
                /*
                if (reflection.X < 0 || reflection.X > _roomWidth || reflection.Y < 0 || reflection.Y > _roomHeight)
                {
                    break;
                }
                */
                sourcePos = reflection;
            }
            Console.WriteLine(sourcePos);
            string imageSource = $"wall{wallIndex}_dist{Vector2.Distance(new Vector2(_sourcePosX, _sourcePosY), sourcePos):F2}";
            imageSources.Add(imageSource);
        }

        return imageSources.ToArray();
    }
}

class Program
{
    static void Main(string[] args)
    {
        int roomWidth = 800;
        int roomHeight = 600;
        int sourcePosX = 400;
        int sourcePosY = 300;

        Wall[] walls = new Wall[4];
        walls[0] = new Wall(new Vector2(0, 0), new Vector2(roomWidth, 0));
        walls[1] = new Wall(new Vector2(roomWidth, 0), new Vector2(roomWidth, roomHeight));
        walls[2] = new Wall(new Vector2(roomWidth, roomHeight), new Vector2(0, roomHeight));
        walls[3] = new Wall(new Vector2(0, roomHeight), new Vector2(0, 0));

        int reflectionOrder = 3;

        ImageSourceGenerator generator = new ImageSourceGenerator(roomWidth, roomHeight, sourcePosX, sourcePosY, walls, reflectionOrder);
        string[] imageSources = generator.GenerateImageSources();

        Console.WriteLine("Generated image sources:");
        foreach (string imageSource in imageSources)
        {
            Console.WriteLine(imageSource);
        }
    }
}
