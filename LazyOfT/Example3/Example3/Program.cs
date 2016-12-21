using System;

namespace Example3
{
    class Program
            var dog = lazyOfDog.Value;
    {
        static void Main(string[] args)
        {
            var lazyOfDog = new Lazy<Dog>(CreateDog);



            Console.ReadKey();
        }

        static Dog CreateDog()
        {
            return new Dog("Default dog name");
        }
    }

    class Dog
    {
        public Dog(string name)
        {
            Name = name;
            Console.WriteLine("In the constructor of the Dog class...");
        }

        public string Name { get; set; }
    }

    class Lazy<T> where T: new()
    {
        private T _value;
        private Func<T> _factory;

        public Lazy()
        {

        }

        public Lazy(Func<T> func)
        {
            _factory = func;
        }

        public T Value
        {
            get
            {
                if (_value == null)
                {
                    if (_factory != null)
                        _value = _factory();
                    else
                    _value = new T();
                }

                return _value;
            }
        }
    }
}
