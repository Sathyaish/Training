namespace PerResolve
{
    class Water { }

    class WaterCompartment 
    {
        public Water Water { get; set; }

        public WaterCompartment(Water water)
        {
            Water = water;
        }
    }

    class CoffeePot 
    {
        public Water Water { get; set; }

        public CoffeePot(Water water)
        {
            Water = water;
        }
    }

    class CoffeeMaker
    {
        public Water Water { get; set; }
        public WaterCompartment WaterCompartment { get; set; }
        public CoffeePot CoffeePot { get; set; }
        
        public CoffeeMaker(WaterCompartment waterCompartment, CoffeePot coffeePot, 
            Water water)
        {
            WaterCompartment = waterCompartment;
            CoffeePot = coffeePot;
            Water = water;
        }
    }
}