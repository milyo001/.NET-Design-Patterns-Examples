namespace DotNetDesignPatternDemos.Creational.Prototype.ICloneableIsBad
{
    // ICloneable is ill-specified (badly or inadequately defined)

    public class Address : ICloneable
    {
        public readonly string StreetName;
        public int HouseNumber;

        public Address(string streetName, int houseNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public override string ToString()
        {
            return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
        }

        // Inherited from ICloneable
        public object Clone()
        {
            return new Address(StreetName, HouseNumber);
        }
    }

    public class Person : ICloneable
    {
        public readonly string[] Names;
        public readonly Address Address;

        public Person(string[] names, Address address)
        {
            Names = names;
            Address = address;
        }

        public override string ToString()
        {
            return $"{nameof(Names)}: {string.Join(",", Names)}, {nameof(Address)}: {Address}";
        }

        // Inherited from ICloneable
        public object Clone()
        {
            return new Person(Names, Address);
        }
    }

    public static class Demo
    {
        static void Main()
        {
            // The problem with IClonable is that you never know if you are dealing with deep cloning or shallow cloning
            // In Deep copy, the copy of the original object and the repetitive copies both are stored.
            // And in Shallow copy, a copy of the original object is stored and only the reference address is finally copied.
            
            var john = new Person(new[] { "Miro", "Ilyovski" }, new Address("West Minster Strasse", 1234));

            // Cast to Person Clone() returns an object
            var jane = (Person)john.Clone();

            // After setting the address of Jane, the HouseNumber of John is also changed because the address of Jane is a reference to the address of John.
            jane.Address.HouseNumber = 321;

            // This doesn't work
            // var jane = john;

            // Then I've changed Address to implement IClonable, and I fixed the problem, but this is considered a bad practice.

            // But clone is typically shallow copy
            jane.Names[0] = "Jane";

            Console.WriteLine(john);
            Console.WriteLine(jane);
        }
    }
}
