using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.ChainOfResponsibility.ModifierChain.BrokerChain
{
    // Command query separation is being used here

    public class Query
    {
        public string CreatureName;

        public enum Argument
        {
            Attack, Defense
        }

        public Argument WhatToQuery;

        public int Value; // bidirectional

        // The value parameter is used if there is a bonus on top of the initial value
        public Query(string creatureName, Argument whatToQuery, int value)
        {
            CreatureName = creatureName ?? throw new ArgumentNullException(paramName: nameof(creatureName));
            WhatToQuery = whatToQuery;
            Value = value;
        }
    }

    public class Game // Mediator pattern
    {
        public event EventHandler<Query> Queries; // Effectively a chain

        public void PerformQuery(object sender, Query q)
        {
            Queries?.Invoke(sender, q);
        }
    }

    public class Creature
    {
        private Game game;
        public string Name;
        private int attack, defense;

        public Creature(Game game, string name, int attack, int defense)
        {
            this.game = game ?? throw new ArgumentNullException(paramName: nameof(game));
            this.Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            this.attack = attack;
            this.defense = defense;
        }

        public int Attack
        {
            get
            {
                var q = new Query(Name, Query.Argument.Attack, attack);
                game.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int Defense
        {
            get
            {
                var q = new Query(Name, Query.Argument.Defense, defense);
                game.PerformQuery(this, q);
                return q.Value;
            }
        }

        // Output the properties, not the fields because we are using their getters 
        public override string ToString() // no game
        {
            return $"{nameof(Name)}: {Name}, {nameof(attack)}: {Attack}, {nameof(defense)}: {Defense}";
            //                                                 ^^^^^^^^ using a property    ^^^^^^^^^
        }
    }

    // Without linked list (you can add and remove modifier)
    public abstract class CreatureModifier : IDisposable
    {
        // Protected because we are in abstract class, references to game and creature
        protected Game game;
        protected Creature creature;

        protected CreatureModifier(Game game, Creature creature)
        {
            this.game = game;
            this.creature = creature;
            // Subscribe
            game.Queries += Handle;
        }

        protected abstract void Handle(object sender, Query q);

        public void Dispose()
        {
            // Unsubscribe
            game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        protected override void Handle(object sender, Query q)
        {
            // Check if that's the right creature, because this a query not command like the previous example
            if (q.CreatureName == creature.Name &&
                q.WhatToQuery == Query.Argument.Attack)
                q.Value *= 2;
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        protected override void Handle(object sender, Query q)
        {
            if (q.CreatureName == creature.Name &&
                q.WhatToQuery == Query.Argument.Defense)
                q.Value += 2;
        }
    }

    public class Demo
    {
        public static void Main()
        {
            var game = new Game();
            var goblin = new Creature(game, "Strong Goblin", 3, 3);
            WriteLine(goblin);

            using (new DoubleAttackModifier(game, goblin))
            {
                WriteLine(goblin);
                using (new IncreaseDefenseModifier(game, goblin))
                {
                    WriteLine(goblin);
                }
            }

            WriteLine(goblin);
        }
    }
}