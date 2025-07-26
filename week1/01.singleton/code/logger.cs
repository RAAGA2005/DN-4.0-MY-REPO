using System;

namespace SingletonApp
{
    
    public class Logger
    {
        // Step 1: Create a private static instance variable
        private static Logger? _instance;

        // Step 2: Make the constructor private
        private Logger()
        {
            Console.WriteLine("Logger instance created.");
        }

        // Step 3: Provide a public method to get the instance
        public static Logger GetInstance()
        {
            _instance ??= new Logger();
            return _instance;
        }

        // A method to simulate logging
        public void Log(string message)
        {
            Console.WriteLine($"[LOG]: {message}");
        }
    }

    // Program class to test the Singleton
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger1 = Logger.GetInstance();
            logger1.Log("First log message");

            Logger logger2 = Logger.GetInstance();
            logger2.Log("Second log message");

            // Check if both logger1 and logger2 refer to the same instance
            Console.WriteLine($"Are both instances the same? {ReferenceEquals(logger1, logger2)}");
        }
    }
}