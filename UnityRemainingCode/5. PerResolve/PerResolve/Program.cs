using Microsoft.Practices.Unity;
using System;

namespace PerResolve
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = new UnityContainer();

                container.RegisterType<Water>(new PerResolveLifetimeManager());

                var coffeeMaker = container.Resolve<CoffeeMaker>();

                TestEquality(coffeeMaker.Water, coffeeMaker.WaterCompartment.Water);
                TestEquality(coffeeMaker.Water, coffeeMaker.CoffeePot.Water);
                TestEquality(coffeeMaker.CoffeePot.Water, coffeeMaker.WaterCompartment.Water);

                Console.WriteLine();
                var anotherCoffeeMaker = container.Resolve<CoffeeMaker>();
                TestEquality(anotherCoffeeMaker.Water, anotherCoffeeMaker.WaterCompartment.Water);
                TestEquality(anotherCoffeeMaker.Water, anotherCoffeeMaker.CoffeePot.Water);
                TestEquality(anotherCoffeeMaker.CoffeePot.Water, anotherCoffeeMaker.WaterCompartment.Water);

                Console.WriteLine();
                TestEquality(coffeeMaker.Water, anotherCoffeeMaker.Water);

                Console.WriteLine();
                var water = container.Resolve<Water>();
                TestEquality(water, coffeeMaker.Water);
                TestEquality(water, anotherCoffeeMaker.Water);
            }
            catch (ResolutionFailedException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        static void TestEquality(object o1, object o2)
        {
            Console.WriteLine("{0} instance #1 {1} {2} instance #2",
                    o1.GetType().Name,
                    o1.Equals(o2) ? "==" : "!=",
                    o2.GetType().Name);
        }
    }
}
