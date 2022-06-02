using System.Text;
using static System.Console;

// We are going to emulate drawing applicaiton in this example

namespace DotNetDesignPatternDemos.Structural.Composite.GeometricShapes
{
    // Let's have a base class GraphicObject
    public class GraphicObject
    {
        // Virtual property, so we can override it
        public virtual string Name { get; set; } = "Group";
        
        public string Color;
        
        
        private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
        
        // Public field to expose the field (only instanciting the list of children when needed)
        public List<GraphicObject> Children => children.Value;

        private void Print(StringBuilder sb, int depth)
        {
            sb.Append(new string('*', depth))
                // Then append the color, before that check if this.Color is null or '' 
              .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color} ")
                // And the name, name is with default value of Group so there is not need of validation of the property
              .AppendLine($"{Name}");

            // Call the same method recursively but increment the depth
            foreach (var child in Children)
                child.Print(sb, depth + 1);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb, 0);
            return sb.ToString();
        }
    }

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    public class Square : GraphicObject
    {
        public override string Name => "Square";
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var drawing = new GraphicObject { Name = "My Drawing" };
            
            drawing.Children.Add(new Square { Color = "Red" });
            drawing.Children.Add(new Circle { Color = "Yellow" });

            // The default name property will be "Group"
            var group = new GraphicObject();
            // Add some sample children
            group.Children.Add(new Circle { Color = "Blue" });
            group.Children.Add(new Square { Color = "Blue" });

            // And then add the children from group GraphicObject to drawing GraphicObject
            drawing.Children.Add(group);

            WriteLine(drawing);
        }
    }
}
