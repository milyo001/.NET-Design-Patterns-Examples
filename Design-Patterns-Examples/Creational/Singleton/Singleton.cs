using MoreLinq;
using NUnit.Framework;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Singleton
{
    public interface IDatabase
    {
        int GetPopulationByName(string name);
    }

    // A mimic for database which will read some data from store, a text file in Singleton folder
    public class SingletonDatabase : IDatabase
    {
        // Field for capitals of towns/cities
        private Dictionary<string, int> capitals;
        // Field for instance of SingletonDatabase count
        private static int instanceCount;

        // Public property to get the instance count of SingletonDatabase from the private field
        public static int Count => instanceCount;

        // Note: Private constructor to prevent creating new instance of SingletonDatabase
        private SingletonDatabase()
        {
            WriteLine("Initializing database...");

            // Make sure the file Copy to Output Directory is set to Copy Always or Copy If Newer
            // (select the file capitals.txt and go to properties, click Copy Always)
            capitals = File.ReadAllLines(
              Path.Combine(
                new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, "capitals.txt")
              )
              .Batch(2)
              .ToDictionary(
                // The key is the name of the capital
                list => list.ElementAt(0).Trim(),
                // The value is the population of the capital
                list => int.Parse(list.ElementAt(1)));
        }

        public int GetPopulationByName(string name)
        {
            return capitals[name];
        }

        // Laziness + thread safety
        // The instance is initialized only when somebody calls the IDatabase Instance property
        // Example: var db = SingletonDatabase.Instance;
        private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() =>
        {
            instanceCount++;
            return new SingletonDatabase();
        });

        // Static property to get the instance of SingletonDatabase (there is only one instance)
        public static IDatabase Instance => instance.Value;
    }

    public class SingletonRecordFinder
    {
        public int TotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
                // Hard coded reference to the SingletonDatabase instance, which is bad idea
                result += SingletonDatabase.Instance.GetPopulationByName(name);
            return result;
        }
    }

    //  ConfigurableRecordFinder will find the total population of the cities by given cities names
    public class ConfigurableRecordFinder
    {
        private IDatabase database;

        public ConfigurableRecordFinder(IDatabase database)
        {
            this.database = database;
        }

        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
                result += database.GetPopulationByName(name);
            return result;
        }
    }

    public class DummyDatabase : IDatabase
    {
        public int GetPopulationByName(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[name];
        }
    }


    /// <summary>
    /// IMPORTANT: be sure to turn off shadow copying for unit tests in R#!
    /// Singleton pattern is hard to test 
    /// </summary>
    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;
            Assert.That(db, Is.SameAs(db2));
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
        }

        [Test]
        public void SingletonTotalPopulationTest()
        {
            // Testing on a live database
            var rf = new SingletonRecordFinder();
            var names = new[] { "Seoul", "Mexico City" };
            int tp = rf.TotalPopulation(names);
            Assert.That(tp, Is.EqualTo(17500000 + 17400000));
        }

        [Test]
        public void DependantTotalPopulationTest()
        {
            var db = new DummyDatabase();
            var rf = new ConfigurableRecordFinder(db);
            Assert.That(
              rf.GetTotalPopulation(new[] { "alpha", "gamma" }),
              Is.EqualTo(4));
        }
    }
    
    public class Demo
    {
        static void Main()
        {
            // We are getting the instance with static call, since the constructor is private
            var db = SingletonDatabase.Instance;

            // Works just fine while you're working with a real database.
            var city = "Tokyo";
            WriteLine($"{city} has population of {db.GetPopulationByName(city)}");
        }
    }

    
}
