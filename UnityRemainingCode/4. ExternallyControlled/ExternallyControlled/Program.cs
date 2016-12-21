using Microsoft.Practices.Unity;
using System;

namespace ExternallyControlled
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            MakeWater(container);

            GC.Collect();

            var water = container.Resolve<Water>();

            GC.Collect();

            var moreWater = container.Resolve<Water>();
            TestEquality(water, moreWater);
            
            Console.ReadKey();
        }

        static void MakeWater(IUnityContainer container)
        {
            var originalWater = new Water();

            container.RegisterInstance(originalWater, new ExternallyControlledLifetimeManager());
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