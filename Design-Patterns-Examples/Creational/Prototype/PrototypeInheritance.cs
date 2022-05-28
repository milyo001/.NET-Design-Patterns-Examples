namespace DotNetDesignPatternDemos.Creational.PrototypeInheritance
{
    public interface IDeepCopyable<T> where T : new()
    {
        void CopyTo(T target);

        // Explicitly casts an variable to IDeepCopyable<T>, there is no need to implement this missing memeber in classes 
        // which inherit the interface, because we already have it here in the interface.
        public T DeepCopy()
        {
            T t = new T();
            CopyTo(t);
            return t;
        }
    }

    public class Address : IDeepCopyable<Address>
    {
        public string StreetName;
        public int HouseNumber;

        // Default constructor
        public Address()
        {

        }        
        
        public Address(string streetName, int houseNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        

        public override string ToString()
        {
            return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
        }

        // Inherirted from IDeepCopyable, Note that there is no need to implment DeepCopy() in classes which inherit the interface
        // The address CopyTo() is easy to implement because we have just two properties
        public void CopyTo(Address target)
        {
            target.StreetName = StreetName;
            target.HouseNumber = HouseNumber;
        }
    }



    public class Person : IDeepCopyable<Person>
    {
        public string[] Names;
        public Address Address;

        public Person()
        {

        }

        public Person(string[] names, Address address)
        {
            Names = names;
            Address = address;
        }

        public override string ToString()
        {
            return $"{nameof(Names)}: {string.Join(",", Names)}, {nameof(Address)}: {Address}";
        }

        // Inherirted from IDeepCopyable, Note that there is no need to implment DeepCopy() in classes which inherit the interface
        public virtual void CopyTo(Person target)
        {
            // The Array.Clone() method returns a new array (a shallow copy) object containing all the elements in the original array. 
            //  A shallow copy means the contents (each array element) contains references to the same object as the elements in the original array.
            // Don't forget to cast it to string[] because the Array.Clone() returns an object.
            target.Names = (string[])Names.Clone();

            // And then deep copy the address
            //  A deep copy (which neither of these methods performs) would create a new instance of each element's object,
            //  resulting in a different, yet identical object.
            target.Address = Address.DeepCopy();
        }
    }

    public class Employee : Person, IDeepCopyable<Employee>
    {
        public int Salary;

        // Inherirted from IDeepCopyable, Note that there is no need to implment DeepCopy() in classes which inherit the interface
        public void CopyTo(Employee target)
        {
            base.CopyTo(target);
            target.Salary = Salary;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Salary)}: {Salary}";
        }
    }

    // A static class which extends the IDeepCopyable<T>
    public static class DeepCopyExtensions
    {
        public static T DeepCopy<T>(this IDeepCopyable<T> item)
          where T : new()
        {
            return item.DeepCopy();
        }

        public static T DeepCopy<T>(this T person)
          where T : Person, new()
        {
            return ((IDeepCopyable<T>)person).DeepCopy();
        }
    }

    public static class Demo
    {
        static void Main()
        {
            var john = new Employee();
            john.Names = new[] { "John", "Doe" };
            john.Address = new Address { HouseNumber = 123, StreetName = "London Road" };
            john.Salary = 321000;
            var copy = john.DeepCopy();

            copy.Names[1] = "Smith";
            copy.Address.HouseNumber++;
            copy.Salary = 123000;

            Console.WriteLine(john);
            Console.WriteLine(copy);
        }
    }
}