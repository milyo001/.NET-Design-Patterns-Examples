using System;
using Autofac;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Proxy.Protection
{
    public interface ICar
    {
        void Drive();
    }

    public class Car : ICar
    {
        public void Drive()
        {
            WriteLine("Car being driven");
        }
    }

    public class CarProxy : ICar
    {
        private Car car = new Car();
        private Driver driver;

        public CarProxy(Driver driver)
        {
            this.driver = driver;
        }

        // Protection proxy method
        public void Drive()
        {
            // Checks if the driver is above 16 years old to drive a car
            if (driver.Age >= 16)
                car.Drive();
            else
            {
                WriteLine("Driver too young");
            }
        }
    }

    public class Driver
    {
        public int Age { get; set; }

        public Driver(int age)
        {
            Age = age;
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var john = new Driver(12);
            ICar bmw = new CarProxy(john);
            bmw.Drive();
            // Outputs too young.

            var miro = new Driver(29);
            ICar toyota = new CarProxy(miro);
            toyota.Drive();
            // Outputs "Car being driven".
        }
    }
}