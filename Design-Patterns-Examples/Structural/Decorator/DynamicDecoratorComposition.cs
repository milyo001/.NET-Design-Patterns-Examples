using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Decorator.DynamicDecoratorComposition
{
    public abstract class Shape
    {
        public virtual string AsString() => string.Empty;
    }

    public class Circle : Shape
    {
        private float radius;

        // Empty constructor with default value, used for the static decorator example
        public Circle() : this(0)
        {

        }

        public Circle(float radius)
        {
            this.radius = radius;
        }

        public void Resize(float factor)
        {
            radius *= factor;
        }

        public override string AsString() => $"A circle of radius {radius}";
    }

    public class Square : Shape
    {
        private float side;

        // Empty constructor with default value, used for the static decorator example
        public Square() : this(0)
        {

        }

        public Square(float side)
        {
            this.side = side;
        }

        public override string AsString() => $"A square with side {side}";
    }


    // Dynamic decorator that can attach to the object Shape
    public class ColoredShape : Shape
    {
        private Shape shape;
        private string color;

        public ColoredShape(Shape shape, string color)
        {
            this.shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
            this.color = color ?? throw new ArgumentNullException(paramName: nameof(color));
        }

        public override string AsString() => $"{shape.AsString()} has the color {color}";
    }

    // Another decorator to show the composition of decorators
    public class TransparentShape : Shape
    {
        private Shape shape;
        private float transparency; // Transparancy value

        public TransparentShape(Shape shape, float transparency)
        {
            this.shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
            this.transparency = transparency;
        }

        public override string AsString() => $"{shape.AsString()} has {transparency * 100.0f} transparency";
    }

    
    // Static decorator
    // CRTP cannot be done in C#, which is commonly used in C++
    
    // The line below cannot be done
    //public class ColoredShape2<T> : T where T : Shape { }
    
    public class ColoredShape<T> : Shape where T : Shape, new()
    {
        private string color;

        // new T() is required from the condition (the new() requirment when inheriting from Shape)
        private T shape = new T();

        // We need an empty constructor (the new() requirment when inheriting from Shape)
        public ColoredShape() : this("black")
        {

        }

        public ColoredShape(string color) // no constructor forwarding
        {
            this.color = color ?? throw new ArgumentNullException(paramName: nameof(color));
        }

        public override string AsString()
        {
            return $"{shape.AsString()} has the color {color}";
        }
    }

    // A second static decorator
    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        private float transparency;
        private T shape = new T();

        public TransparentShape(float transparency)
        {
            this.transparency = transparency;
        }

        public override string AsString()
        {
            return $"{shape.AsString()} has transparency {transparency * 100.0f}";
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            // ------------- Dynamic decorator composition demo - runs at runtime ----------------

            // Let's init a square 
            var square = new Square(1.23f);
            WriteLine(square.AsString());

            // Then create square with a color "red" as a ColoredShape(the decorator)
            var redSquare = new ColoredShape(square, "red");
            WriteLine(redSquare.AsString());

            // Then create square with a transparency of 0.5 and with color red as a TransparentShape(the second decorator)
            var redHalfTransparentSquare = new TransparentShape(redSquare, 0.5f);
            WriteLine(redHalfTransparentSquare.AsString());

            // ------------- Static decorator demo (not that used in C#, C# doesn't support inheritance from template argument like C++ ) ----------------
            
            ColoredShape<Circle> blueCircle = new ColoredShape<Circle>("blue");
            WriteLine(blueCircle.AsString());

            // This is with the default value of colored shape
            TransparentShape<ColoredShape<Square>> blackHalfSquare = new TransparentShape<ColoredShape<Square>>(0.4f);
            WriteLine(blackHalfSquare.AsString());
        }
    }
}