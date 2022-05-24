
// This is a builder made with more functional way ( using functons )
namespace DotNetDesignPatternDemos.Creational.Builder
{
    // A person object is built using this builder
    public class Person
    {
        public string Name, Position;
    }

    // Notice that this class is sealed, so you cannot inherit from it.
    // If you want to extend it in the future, you need to do it not with inheritence
    // but with extenition method (using "this" keyword)
    public sealed class PersonBuilder
    {
        // Preseve list of functions
        public readonly List<Action<Person>> Actions
          = new List<Action<Person>>();

        public PersonBuilder Called(string name)
        {
            // Add a function to the list of functions
            Actions.Add(p => { p.Name = name; });
            // Return the builder itself, so that we can chain the calls in Main method
            return this;
        }

        public Person Build()
        {
            // Initialize the person object
            var p = new Person();
            // Loop through all the functions in the list
            Actions.ForEach(a => a(p));
            // And finally return the person object
            return p;
        }
    }

    // This will extend the PersonBuilder, since it is sealed (cannot be inherited)
    public static class PersonBuilderExtensions
    {
        // This is an extension method, notice the "this" keyword
        public static PersonBuilder WorksAsA
          (this PersonBuilder builder, string position)
        {
            builder.Actions.Add(p =>
            {
                p.Position = position;
            });
            return builder;
        }

    }

    public class FunctionalBuilder
    {
        public static void Main(string[] args)
        {
            var pb = new PersonBuilder();
            var person = pb
                .Called("Miro")
                .WorksAsA("Programmer")
                .Build();
        }
    }
}