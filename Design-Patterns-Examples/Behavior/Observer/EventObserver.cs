
// Let's have a Person class that can fall ill and find a way to notify us that the person is ill and call the doctor
namespace DotNetDesignPatternDemos.Behavioral.Observer
{
    public class FallsIllEventArgs
    {
        public string Address;
    }

    public class Person
    {
        public void CatchACold()
        {
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

            person.FallsIll += CallDoctor;

            person.CatchACold();
        }

        // When person is ill call the doctor to help him 
        private static void CallDoctor(object sender, FallsIllEventArgs eventArgs)
        {
            Console.WriteLine($"A doctor has been called to {eventArgs.Address}");
        }
    }
}