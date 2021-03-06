using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.ChainOfResponsibility.MethodChain
{
    public class Creature
    {
        public string Name;
        public int Attack, Defense;

        public Creature(string name, int attack, int defense)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    // This base class is like collection for all creatures
    public class CreatureModifier
    {
        protected Creature creature;
        protected CreatureModifier next; // A linked list

        public CreatureModifier(Creature creature)
        {
            this.creature = creature ?? throw new ArgumentNullException(paramName: nameof(creature));
        }

        public void Add(CreatureModifier cm)
        {
            if (next != null) next.Add(cm);
            else next = cm;
        }

        // When next is null we will reach the end of execution, calling Handle() will perform all commands 
        // in the entire collection (CreatureModifier)
        public virtual void Handle() => next?.Handle();
    }

    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            WriteLine($"Sorry, {creature.Name}, no bonuses for you! :(");
            // Do not forget the to call base.Handle(), otherwise it NoBonusesModifier will break the chain
            // regardless the fact that no bonuses are applied, check the DispellModifer
            base.Handle();
        }
    }

    public class CurseModifier : CreatureModifier
    {
        public CurseModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            WriteLine($"{creature.Name} has been cursed, no more buffs are allowed!");
            // Note that we break the chain here by removing the invocation of the Handle() method in the base class
            //base.Handle();
        }
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
        }

        // Overriding the vitual method Handle() in the base class CreatureModifier
        public override void Handle()
        {
            // We can access creature, because it is protected in the base class
            WriteLine($"Doubling {creature.Name}'s attack.");
            creature.Attack *= 2;
            // Will call CreatureModifier
            base.Handle();
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Creature creature) : base(creature)
        {
        }

        // Same thing with defence of the creature
        public override void Handle()
        {
            WriteLine("Increasing goblin's defense");
            creature.Defense += 3;
            base.Handle();
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var goblin = new Creature("Goblin", 2, 2);
            WriteLine(goblin);

            // The base class creature modifier
            var root = new CreatureModifier(goblin);

            // Apply no bonuses, just to test the functionality
            root.Add(new NoBonusesModifier(goblin));

            WriteLine("Let's double goblin's attack...");
            // Buff the attack
            root.Add(new DoubleAttackModifier(goblin));

            WriteLine("Let's increase goblin's defense");
            // Increase the defence with the defence modifer
            root.Add(new IncreaseDefenseModifier(goblin));

            // Will break the chain, no more buffs for the goblin
            root.Add(new CurseModifier(goblin));

            // Won't work
            root.Add(new DoubleAttackModifier(goblin));

            // Handle will execute all the commands, depending on the modifier on the creature
            // So the goblin will have double attack, and increased defence. And when we reach 
            // the CurseModifier the chain will break
            root.Handle();
            WriteLine(goblin);
        }
    }
}