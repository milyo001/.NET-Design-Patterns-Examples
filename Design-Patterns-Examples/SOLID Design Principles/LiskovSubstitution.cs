namespace Design-Patterns-Examples.SOLID Design Principles
{
    using static System.Console;

    // Using a Classic example
    // THE COMMENTED CODE IS WITHOUT LISKOV and the solution is now working as expected
    public class Rectangle
{
    // Width and Height are declared without virtual access modifier
    
    //public int Width { get; set; }
    //public int Height { get; set; }

    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public Rectangle()
    {

    }
    
    // A second constructor that takes width and height as parameters
    public Rectangle(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
    }
}

public class Square : Rectangle
{
    // THE COMMENTED CODE IS WITHOUT LISKOV
    // For the example(without Liskov), I used the new keyword to override the base class constructor

    //public new int Width
    //{
    //  set { base.Width = base.Height = value; }
    //}

    //public new int Height
    //{ 
    //  set { base.Width = base.Height = value; }
    //}

    public override int Width // Nasty side effects, used just for the example
    {
        set { base.Width = base.Height = value; }
    }
    {
        set { base.Width = base.Height = value; }
    }

    public override int Height
    {
        set { base.Width = base.Height = value; }
    }
}

public class Demo
{
    // A static function returning the area of Rectangle class or any class inherited from it
    static public int Area(Rectangle r) => r.Width * r.Height;

    static void Main(string[] args)
    {
        // This is working fine because this is the base class
        Rectangle rc = new Rectangle(2, 3);
        WriteLine($"{rc} has area {Area(rc)}");

        // Should be able to substitute a base type for a subtype Square
        // Now if you can see the commented code above you can see that the Width and Height properties are not virtual
        // which means that the Square class can't override the Width and Height properties correctly
        Rectangle sq = new Square();
        sq.Width = 4;
        WriteLine($"{sq} has area {Area(sq)}");
    }
}
}
