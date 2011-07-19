using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restaurant.Model.Menu
{
    public abstract class OrderableItem
    {

        public int Nr { get; set; }
        public string Name { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0}. {1}", Nr, Name);
            }
        }

        public string ShortName
        {
            get
            {   if (Name.Length > 20)
                {
                    return Name.Substring(0, 20);
                }
                return Name;
            }
        }

        
        public double Price { get; set; }

        public OrderableItem(int nr, string name, double price)
        {
            this.Nr = nr;
            this.Name = name;
            this.Price = price;
        }

    }
}
