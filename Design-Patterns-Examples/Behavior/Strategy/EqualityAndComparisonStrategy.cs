
// todo: this is somehow buggy in recording
namespace DotNetDesignPatternDemos.Behavioral.Strategy
{
    // Let's say we need to sort a collection of person class and calling people.Sort() won't work by default 
    // because we need to specify a condition
    class Person : IEquatable<Person>, IComparable<Person>
    {
        public int Id;
        public string Name;
        public int Age;

        // IComparable
        public int CompareTo(Person other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Id.CompareTo(other.Id);
        }

        public Person(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }

        // IEquatable
        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        // IEquatable
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Person)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(Person left, Person right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Person left, Person right)
        {
            return !Equals(left, right);
        }

        // Inner class to sort by name
        private sealed class NameRelationalComparer : IComparer<Person>
        {
            public int Compare(Person x, Person y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return string.Compare(x.Name, y.Name,
                  StringComparison.Ordinal);
            }
        }

        public static IComparer<Person> NameComparer { get; }
          = new NameRelationalComparer();
    }

    public class ComparisonStrategies
    {
        public static void Main(string[] args)
        {
            var people = new List<Person>();

            // Equality == != and comparison < = >

            people.Sort(); // meaningless by default

            // Sort by name with a lambda
            people.Sort((x, y) => x.Name.CompareTo(y.Name));

            people.Sort(Person.NameComparer);

        }
    }
}