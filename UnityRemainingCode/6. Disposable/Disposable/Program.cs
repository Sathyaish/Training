using Microsoft.Practices.Unity;
using System;

namespace Disposable
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            
            container.RegisterType<Foo>(new ContainerControlledLifetimeManager());

            var foo = container.Resolve<Foo>();
            
            container.Dispose();

            Console.ReadKey();
        }
    }
    
    class Foo : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("Disposing foo...");
        }
    }
}