using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Observer.WeakEventPattern
{
    // An event subscription can lead to a memory
    // Leak if you hold on to it past the object's
    // lifetime
    
    // Weak events help with this

    public class Button
    {
        public event EventHandler Clicked;

        // Fire the Clicked event
        public void Fire()
        {
            // Empty default args
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Window
    {
        public Window(Button button)
        {
            // Not taking referance to the button, instead subscribe to the Clicked event
            button.Clicked += ButtonOnClicked;
        }

        private void ButtonOnClicked(object sender, EventArgs eventArgs)
        {
            WriteLine("Button clicked (Window handler)");
        }

        // This is how you unsubscribe from the event, you need a reference to button example: private Button button;
        
        //public void Unsubscribe()
        //{
        //    button.Clicked -= ButtonOnClicked;
        //} 

            
        // If you are not familiar with C# Destructors check the official documentation: https://www.geeksforgeeks.org/destructors-in-c-sharp/
        // Used for statistics on the console
        ~Window()
        {
            WriteLine("Window finalized");
        }
    }

    // Another solution is to include Windows.Base assembly and use WeakEventManager<T, EventArgs>()
    
    //public class Window2
    //{
    //    public Window2(Button button)
    //    {
    //        WeakEventManager<Button, EventArgs>
    //          .AddHandler(button, "Clicked", ButtonOnClicked);
    //    }

    //    private void ButtonOnClicked(object sender, EventArgs eventArgs)
    //    {
    //        WriteLine("Button clicked (Window2 handler)");
    //    }

    //    ~Window2()
    //    {
    //        WriteLine("Window2 finalized");
    //    }
    //}

    public class Demo
    {
        static void Main(string[] args)
        {
            var btn = new Button();
            
            var window = new Window(btn);
            
            var windowRef = new WeakReference(window);
            btn.Fire();

            WriteLine("Setting window to null");
            // This won't work, since we've already subscribed to the button from the Window class, the button isn't dead yet
            // which brings us memory leak, the solution is to unsubscribe from the event
            window = null;

            FireGC();
            WriteLine($"Is window alive after GC? {windowRef.IsAlive}"); // true

            btn.Fire();

            WriteLine("Setting button to null");
            btn = null;

            FireGC();
        }

        // Fire garbage collector
        private static void FireGC()
        {
            WriteLine("Starting GC");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            WriteLine("GC is done!");
        }
    }
}