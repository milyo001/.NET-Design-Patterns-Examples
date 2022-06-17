
// This is scenario is included, but this is not the way we build state machines nowdays
namespace DotNetDesignPatternDemos.Behavioral.State.Classic
{
    
    public class Switch
    {
        public State State = new OffState();
        public void On() { State.On(this); }
        public void Off() { State.Off(this); }
    }

    public abstract class State
    {
        // If we call lightswitch.On() and the state object will be OnState which has overriden Off method the On method is
        //  inherited from this base class
        public virtual void On(Switch sw)
        {
            Console.WriteLine("Light is already on.");
        }

        // And if we call lightswitch.Off() and the state object will be OffState which has overriden On method the Off method
        // is inherited from this base class
        public virtual void Off(Switch sw)
        {
            Console.WriteLine("Light is already off.");
        }
    }

    public class OnState : State
    {
        public OnState()
        {
            Console.WriteLine("Light turned on.");
        }
        // In the OnState class, we can turn the light off only 
        public override void Off(Switch sw)
        {
            Console.WriteLine("Turning light off...");
            // Set the state as new OffState in the abstract Switch base class
            sw.State = new OffState();
        }
    }

    public class OffState : State
    {
        public OffState()
        {
            Console.WriteLine("Light turned off.");
        }

        // In the OffState class, we can turn the light on only
        public override void On(Switch sw)
        {
            Console.WriteLine("Turning light on...");
            // Set the state as new OnState in the abstract Switch base class
            sw.State = new OnState();
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            var lightSwitch = new Switch();
            lightSwitch.On();
            lightSwitch.Off();
            lightSwitch.Off();
        }
    }
}
