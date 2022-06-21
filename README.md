# .NET-Design-Patterns-Examples
An educational project where all .NET OOP design patterns are implemented with given examples.
> :warning: **Bad practise alert**: A lot of public fields in the repository examples. In real projects use encapsulation and always think how to expose your data.

# Content
## SOLID Design Principles
## Creational
- Builder
> When constructing objects gets a little bit complicated. Some objects are simple and can be created with a simple constructor call. Other objects require a lot of ceremony to create. Having a constructor with 10 constructor arguments is not productive. 
- Factories
  * Abstract Factory
  > The purpose of the Abstract Factory is to provide an interface for creating families of related objects, without specifying concrete classes.
  * Factory Method
  > Creating an object in one invocation same as Builder but the act of creating object is outsourced from the actual object to something else like separate function 
   or a separate class (Factory)
- Prototype
> A partially or fully initialized object that you copy (clone) and make use of it. 
- Singleton
> Use the Singleton pattern when a class in your program should have just a single instance available to all clientsfor example, a single database object shared by different parts of the program.
## Structural
- Bridge
> Connecting components together through abstractions.
- Composite
> Allows composing objects into a tree-like structure and work with the it as if it was a singular object.
- Adapter
> Getting the interface you need from the interface you already have.
- Facade 
> Exposing several components through a single interface.
- Decorator
> Adding behavior without altering the class itself.
- Flyweight
> Used for space optimization.
- Proxy
> An interface for accessing particular resource. 
## Behavioral
- Chain of Responsibility
> Sequence of handlers processing an event one after another.
- Command
> Command turns a request into a stand-alone object that contains all information about the request. This transformation lets you pass requests as a method arguments, delay or queue a request’s execution, and support undoable operations.
- Interpreter
> A component that processes structured text data (by lexing and then parsing tokens).
- Iterator
> How traversal of data structures happens and who exactly allows the traversal.
- Mediator
> A componet that facilitates  communication between other components without them necessarily being aware of each other or have a direct reference access to each other. Used mostly in chat rooms participants and players in MMORPGs 
- Memento
> A token/handle representing the system state. It let's us roll back to the state when the token was generated. May or may not directly expose state information.
- Null Object
> A no operation object that conforms to the required  interface, satisfying a dependancy requirement of some other object.
- Observer
> Observer is an object that wishes to be informed about events happening in the system. The entity generating the events is an observable.
- State
> Object's behavior is determined by its state. An object transitions from one to another (something needs to trigger a transaction). A formalized construct which manages state and transitions is called state machine.
- Strategy
> Enable  the exact  behavior of the system to be selected either at run-time (dynamic) or compile time (static). Also know as a policy (in low level languages like C++)
- Template Method
> Allows us to define the "skeleton" of the algorithm, with concreteimplementations defined in subclasses
- Visitor
> A component (visitor) is allowed to traverse the entire inheritance hierarchy. Implemented by propagating a single Visit() method throughout the entire hierarchy
