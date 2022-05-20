
namespace Design-Patterns-Examples.SOLID Design Principles
{
	using System;

	public class Document
{
}

// Let's say we have IMachine interface for all printers functionality
public interface IMachine
{
	void Print(Document d);
	void Fax(Document d);
	void Scan(Document d);
}

// Ok if you need a multifunction machine, but what about a simple old fashioned printer? Or maybe a Fax machine?
public class MultiFunctionPrinter : IMachine
{
	public void Print(Document d)
	{
		// Do something here
	}

	public void Fax(Document d)
	{
		// Do something here
	}

	public void Scan(Document d)
	{
		// Do something here
	}
}

public class OldFashionedPrinter : IMachine
{
	public void Print(Document d)
	{
        // Oops, The OldFashionedPrinter can only print documents and that's it, It is too outdated to send Fax or Scan
    }

    // We do not need to implement the other methods
    public void Fax(Document d)
	{
		throw new System.NotImplementedException();
	}

	public void Scan(Document d)
	{
		throw new System.NotImplementedException();
	}
}

// Here comes the Interface Segregation principle. Create smaller interfaces that can be inherited from classes/interfaces

// As you can see the IPrinter contains only Print method
public interface IPrinter
{
	void Print(Document d);
}

// IScanner contains only Scan method
public interface IScanner
{
	void Scan(Document d);
}

// Now the Printer class can inherit from IPrinter without adding additional methods which are not needed
public class Printer : IPrinter
{
	public void Print(Document d)
	{
        // Hurray, the Printer can print documents and nothing else
    }
}

// Same goes for Photocopier class which can inherit from IPrinter and IScanner without
// adding additional methods which are not needed
public class Photocopier : IPrinter, IScanner
{
	public void Print(Document d)
	{
		throw new System.NotImplementedException();
	}

	public void Scan(Document d)
	{
		throw new System.NotImplementedException();
	}
}

// This interface will combine IPrinter and IScanner interfaces
public interface IMultiFunctionDevice : IPrinter, IScanner //
{

}

// Also example of Decorator design pattern
public class MultiFunctionMachine : IMultiFunctionDevice
{
	// Compose this out of several modules inherited from IMultiFunctionDevice interface
	private IPrinter printer;
	private IScanner scanner;

	public MultiFunctionMachine(IPrinter printer, IScanner scanner)
	{
		if (printer == null)
		{
			throw new ArgumentNullException(paramName: nameof(printer));
		}
		if (scanner == null)
		{
			throw new ArgumentNullException(paramName: nameof(scanner));
		}
		this.printer = printer;
		this.scanner = scanner;
	}

	public void Print(Document d)
	{
		printer.Print(d);
	}

	public void Scan(Document d)
	{
		scanner.Scan(d);
	}
}
}
