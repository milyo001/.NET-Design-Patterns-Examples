using System;
using System.Collections.Generic;

// This is a builder made with more functional way ( using functons )
namespace DotNetDesignPatternDemos.Creational.Builder
{
    public class Person
    {
        public string Name, Position;
    }

    // Notice that this class is sealed, so you cannot inherit from it.
    // If you want to extend it in the future,
    // you need to do it not with inheritence but with extenition method (using this keyword)
    public sealed class PersonBuilder
    {
        public readonly List<Action<Person>> Actions
          = new List<Action<Person>>();

        public PersonBuilder Called(string name)
        {
            Actions.Add(p => { p.Name = name; });
            return this;
        }

        public Person Build()
        {
            var p = new Person();
            Actions.ForEach(a => a(p));
            return p;
        }
    }

    public static class PersonBuilderExtensions
    {
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
            var person = pb.Called("Miro").WorksAsA("Programmer").Build();
        }
    }
}