using System;
using System.Collections.Generic;
using System.Numerics;

public class ImageSourceGenerator
{
    private static List<Vector4> walls = new List<Vector4>() {
        new Vector4(0, 0, 1, 1),   // wall 1 (z = 0)
        new Vector4(0, 0, -1, 1),  // wall 2 (z = 0)
        new Vector4(1, 0, 0, 1),   // wall 3 (x = 0)
        new Vector4(-1, 0, 0, 1)   // wall 4 (x = 0)
    };

    public static List<Vector3> GenerateImageSources(int maxOrder, int numWalls, Vector3 src)
    {
        List<Vector3> imageSources = new List<Vector3>();
        imageSources.Add(src); // Add the original source as the first image source

        // Loop through the reflection order
        for (int i = 1; i <= maxOrder; i++)
        {
            int prevNumSources = imageSources.Count;

            // Loop through each previous image source
            for (int j = 0; j < prevNumSources; j++)
            {
                Vector3 source = imageSources[j];

                // Loop through each wall
                for (int k = 0; k < numWalls; k++)
                {
                    Vector4 plane = walls[k];
                    Vector3 normal = new Vector3(plane.X, plane.Y, plane.Z);
                    float distance = plane.W;
                    
                    Vector3 reflected = Vector3.Reflect(source, normal);

                    // Check if the reflected point is already on the plane
                    if (Math.Abs(Vector3.Dot(reflected, normal) + distance) < 1e-6f)
                    {
                        continue;
                    }

                    // Project the reflected point onto the plane
                    float d = distance / Vector3.Dot(normal, reflected);
                    reflected -= d * normal;
                    Console.WriteLine(reflected);

                    // Add the reflected source if it's not already in the list
                    if (!imageSources.Contains(reflected))
                    {
                        imageSources.Add(reflected);
                    }
                }
            }
        }
        //Console.WriteLine(imageSources);
        return imageSources;
    }

    static void Main(string[] args)
    {
        int maxOrder = 3;
        int numWalls = 4;
        Vector3 src = new Vector3(50, 50, 50);
        List<Vector3> imageSources = GenerateImageSources(maxOrder, numWalls, src);

        Console.WriteLine("Image sources:");
        foreach (Vector3 source in imageSources)
        {
            Console.WriteLine("(" + source.X + ", " + source.Y + ", " + source.Z + ")");
        }
    }
}
