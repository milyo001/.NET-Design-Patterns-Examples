
// Let's have a Person class that can fall ill and find a way to notify us that the person is ill and call the doctor
namespace DotNetDesignPatternDemos.Behavioral.Observer
{
    // The event arguments used in Person class CatchCold()
    public class FallsIllEventArgs
    {
        public string Address;
    }

    public class Person
    {
        public void CatchACold()
        {
            // Using the safe call '?' to prevent null ref exeption if nobody has subscribed to the event 
            // Invoke() first parameter sender method is who exactly generates this event, so we are sending 'this' (Person instance)
            // The second parameter is a set of arguments, new example: FallsIllEventArgs { Name = "Miro" }, EventArgs.Emty etc.
            FallsIll?.Invoke(this,
              new FallsIllEventArgs { Address = "123 London Road" });
        }

        // Using the event keyword
        public event EventHandler<FallsIllEventArgs> FallsIll;
    }

    public class Demo
    {
        static void Main()
        {
            var person = new Person();

            // Listen to 
            person.FallsIll += CallDoctor;

            // After we call the CatchACold method the event will trigger and print "A doctor has been called to 123 London Road"
            person.CatchACold();
            
            // When you are no longer interested in listening the event
            person.FallsIll -= CallDoctor;
        }

        // When person is ill call the doctor to help him  
        private static void CallDoctor(object sender, FallsIllEventArgs eventArgs)
        {
            Console.WriteLine($"A doctor has been called to {eventArgs.Address}");
        }
    }
}