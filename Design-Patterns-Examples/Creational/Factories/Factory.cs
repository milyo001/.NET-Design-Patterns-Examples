using System;

namespace DotNetDesignPatternDemos.Creational.Factories
{
    
    public class Point
    {
        private double x, y;

        protected Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(double a,
                     double b, // names do not communicate intention
            CoordinateSystem cs = CoordinateSystem.Cartesian)
        {
            switch (cs)
            {
                // Calculate the x and y values based on the coordinate system, this is without the factory methods
                case CoordinateSystem.Polar:
                    x = a * Math.Cos(b);
                    y = a * Math.Sin(b);
                    break;
                default:
                    
                    break;
            }
        }

        // Factory property
        public static Point Origin => new Point(0, 0);

        // Singleton field
        public static Point Origin2 = new Point(0, 0);

        // Example factory methods, return new Point with the default constructor which takes two arguments
        public static Point NewCartesianPoint(double x, double y)
        {
            // Names do communicate intention, x and y are coordinates
            return new Point(x, y);
        }

        // So If we want a NewPolarPoint we can simply call this method
        public static Point NewPolarPoint(double rho, double theta)
        {
            // Names do communicate intention, rho and theta (parameters) are angles in Polar coordinate system
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
        

        // And finally make it lazy
        public static class Factory
        {
            
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }

        public enum CoordinateSystem
        {
            Cartesian,
            Polar
        }
    }

    // Or make a separate class returning points with the default constructor of Point
    class PointFactory
    {
        public static Point NewCartesianPoint(float x, float y)
        {
            return new Point(x, y); // needs to be public
        }
        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
    }

    class Demo
    {
        static void Main(string[] args)
        {
            // This example is without the pattern
            var p1 = new Point(2, 3, Point.CoordinateSystem.Cartesian);
            var origin = Point.Origin;

            // Demonstration of Factory design pattern
            var lazyPoint1 = Point.Factory.NewCartesianPoint(1, 2);
            var lazyPoint2 = Point.Factory.NewPolarPoint(1, 2);

            // Using the separate class PointFactory, which purpose is to return points depending on the coordinate system
            // This class can be easily extended if more coordinate system are available
            var factoredPoint = PointFactory.NewCartesianPoint(2, 3);
            var factoredPoint2 = PointFactory.NewPolarPoint(2, 3);

        }
    }
}
