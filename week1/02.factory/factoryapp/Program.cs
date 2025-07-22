using System;

namespace FactoryMethodExample
{
    // Step 1: Product interface
    public interface IVehicle
    {
        void Drive();
    }

    // Step 2: Concrete products
    public class Car : IVehicle
    {
        public void Drive()
        {
            Console.WriteLine("Driving a car...");
        }
    }

    public class Bike : IVehicle
    {
        public void Drive()
        {
            Console.WriteLine("Riding a bike...");
        }
    }

    // Step 3: Abstract Creator (Factory)
    public abstract class VehicleFactory
    {
        public abstract IVehicle CreateVehicle();
    }

    // Step 4: Concrete Factories
    public class CarFactory : VehicleFactory
    {
        public override IVehicle CreateVehicle()
        {
            return new Car();
        }
    }

    public class BikeFactory : VehicleFactory
    {
        public override IVehicle CreateVehicle()
        {
            return new Bike();
        }
    }

    // Step 5: Client Code
    class Program
    {
        static void Main(string[] args)
        {
            VehicleFactory factory;

            Console.WriteLine("Enter vehicle type (car/bike): ");
            string? input = Console.ReadLine()?.ToLower();

            factory = input switch
            {
                "car" => new CarFactory(),
                "bike" => new BikeFactory(),
                _ => throw new ArgumentException("Invalid vehicle type entered.")
            };

            IVehicle vehicle = factory.CreateVehicle();
            vehicle.Drive();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

