using System.Collections.ObjectModel;
using MoreLinq;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Adapter.NoCaching
{
    public class Point
    {
        public int X;
        public int Y;

        // Define a constructor to initialize the point
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        // And classic overriden ToString()
        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }
    }

    // Then we have a Line class, which constructor takes two points (start and end)
    public class Line
    {
        public Point Start;
        public Point End;

        public Line(Point start, Point end)
        {
            this.Start = start;
            this.End = end;
        }
    }

    // Let's say we have a vector object which inherit a collection of lines
    public class VectorObject : Collection<Line>
    {

    }

    // And then a VectorRectangle which iherits VectorObject
    public class VectorRectangle : VectorObject
    {
        public VectorRectangle(int x, int y, int width, int height)
        {
            // Since VectorObject inherits from Collection<T>, we can use the Add method lines
            Add(new Line(new Point(x, y), new Point(x + width, y)));
            Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
            Add(new Line(new Point(x, y), new Point(x, y + height)));
            Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
        }
    }

    // The pattern class, we take a Line and we will try to convert it into Point
    public class LineToPointAdapter : Collection<Point>
    {
        private static int count = 0;

        public LineToPointAdapter(Line line)
        {
            WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}] (no caching)");

            // Calculate the left, right, top, bottom etc. margins 
            int left = Math.Min(line.Start.X, line.End.X);
            int right = Math.Max(line.Start.X, line.End.X);
            int top = Math.Min(line.Start.Y, line.End.Y);
            int bottom = Math.Max(line.Start.Y, line.End.Y);
            int dx = right - left;
            int dy = line.End.Y - line.Start.Y;

            // Then some validations to check if is either vertical or horizontal point
            if (dx == 0)
            {
                for (int y = top; y <= bottom; ++y)
                {
                    // Inherited from Collection<Point>
                    Add(new Point(left, y));
                }
            }
            else if (dy == 0)
            {
                for (int x = left; x <= right; ++x)
                {
                    Add(new Point(x, top));
                }
            }
        }
    }

    public class Demo
    {
        
        private static readonly List<VectorObject> vectorObjects = new List<VectorObject>
    {
      new VectorRectangle(1, 1, 10, 10),
      new VectorRectangle(3, 3, 6, 6)
    };

        // the interface we have
        public static void DrawPoint(Point p)
        {
            Write(".");
        }

        static void Main()
        {
            Draw();
            Draw();
        }

        private static void Draw()
        {
            foreach (var vo in vectorObjects)
            {
                // Each 'vo' is collection of lines
                foreach (var line in vo)
                {
                    // Make adapter and feed it with the line
                    var adapter = new LineToPointAdapter(line);
                    // And then print, using MoreLinq addon. And we are going to see a bunch of points and the first
                    // WriteLine() from the LineToPointAdapter. The points are just to see that it is actually converting
                    adapter.ForEach(DrawPoint);
                }
            }
        }
    }
}