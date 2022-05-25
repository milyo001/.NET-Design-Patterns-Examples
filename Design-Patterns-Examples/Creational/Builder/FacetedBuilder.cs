using static System.Console;

// Sometimes a single builder isn't enough (like the other examples). So let's multiple
// builders to create an object
namespace DotNetDesignPatternDemos.Creational.BuilderFacets
{
    // We are going to build a Person object
    public class Person
    {
        // We need a builder for the Address
        public string StreetAddress, Postcode, City;

        // Also for the employment
        public string CompanyName, Position;

        public int AnnualIncome;

        public override string ToString()
        {
            return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(Postcode)}: {Postcode}, {nameof(City)}: {City}, {nameof(CompanyName)}: {CompanyName}, {nameof(Position)}: {Position}, {nameof(AnnualIncome)}: {AnnualIncome}";
        }
    }

    // This is not the ordinary builder, it is a Facade for accessing other builders
    // You can check the Facade design pattern in the same repo
    public class PersonBuilder
    {
        // This property is reference to the person that is being built up. Inaccessible due to protected specifier
        protected Person person = new Person(); 

        // Here we have the first builder which will build the address of a person
        public PersonAddressBuilder Lives => new PersonAddressBuilder(person);

        // And then the second builder which will build the employment of a person
        public PersonJobBuilder Works => new PersonJobBuilder(person);

        public static implicit operator Person(PersonBuilder pb)
        {
            return pb.person;
        }
    }

    // Designed to build up the address of a person
    public class PersonJobBuilder : PersonBuilder
    {
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        public PersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            // Again we are returning the builder to allow chaining
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            // Also set the person's position and return the builder
            return this;
        }

        public PersonJobBuilder Earning(int annualIncome)
        {
            person.AnnualIncome = annualIncome;
            // Lastly set the person's annual income and return the builder
            return this;
        }
    }

    public class PersonAddressBuilder : PersonBuilder
    {
        // Might not work with a value type!
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string streetAddress)
        {
            person.StreetAddress = streetAddress;
            return this;
        }

        public PersonAddressBuilder WithPostcode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }

    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var pb = new PersonBuilder();
            
            Person person = pb
              .Lives // Note that you can access the .At(), .In() and .WithPostcode() methods of the PersonAddressBuilder after you chain .Lives() first
                .At("123 London Road")
                .In("Hamburg, Germany")
                .WithPostcode("23311")
              .Works // Note that you can access the .At and .AsA methods of the PersonJobBuilder after you chain .Works() first
                .At("Microhard Corp.")
                .AsA("Junior Developer")
                .Earning(123000);
            
            // Example two
            var person2 = pb
                .Lives
                .In("Sofia")
                .At("Schtuttgart Strasse 99")
                .Works
                .Earning(50000)
                .At("Edeka")
                .AsA("Mrejar");


            WriteLine(person);
            WriteLine(person2);
        }
    }
}
