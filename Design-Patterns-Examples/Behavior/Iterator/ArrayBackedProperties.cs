using System.Collections;

namespace DotNetDesignPatternDemos.Structural.Iterator.ArrayBackedProperties
{
    public class Creature : IEnumerable<int>
    {
        // Strength, Agility, Intelligence
        private int[] stats = new int[3];

        // Idexes of stats
        private const int strength = 0;
        private const int agility = 1;
        private const int intelligence = 2;

        // Array backed properties
        public int Strength
        {
            get => stats[strength];
            set => stats[strength] = value;
        }

        public int Agility 
        {
            get => stats[agility];
            set => stats[agility] = value;
        }
        
        public int Intelligence 
        {
            get => stats[intelligence];
            set => stats[intelligence] = value;
        }

        // The average stats for creature
        public double AverageStat =>
          stats.Average();

        
        public IEnumerator<int> GetEnumerator()
        {
            // Expose the stats array as enumerable 
            return stats.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Get the STR, AGI or INT values by given index
        public int this[int index]
        {
            get { return stats[index]; }
            set { stats[index] = value; }
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var creature = new Creature();
            // Sets creature strength to 1
            creature[0] = 1;
            // Sets creature agility to 2
            creature[0] = 2;
            // Sets creature inteligence to 1
            creature[0] = 1;

            // Iterates the statuses of the creature
            foreach (var status in creature)
            {
                Console.WriteLine(status);
            }
            
            // Getters of Creature class
            Console.WriteLine($"Strengt is {creature[0]}");
            Console.WriteLine($"Agility is {creature[1]}");
            Console.WriteLine($"Inteligence is {creature[2]}");
        }
    }
}