using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Builder
{
    // About Builder pattern:
    // Builder is a creational design pattern, which allows constructing complex objects step by step.
    // Unlike other creational patterns, Builder doesn’t require products to have a common interface.
    // That makes it possible to produce different products using the same construction process.

    // Let's build HtmlElement class that contains all the information about the element
    class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private const int indentSize = 2;

        public HtmlElement()
        {

        }

        public HtmlElement(string name, string text)
        {
            Name = name;
            Text = text;
        }
        
        private string ToStringImpl(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', indentSize * indent);

            // Firstly Appends the name of the element and goes to the next line
            sb.Append($"{i}<{Name}>\n");

            // Then append the Text property if not null
            if (!string.IsNullOrWhiteSpace(Text))
            {
                // Append the text with some spaces using string constructor(char c, int count)
                sb.Append(new string(' ', indentSize * (indent + 1)));
                // Then append the text
                sb.Append(Text);
                // And finally append new line
                sb.Append("\n");
            }

            // Then loop through the child elements and append them with the indent + 1 (additional white space)
            foreach (var e in Elements)
                sb.Append(e.ToStringImpl(indent + 1));

            // Close the tag of the element and go to the next line
            sb.Append($"{i}</{Name}>\n");
            return sb.ToString();
        }

        // Main funct
        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    class HtmlBuilder
    {
        private readonly string rootName;

        public HtmlBuilder(string rootName)
        {
            this.rootName = rootName;
            root.Name = rootName;
        }

        // This method is not using fluent builder pattern, because it is void. Fluent builder pattern
        // methods return the actual builder instance, so it can be chainable and reused later on.
        public void AddChild(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);
        }

        // Now this is a fluent builder pattern method because it returns the actual HtmlBuilder instance
        public HtmlBuilder AddChildFluent(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);
            return this;
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HtmlElement { Name = rootName };
        }

        // The root element passed from the constructor
        HtmlElement root = new HtmlElement();
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            // If you want to build a simple HTML paragraph using StringBuilder
            var hello = "hello";
            var sb = new StringBuilder();
            sb.Append("<p>");
            sb.Append(hello);
            sb.Append("</p>");
            // This will output <p>hello</p>
            WriteLine(sb);

            // And now I want an HTML list with 2 words in it
            var words = new[] { "hello", "world" };

            // Clear the <p>hello</p> from the code above
            sb.Clear();

            // Append an unordered list HTML tag
            sb.Append("<ul>");

            // Now loop through the words array and append formated string which include <li>{word}</li>
            foreach (var word in words)
            {
                sb.AppendFormat("<li>{0}</li>", word);
            }
            // Lastly close the unordered list and print on the console
            sb.Append("</ul>");
            WriteLine(sb);

            // Now here comes our custom ordinary non-fluent builder
            var builder = new HtmlBuilder("ul");

            // AddChild() is void, so you cannot chain methods, you cannot write
            // builder.AddChild("p", "test").AddChild("p", "test").Clear()
            builder.AddChild("li", "hello");
            builder.AddChild("li", "world");
            WriteLine(builder.ToString());

            // And an example with fluent builder
            sb.Clear();
            builder.Clear(); // Clear the builder 

            // Because AddChildFluent method returns HtmlBuilder instance you can chain methods, see the example below
            builder
                .AddChildFluent("h1", "I am h1 text context!")
                .AddChildFluent("h2", "I am h2 text context!")
                .Clear();

            // And some test data for the example
            builder
                .AddChildFluent("li", "hello")
                .AddChildFluent("li", "world");

            WriteLine(builder);
        }
    }
}
