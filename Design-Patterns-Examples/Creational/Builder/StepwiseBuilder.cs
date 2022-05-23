
// Stepwise Builder is builder that is constructing object by specific order
namespace StepwiseBuilder
{
    public enum CarType
    {
        Sedan,
        Crossover
    };

    public class Car
    {
        public CarType Type;
        public int WheelSize;
    }


    // When we specify the car type we return different interface ISpecifyWheelSize which has WithWheels method
    public interface ISpecifyCarType
    {
        public ISpecifyWheelSize OfType(CarType type);
    }

    // When we specify the wheel size we return different interface IBuildCar which has Build method 
    public interface ISpecifyWheelSize
    {
        public IBuildCar WithWheels(int size);
    }

    // And from IBuildCard we return the actual car. Check Interface Segregation from the SOLID principles
    public interface IBuildCar
    {
        public Car Build();
    }


    public class CarBuilder
    {
        public static ISpecifyCarType Create()
        {
            return new Impl();
        }

        private class Impl :
          ISpecifyCarType,
          ISpecifyWheelSize,
          IBuildCar
        {
            private Car car = new Car();

            public ISpecifyWheelSize OfType(CarType type)
            {
                car.Type = type;
                // The method will return ISpecifyWheelSize, the CarBuilder inherit from it
                return this;
            }

            public IBuildCar WithWheels(int size)
            {
                // I've added some validation logic here for the example to determine if wheel size is correct
                switch (car.Type)
                {
                    case CarType.Crossover when size < 17 || size > 20:
                    case CarType.Sedan when size < 15 || size > 17:
                        throw new ArgumentException($"Wrong size of wheel for {car.Type}.");
                }
                car.WheelSize = size;

                // The method will return IBuildCar, the CarBuilder inherit from it
                return this;
            }

            public Car Build()
            {
                return car;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var car = CarBuilder.Create()
              .OfType(CarType.Crossover)
              .WithWheels(18)
              .Build();

            // This won't work because builder needs to invoke .OfType() first

            //var car2 = CarBuilder.Create()
            //  .WithWheels(18)
            //  .OfType(CarType.Crossover)
            //  .Build();

            Console.WriteLine(car);
        }
    }
}
