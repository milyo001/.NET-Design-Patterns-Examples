namespace Design-Patterns-Examples.SOLID Design Principles
{
	using System;
	using System.Collections.Generic;
	using static System.Console;
  
    // Used in Product class
	public enum Color
	{
		Red, Green, Blue
	}

	// Used in Product class
	public enum Size
	{
		Small, Medium, Large, XXXXXXXLSize
	}

	public class Product
	{
		public string Name;
		public Color Color;
		public Size Size;

		// Creates a product with the specified values from constructor arguments
		public Product(string name, Color color, Size size)
		{
        
			// Checks if name argument is null
			Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
			Color = color;
			Size = size;
		}
	}

	// The Product Filter will be used to filter the products by given criteria
	public class ProductFilter
	{
		// So we have two filters below to filter the products class by given products collection and criteria
		public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
		{
			foreach (var p in products)
				if (p.Color == color)
					yield return p;
		}

		public static IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
		{
			foreach (var p in products)
				if (p.Size == size)
					yield return p;
		}

		// Here where the open close principle breaks. Let's see if your boss tells you to do another 50 filters for example.
		// Every new filter method like FilterBySizeAndColor will extend the ProductFilter class and code will become messy.
		// And if we need additional filters, we need to create a new filter class which inherits from ProductFilter
		public static IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
		{
			foreach (var p in products)
				if (p.Size == size && p.Color == color)
					yield return p;
		} 
	}

	// We introduce two new interfaces
	public interface ISpecification<T>
	{
		bool IsSatisfied(Product p);
	}

	public interface IFilter<T>
	{
		IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
	}

	// We create a new class which implements the ISpecification<Product> interface
	public class ColorSpecification : ISpecification<Product>
	{
		private Color color;

		public ColorSpecification(Color color)
		{
			this.color = color;
		}

		public bool IsSatisfied(Product p)
		{
			return p.Color == color;
		}
	}
	
	public class SizeSpecification : ISpecification<Product>
	{
		private Size size;

		public SizeSpecification(Size size)
		{
			this.size = size;
		}

		public bool IsSatisfied(Product p)
		{
			return p.Size == size;
		}
	}

	// Combinator class 
	public class AndSpecification<T> : ISpecification<T>
	{
		private ISpecification<T> first, second;

		public AndSpecification(ISpecification<T> first, ISpecification<T> second)
		{
			this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
			this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
		}

		// Checks if the first and second specification are satisfied
		public bool IsSatisfied(Product p)
		{
			return first.IsSatisfied(p) && second.IsSatisfied(p);
		}
	}

	public class BetterFilter : IFilter<Product>
	{
		public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
		{
			foreach (var i in items)
				if (spec.IsSatisfied(i))
					yield return i;
		}
	}

	public class Demo
	{
		static void Main(string[] args)
		{
			// BEFORE
			var apple = new Product("Apple", Color.Green, Size.Small);
			var tree = new Product("Tree", Color.Green, Size.Large);
			var house = new Product("House", Color.Blue, Size.Large);

			Product[] products = { apple, tree, house };

			var pf = new ProductFilter();
			WriteLine("Green products (with broken principle):");
        
			foreach (var p in pf.FilterByColor(products, Color.Green))
				WriteLine($" - {p.Name} is green");


			// AFTER
			var bf = new BetterFilter();
			WriteLine("Green products (new):");
			foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
				WriteLine($" - {p.Name} is green");

			WriteLine("Large products");
			foreach (var p in bf.Filter(products, new SizeSpecification(Size.Large)))
				WriteLine($" - {p.Name} is large");

			WriteLine("Large blue items");
			foreach (var p in bf.Filter(products,
			  new AndSpecification<Product>(new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large)))
			)
			{
				WriteLine($" - {p.Name} is big and blue");
			}
		}
	}
}
