using System;

namespace Example1
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 1;

            Lazy<object> lazy = new Lazy<object>(() =>
            {
                if (i % 2 == 0)
                    return new object();
                else
                    throw new DivideByZeroException("Abra ka dabra");
            });

            while(i++ < 10)
            {
                try
                {
                    object v = lazy.Value;

                    Console.WriteLine("New object");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.ReadKey();
        }
    }
}
