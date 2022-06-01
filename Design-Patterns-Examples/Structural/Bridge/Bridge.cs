using Autofac;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Bridge
{
    public interface IRenderer
    {
        void RenderCircle(float radius);
    }

    public class VectorRenderer : IRenderer
    {
        // X and y coordinates are missing for the sake of simplicity
        public void RenderCircle(float radius)
        {
            WriteLine($"Drawing a circle of radius {radius}");
        }
    }

    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            WriteLine($"Drawing pixels for circle of radius {radius}");
        }
    }

    // In this abstract class the 'bridging' is happening, you don't inherit anything
    // You don't let Shape class decide how the Shape is drawn
    public abstract class Shape
    {
        protected IRenderer renderer;

        // A bridge between the shape that's being drawn and
        // the component which actually draws it
        public Shape(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        // Abstract methods to be inherited from other classes
        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    // We inherit the bridge abstract class shape and implement the members on our own
    public class Circle : Shape
    {
        private float radius;

        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }

        // Inherited abstract methods from Shape abstract class
        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }

        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            // Without autofac plugin
            
            //var raster = new RasterRenderer();
            //var vector = new VectorRenderer();
            //var circle = new Circle(vector, 5, 5, 5);
            //circle.Draw();
            //circle.Resize(2);
            //circle.Draw();
            
            // Using autofac plugin, check: https://autofac.org/ if not familiar
            var cb = new ContainerBuilder();
            
            cb.RegisterType<VectorRenderer>().As<IRenderer>();
            cb.Register((c, p) => new Circle(c.Resolve<IRenderer>(),
              p.Positional<float>(0)));
            
            using (var c = cb.Build())
            {
                var circle = c.Resolve<Circle>(
                  new PositionalParameter(0, 5.0f)
                );
                // Execute some commands
                circle.Draw();
                circle.Resize(2);
                circle.Draw();
            }
        }
    }
}