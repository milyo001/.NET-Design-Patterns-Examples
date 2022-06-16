using System.Reactive.Linq;
using static System.Console;

// Observer without using the 'event' keyword, using reactive extentions (RX)
// Events can have memory leak, will use IObservable which inherit IDisposable
namespace DotNetDesignPatternDemos.Behavioral.Observer.Interfaces
{
    // Mimic an event, an empty base class
    public class Event
    {

    }

    // Same as Event observer example, but this time inherit Event clas
    public class FallsIllEvent : Event
    {
        public string Address;
    }

    // Person can fall ill
    public class Person : IObservable<Event>
    {
        private readonly HashSet<Subscription> subscriptions
          = new HashSet<Subscription>();

        // Inherited from IObservable<Event>, the events will be disposable now so no memory leak
        public IDisposable Subscribe(IObserver<Event> observer)
        {
            var subscription = new Subscription(this, observer);
            subscriptions.Add(subscription);
            // Return IDisposable so we can Dispose() it later
            return subscription;
        }

        // Will fire all the event subscriptions
        public void CatchACold()
        {
            foreach (var sub in subscriptions)
                sub.Observer.OnNext(new FallsIllEvent { Address = "123 London Road" });
        }

        // Inner private class, also disposable
        private class Subscription : IDisposable
        {
            // Store the person 
            private Person person;
            
            // Can be accessed outside the class
            public IObserver<Event> Observer;

            // The first argument is the object we are subscibing to, and the is the event (observer)
            public Subscription(Person person, IObserver<Event> observer)
            {
                this.person = person;
                Observer = observer;
            }

            // Remove this subscribtion from the person
            public void Dispose()
            {
                person.subscriptions.Remove(this);
            }
        }
    }

    public class Demo : IObserver<Event>
    {
        static void Main(string[] args)
        {
            new Demo();
        }

        // Constructor
        public Demo()
        {
            var person = new Person();
            IDisposable sub = person.Subscribe(this);

            person.OfType<FallsIllEvent>()
              .Subscribe(args => WriteLine($"A doctor has been called to {args.Address}"));
            
            person.CatchACold();

            // If you want you can dispose it anytime
            sub.Dispose();
        }

        // Inherited from IObserver<Event>
        public void OnNext(Event value)
        {
            if (value is FallsIllEvent args)
                WriteLine($"A doctor has been called to {args.Address}");
        }
        // Inherited from IObserver<Event>
        public void OnError(Exception error) { }

        // Inherited from IObserver<Event>, when there are no more events, no more events will be generated  
        public void OnCompleted() { }
    }
}
