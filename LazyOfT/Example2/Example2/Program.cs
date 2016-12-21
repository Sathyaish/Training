using System;

namespace Example2
{
    class Program
    {
        static void Main(string[] args)
        {
            var lazyOfDog = new Lazy<Dog>(Dog.Factory);

            var dog = lazyOfDog.Value;

            dog.Name = "Woofy";

            Console.WriteLine(dog.Name);

            Console.ReadKey();
        }
    }

    class Dog
    {
        public static readonly Func<Dog> Factory = () => new Dog(null);

        public Dog(string name)
        {
            // lots of stuff happening here
            // which slows down response times
        }

        public string Name { get; set; }
    }
}
