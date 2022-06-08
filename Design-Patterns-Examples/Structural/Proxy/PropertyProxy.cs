using static System.Console;

namespace DesignPatterns
{
    public class Property<T> : IEquatable<Property<T>> where T : new()
    {
        private T value;

        public T Value
        {
            // Default value of the getter;
            get => value;
            set
            {
                if (Equals(this.value, value)) return;
                // Otherwise log some string
                WriteLine($"Assigning value to {value}");
                this.value = value;
            }
        }

        public Property() : this(default(T))
        {

        }

        public Property(T value)
        {
            this.value = value;
        }

        // For implicit coversion (assignment)
        public static implicit operator T(Property<T> property)
        {
            // int n = p_int; (when using property of int)
            return property.value; 
        }

        public static implicit operator Property<T>(T value)
        {
            // This operator take care of the case Property<int> p = 12345;
            return new Property<T>(value); 
        }

        // Equality members
        
        // Generated from Resharper, used by the operators above
        public bool Equals(Property<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(value, other.value);
        }
        
        // Generated from Resharper, used by the operators above
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Property<T>)obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(Property<T> left, Property<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Property<T> left, Property<T> right)
        {
            return !Equals(left, right);
        }
    }

    public class Creature
    {
        private Property<int> agility = new Property<int>();

        public int Agility
        {
            get => agility.Value;
            set => agility.Value = value;
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var c = new Creature();

            // Assignment operator uses implicit conversion in Property Object
            // This will execute the following code
            // c.Agility = new Property<int>(10) 
            c.Agility = 10; 
            
            c.Agility = 10;
        }
    }
}