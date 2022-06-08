using ImpromptuInterface;
using System.Dynamic;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Proxy
{
    // Let's say we hace a BankAccount interface and class with two simple methods Withdraw and Deposit
    public interface IBankAccount
    {
        void Deposit(int amount);
        bool Withdraw(int amount);
        string ToString();
    }

    public class BankAccount : IBankAccount
    {
        private int balance;
        private int overdraftLimit = -500;

        public void Deposit(int amount)
        {
            balance += amount;
            WriteLine($"Deposited ${amount}, balance is now {balance}");
        }

        public bool Withdraw(int amount)
        {
            if (balance - amount >= overdraftLimit)
            {
                balance -= amount;
                WriteLine($"Withdrew ${amount}, balance is now {balance}");
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}";
        }
    }

    // Dynamic proxy, DynamicObject is a bit of complicated 
    public class Log<T> : DynamicObject 
        where T : class, new()
    {
        // A reference to the subject. 
        private readonly T subject;
        
        // The key is the name of the method and the value is the number of times the method was called 
        private Dictionary<string, int> methodCallCount = new();

        // The default constructor, which is required from the constraint new() above
        protected Log(T subject)
        {
            this.subject = subject ?? throw new ArgumentNullException(paramName: nameof(subject));
        }

        // Factory method
        public static I As<I>(T subject) where I : class
        {
            if (!typeof(I).IsInterface)
                throw new ArgumentException("I must be an interface type");

            // duck typing here!
            return new Log<T>(subject).ActLike<I>();
        }

        public static I As<I>() where I : class
        {
            if (!typeof(I).IsInterface)
                throw new ArgumentException("I must be an interface type");

            // duck typing here!
            return new Log<T>(new T()).ActLike<I>();
        }

        // Overriding the DynamicObject member TryInvokeMember
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                // Logging
                WriteLine($"Invoking {subject.GetType().Name}.{binder.Name} with arguments [{string.Join(",", args)}]");

                // Additional logging

                // Check if the name of the method is present in the Dictionary
                if (methodCallCount.ContainsKey(binder.Name)) methodCallCount[binder.Name]++;

                // Otherwise add the name and set it's value to one
                else methodCallCount.Add(binder.Name, 1);

                result = subject.GetType().GetMethod(binder.Name).Invoke(subject, args);

                // Return true if everything works correct
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public string Info
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var kv in methodCallCount)
                    sb.AppendLine($"{kv.Key} called {kv.Value} time(s)");
                return sb.ToString();
            }
        }

        // will not be proxied automatically
        public override string ToString()
        {
            return $"{Info}{subject}";
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            //var ba = new BankAccount();
            var ba = Log<BankAccount>.As<IBankAccount>();

            ba.Deposit(100);
            ba.Withdraw(50);

            WriteLine(ba);
        }
    }
}