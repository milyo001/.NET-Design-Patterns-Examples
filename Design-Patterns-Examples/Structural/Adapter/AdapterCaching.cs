using System.Collections;
using System.Collections.ObjectModel;
using MoreLinq;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Adapter.WithCaching
{
    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        // Get the hash code 
        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public class Line
    {
        public Point Start;
        public Point End;

        public Line(Point start, Point end)
        {
            this.Start = start;
            this.End = end;
        }

        // Get the hash code of Line
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Start != null ? Start.GetHashCode() : 0) * 397) ^ (End != null ? End.GetHashCode() : 0);
            }
        }
    }

    public abstract class VectorObject : Collection<Line>
    {
    }

    public class VectorRectangle : VectorObject
    {
        public VectorRectangle(int x, int y, int width, int height)
        {
            Add(new Line(new Point(x, y), new Point(x + width, y)));
            Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
            Add(new Line(new Point(x, y), new Point(x, y + height)));
            Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
        }
    }

    // In the previuous example(AdapterNoCaching) we used to inherit the class from Collection<Point>, in this example we are going to 
    // use IEnumerable<Point>
    public class LineToPointAdapter : IEnumerable<Point>
    {
        private static int count = 0;
        // This will be the cache, as a key we need to store hashcode (The lines are composed of points and the
        // points are composed // of X and Y coordinates, which are all unique which means it is unlikely to get hash collisions)
        static Dictionary<int, List<Point>> cache = new Dictionary<int, List<Point>>();
        
        // The actual hash code used as a key for the cache
        private int hash;

        public LineToPointAdapter(Line line)
        {
            // Set the hash field
            hash = line.GetHashCode();
            
            // If the hash ket is in the cach we simply return and exit the program
            if (cache.ContainsKey(hash)) return;

            WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}] (with caching)");
            //                                                 ^^^^

            List<Point> points = new List<Point>();
            
            // Then perfom the same calculations as in AdapterNoCaching example
            int left = Math.Min(line.Start.X, line.End.X);
            int right = Math.Max(line.Start.X, line.End.X);
            int top = Math.Min(line.Start.Y, line.End.Y);
            int bottom = Math.Max(line.Start.Y, line.End.Y);
            int dx = right - left;
            int dy = line.End.Y - line.Start.Y;

            if (dx == 0)
            {
                for (int y = top; y <= bottom; ++y)
                {
                    // Note we are adding the points to the list, unlike the AdapterNoCaching example
                    points.Add(new Point(left, y));
                }
            }
            else if (dy == 0)
            {
                for (int x = left; x <= right; ++x)
                {
                    // Here too
                    points.Add(new Point(x, top));
                }
            }

            // And finally add the points to the cache
            cache.Add(hash, points);
        }

        // Inherited
        public IEnumerator<Point> GetEnumerator()
        {
            // We already have the points stored in the cache, from here we can access them
            return cache[hash].GetEnumerator();
        }

        // Inherited
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public class Demo
    {
        private static readonly List<VectorObject> vectorObjects = new List<VectorObject>
    {
      new VectorRectangle(1, 1, 10, 10),
      new VectorRectangle(3, 3, 6, 6)
    };

        public static void DrawPoint(Point p)
        {
            Write(".");
        }

        static void Main(string[] args)
        {
            Draw();

            // Won't do anything because the points and lines are already stored in the cashe
            Draw();
        }

        private static void Draw()
        {
            foreach (var vo in vectorObjects)
            {
                foreach (var line in vo)
                {
                    var adapter = new LineToPointAdapter(line);
                    // Unlike the previous example we've printed 16 result, now we have only 8
                    adapter.ForEach(DrawPoint);
                }
            }
        }
    }
}