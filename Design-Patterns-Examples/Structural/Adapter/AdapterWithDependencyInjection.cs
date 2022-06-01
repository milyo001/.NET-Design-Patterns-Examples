using Autofac;
using Autofac.Features.Metadata;

// For the example I am using Autofac plugin. 
// It manages the dependencies between classes so that applications stay easy to change as they grow in size and complexity.
// This is achieved by treating regular . NET classes as components.
// For more info https://autofac.readthedocs.io/en/latest/
namespace AutofacDemos
{
    // Let's have a ICommand interface with a single void method Execute
    public interface ICommand
    {
        void Execute();
    }

    // SaveCommand to save data into file for example
    public class SaveCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Saving current file");
        }
    }

    // Open command to open a file
    public class OpenCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Opening a file");
        }
    }

    // Now we have a Button, which will have a command but injected with DI
    public class Button
    {
        // We have ICommand here, it is not inheritet
        private ICommand command;
        private string name;

        // Initialize the command and the name from the constructor
        public Button(ICommand command, string name)
        {
            if (command == null)
            {
                throw new ArgumentNullException(paramName: nameof(command));
            }
            this.command = command;
            this.name = name;
        }

        // When we click the button we execute the injected command
        public void Click()
        {
            command.Execute();
        }

        public void PrintMe()
        {
            Console.WriteLine($"I am a button called {name}");
        }
    }

    public class Editor
    {
        private readonly IEnumerable<Button> buttons;

        public IEnumerable<Button> Buttons => buttons;

        public Editor(IEnumerable<Button> buttons)
        {
            this.buttons = buttons;
        }

        // Diagnostic method to click all buttons
        public void ClickAll()
        {
            foreach (var btn in buttons)
            {
                btn.Click();
            }
        }
    }

    public class Adapters
    {
        static void Main_(string[] args)
        {
            // If you are not familiar with Autofac ContainerBuilder go to https://autofac.org/apidoc/html/717248.htm
            // For each ICommand, a ToolbarButton is created to wrap it, and all
            // are passed to the editor
            var b = new ContainerBuilder();

            // We register a OpenCommand and SaveCommand with the container meta data
            b.RegisterType<OpenCommand>()
              .As<ICommand>()
              .WithMetadata("Name", "Open");
            b.RegisterType<SaveCommand>()
              .As<ICommand>()
              .WithMetadata("Name", "Save");

            // Then we register an Container adapter using DI
            b.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd, ""));
            
            // If we need a metadata we can use the Meta<T> class
            b.RegisterAdapter<Meta<ICommand>, Button>(cmd =>
              new Button(cmd.Value, (string)cmd.Metadata["Name"]));
            
            // And lastly we register container type
            b.RegisterType<Editor>();
            
            using (var c = b.Build())
            {
                var editor = c.Resolve<Editor>();
                // Outputs "Saving a file" and "Opening a file"
                editor.ClickAll();


                foreach (var btn in editor.Buttons)
                    btn.PrintMe();
                // Outputs "I am a button called Save" and "I am a button called Open"
            }
        }
    }
}