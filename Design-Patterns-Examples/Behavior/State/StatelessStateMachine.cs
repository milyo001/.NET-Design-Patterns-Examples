using Stateless;

// Using stateless nuget package
namespace DotNetDesignPatternDemos.Behavioral.State.Stateless
{
    public enum Health
    {
        NonReproductive,
        Pregnant,
        Reproductive
    }

    public enum Activity
    {
        GiveBirth,
        ReachPuberty,
        HaveAbortion,
        HaveUnprotectedSex,
        Historectomy
    }

    class Demo
    {
        static void Main(string[] args)
        {
            // Two argumests: state and trigger and specify the initial (default) state, which in our case is Health.NonReproductive
            var stateMachine = new StateMachine<Health, Activity>(Health.NonReproductive);
            
            // If you are in non reproductive state than you can transition to a ReachPuberty and Reproductive state
            stateMachine.Configure(Health.NonReproductive)
              .Permit(Activity.ReachPuberty, Health.Reproductive);
            
            stateMachine.Configure(Health.Reproductive)
              .Permit(Activity.Historectomy, Health.NonReproductive)
              .PermitIf(Activity.HaveUnprotectedSex, Health.Pregnant,
                () => ParentsNotWatching);
            
            stateMachine.Configure(Health.Pregnant)
              .Permit(Activity.GiveBirth, Health.Reproductive)
              .Permit(Activity.HaveAbortion, Health.Reproductive);
        }

        public static bool ParentsNotWatching
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}