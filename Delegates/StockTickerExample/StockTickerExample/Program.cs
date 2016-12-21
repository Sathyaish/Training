using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTickerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // glue code

            var ram = new Person();

            var ticker = new StockTicker
            {
                Company = "Microsoft Corporation",
                Price = 20m
            };

            ticker.PriceChanged = new PriceChanged(ram.TellMeWhenPriceChanges);

            // price changed
            ticker.Price = 25m;

            decimal bonus;

            // tightly coupled code
            BonusCalculator objBonusCalculator;
            if (objBonusCalculator != null)
                bonus = objBonusCalculator(new Employee());


            Console.ReadKey();

        }

        public delegate decimal BonusCalculator(Employee e);

        decimal CalculateBonus(Employee e)
        {
            // algorithm
        }
    }

    class Person
    {
        // Subscriber
        public void TellMeWhenPriceChanges(string companyName,
            decimal oldPrice, decimal newPrice)
        {
            Console.WriteLine("The price of {0} stock changed from {1} to {2}.",
                companyName, oldPrice, newPrice);
        }
    }
}
