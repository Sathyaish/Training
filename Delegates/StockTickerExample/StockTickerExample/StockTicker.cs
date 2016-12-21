using System;

namespace StockTickerExample
{
    public class StockTicker
    {
        public string Company { get; set; }

        private decimal _price;

        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                var oldPrice = _price;

                _price = value;

                if (this.PriceChanged != null)
                    this.PriceChanged(this.Company, oldPrice, _price);
            }
        }

        public PriceChanged PriceChanged;
    }
}
