using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

class Program
{
    static void Main(string[] args)
    {
        // Define the real source location
        Point3D realSource = new Point3D(5, 5, 5);

        // Define the reflective surfaces
        List<ReflectiveSurface> surfaces = new List<ReflectiveSurface>();
        //surfaces.Add(new ReflectiveSurface(new Point3D(0, 0, 0), new Vector3D(0, 0, 1), new List<Point3D> {
        surfaces.Add(new ReflectiveSurface(new Vector3D(0, 0, 1), new List<Point3D> {
            new Point3D(10, 10, 0),
            new Point3D(-10, 10, 0),
            new Point3D(-10, -10, 0),
            new Point3D(10, -10, 0)
        }));
        //surfaces.Add(new ReflectiveSurface(new Point3D(0, 0, 0), new Vector3D(1, 0, 0), new List<Point3D> {
        surfaces.Add(new ReflectiveSurface(new Vector3D(1, 0, 0), new List<Point3D> {
            new Point3D(0, 10, 10),
            new Point3D(0, -10, 10),
            new Point3D(0, -10, -10),
            new Point3D(0, 10, -10)
        }));

        // Calculate the image sources
        List<Point3D> imageSources = CalculateImageSources(realSource, surfaces);

        // Print the image sources
        Console.WriteLine("Image sources:");
        foreach (Point3D imageSource in imageSources)
        {
            Console.WriteLine($"({imageSource.X}, {imageSource.Y}, {imageSource.Z})");
        }
    }

    static List<Point3D> CalculateImageSources(Point3D realSource, List<ReflectiveSurface> surfaces)
    {
        List<Point3D> imageSources = new List<Point3D>();
        // MANGLER ET LOOP MED ORDENNUMMER
        foreach (ReflectiveSurface surface in surfaces)
        {
            for (int i = 0; i < surface.Vertices.Count; i++)
            {
                // Get the two vertices that define an edge of the surface
                Point3D vertex1 = surface.Vertices[i];
                Point3D vertex2 = surface.Vertices[(i + 1) % surface.Vertices.Count];

                // Calculate the normal vector of the surface
                Vector3D edge = vertex2 - vertex1;
                Console.WriteLine(edge);
                Vector3D normal = Vector3D.CrossProduct(edge, -surface.NormalVector); // NORMALVEKTOREN ER NOK GAL
                normal.Normalize();

                // Calculate the distance between the real source and the surface
                Vector3D s = realSource - vertex1; //AFSTANDEN BLIVER NOK FORKERT HVIS SKRÅ; BØR REGNE TIL LINJEN
                double dot = Vector3D.DotProduct(s, normal); 
                if (dot >= 0)
                {
                    // Calculate the mirror image source
                    Point3D imageSource = realSource - 2 * dot * normal;
                    imageSources.Add(imageSource);
                }
            }
        }

        return imageSources;
    }
}

class ReflectiveSurface
{
    //public Point3D Location { get; set; }
    public Vector3D NormalVector { get; set; }
    public List<Point3D> Vertices { get; set; }

    //public ReflectiveSurface(Point3D location, Vector3D normalVector, List<Point3D> vertices)
    public ReflectiveSurface(Vector3D normalVector, List<Point3D> vertices)
    {
        //Location = location;
        NormalVector = normalVector;
        Vertices = vertices;
    }
}
