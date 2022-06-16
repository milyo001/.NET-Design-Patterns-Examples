using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Memento.UndoRedo
{
    public class Memento
    {
        public int Balance { get; }

        public Memento(int balance)
        {
            Balance = balance;
        }
    }

    public class BankAccount
    {
        private int balance;
        
        // Used for undo and redo
        private List<Memento> changes = new List<Memento>();
        
        // The state we are currently in
        private int current;

        public BankAccount(int balance)
        {
            this.balance = balance;
            // We also add the change 
            changes.Add(new Memento(balance));
        }

        public Memento Deposit(int amount)
        {
            balance += amount;
            var m = new Memento(balance);
            
            // Add changes here as well
            changes.Add(m);
            
            // Increment the current state
            ++current;
            return m;
        }

        public void Restore(Memento m)
        {
            if (m != null)
            {
                balance = m.Balance;
                // Even a restore is operation which we track in changes list
                changes.Add(m);
                current = changes.Count - 1;
            }
        }

        // Undo functionality 
        public Memento Undo()
        {
            if (current > 0)
            {
                // Go back to the previous state of memento
                var m = changes[--current];
                // Set the balance of the previous memento
                balance = m.Balance;
                return m;
            }
            return null;
        }

        // Similar functionality for redo
        public Memento Redo()
        {
            if (current + 1 < changes.Count)
            {
                // Reversed functionality
                var m = changes[++current];
                balance = m.Balance;
                return m;
            }
            return null;
        }

        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}";
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount(100);
            ba.Deposit(50);
            ba.Deposit(25);
            WriteLine(ba);

            ba.Undo();
            WriteLine($"Undo 1: {ba}");
            ba.Undo();
            WriteLine($"Undo 2: {ba}");
            ba.Redo();
            WriteLine($"Redo 2: {ba}");
        }
    }
}