// Example copied from https://refactoring.guru/design-patterns/facade/csharp/example

using System.Text;

namespace RefactoringGuru.DesignPatterns.Facade.Conceptual
{
    // The Facade class provides a simple interface to the complex logic of one
    // or several subsystems. The Facade delegates the client requests to the
    // appropriate objects within the subsystem. The Facade is also responsible
    // for managing their lifecycle. All of this shields the client from the
    // undesired complexity of the subsystem.
    public class Facade
    {
        protected Subsystem1 _subsystem1;

        protected Subsystem2 _subsystem2;

        public Facade(Subsystem1 subsystem1, Subsystem2 subsystem2)
        {
            this._subsystem1 = subsystem1;
            this._subsystem2 = subsystem2;
        }

        // The Facade's methods are convenient shortcuts to the sophisticated
        // functionality of the subsystems. However, clients get only to a
        // fraction of a subsystem's capabilities. 
        public string Operation()
        {
            // Note that we are not exposing any methods to the final user by executing Operation()
            // subsystem1.operationAlpha, Beta or Charley are never called etc.
            var sb = new StringBuilder();
            sb.AppendLine("Facade initializes subsystems:")
                .AppendLine(this._subsystem1.operation1())
                .AppendLine(this._subsystem2.operation1())
                .AppendLine("Facade orders subsystems to perform the action:")
                .AppendLine(this._subsystem1.operationN())
                .AppendLine(this._subsystem2.operationZ());
            return sb.ToString();
        }
    }

    // The Subsystem can accept requests either from the facade or client
    // directly. In any case, to the Subsystem, the Facade is yet another
    // client, and it's not a part of the Subsystem.
    public class Subsystem1
    {
        public string operation1()
        {
            return "Subsystem1: Ready!";
        }

        public string operationN()
        {
            return "Subsystem1: Go!";
        }

        public string operationAlpha()
        {
            return "Subsystem1: Useless operation!";
        }

        public string operationBeta()
        {
            return "Subsystem1: Memory eater activated!";
        }

        public string operationCharley()
        {
            return "Subsystem1: Heap overflow attack launched!";
        }        
    }
    

    // Some facades can work with multiple subsystems at the same time.
    public class Subsystem2
    {
        public string operation1()
        {
            return "Subsystem2: Get ready!";
        }

        public string operationZ()
        {
            return "Subsystem2: Fire!";
        }

        public string operationOmega()
        {
            return "Subsystem2: Do not expose this secret data!";
        }
    }


    class Client
    {
        // The client code works with complex subsystems through a simple
        // interface provided by the Facade. When a facade manages the lifecycle
        // of the subsystem, the client might not even know about the existence
        // of the subsystem. This approach lets you keep the complexity under
        // control.
        public static void ClientCode(Facade facade)
        {
            Console.Write(facade.Operation());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // The client code may have some of the subsystem's objects already
            // created. In this case, it might be worthwhile to initialize the
            // Facade with these objects instead of letting the Facade create
            // new instances.
            Subsystem1 subsystem1 = new Subsystem1();
            Subsystem2 subsystem2 = new Subsystem2();
            Facade facade = new Facade(subsystem1, subsystem2);
            Client.ClientCode(facade);
        }
    }
}