namespace Design-Patterns-Examples.SOLID Design Principles
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using static System.Console;

	// High level modules should not depend on low-level; both should depend on abstractions.
	// Abstractions should not depend on details. Details should depend on abstractions.
	public enum Relationship
	{
		Parent,
		Child,
		Sibling
	}
	
	public class Person
	{
		public string Name;
		// public DateTime DateOfBirth;
	}
	
	// Low-level parts of the system are defined as Relationships
	public interface IRelationshipBrowser
	{
		IEnumerable<Person> FindAllChildrenOf(string name);
	}
	

	public class Relationships : IRelationshipBrowser // low-level
	{
	// A list of tuples, You can access the tuple by Item1, Item2, Item3... etc
	// If you are not familiar with tuples, you can view the following official documentation here:
	// https://docs.microsoft.com/en-us/dotnet/api/system.tuple-4?view=net-6.0
	private List<(Person, Relationship, Person)> relations
		  = new List<(Person, Relationship, Person)>();
		
		// Mimic of an API, not typical database operation
		public void AddParentAndChild(Person parent, Person child)
		{
			relations.Add((parent, Relationship.Parent, child));
			// Reverse relationship
			relations.Add((child, Relationship.Child, parent));
		}
		
		// Here we expose the Relations with public field
		public List<(Person, Relationship, Person)> Relations => relations;
	
		public IEnumerable<Person> FindAllChildrenOf(string name)
		{
			return relations
			  .Where(x => x.Item1.Name == name
						  && x.Item2 == Relationship.Parent).Select(r => r.Item3);
		}
	}
	
	public class Research
	{
		public Research(Relationships relationships)
		{
			// high-level: find all of john's children
			// var relations = relationships.Relations;
			// foreach (var r in relations
			// .Where(x => x.Item1.Name == "John"
			//              && x.Item2 == Relationship.Parent))
			// {
			//  WriteLine($"John has a child called {r.Item3.Name}");
			// }
		}
	
		public Research(IRelationshipBrowser browser)
		{
			foreach (var p in browser.FindAllChildrenOf("John"))
			{
				WriteLine($"John has a child called {p.Name}");
			}
		}
	
		static void Main(string[] args)
		{
			var parent = new Person { Name = "John" };
			var child1 = new Person { Name = "Chris" };
			var child2 = new Person { Name = "Matt" };
	
			// low-level module
			var relationships = new Relationships();
			relationships.AddParentAndChild(parent, child1);
			relationships.AddParentAndChild(parent, child2);
			new Research(relationships);
		}
	}
}
