# .NET-Design-Patterns-Examples
An educational project where all .NET OOP design patterns are implemented with given examples.
> :warning: **Bad practise alert**: A lot of public fields in the repository examples. In real projects use encapsulation and always think how to expose your data.

# Content
## SOLID Design Principles
## Creational
- Builder
> **When constructing objects gets a little bit complicated.** Some objects are simple and can be created with a simple constructor call. Other objects require a lot of ceremony to create. Having a constructor with 10 constructor arguments is not productive. 
- Factories
  * Abstract Factory
  > **The purpose of the Abstract Factory is to provide an interface for creating families of related objects, without specifying concrete classes.**
  * Factory Method
  > Creating an object in one invocation same as Builder but the act of **creating object is outsourced from the actual object to something else like separate function 
   or a separate class (Factory)**
- Prototype
> When it is easier to copy existing object to fully initialize a new one.
- Singleton
> **Use the Singleton pattern when a class in your program should have just a single instance available to all clientsfor example**, a single database object shared by different parts of the program.
## Structural
- Bridge
> Connecting components together through abstractions.
- Composite
> Allows composing objects into a tree-like structure and work with the it as if it was a singular object.
- Adapter
> Getting the interface you need from the interface you already have.
- Facade 
- Decorator
> Adding behavior without altering the class itself.
- Flyweight
- Proxy
## Behavioral
- Chain of Responsibility
- Command
- Interpreter
- Iterator
- Mediator
- Memento
- Null Object
- Observer
- State
- Strategy
- Template Method
- Visitor
