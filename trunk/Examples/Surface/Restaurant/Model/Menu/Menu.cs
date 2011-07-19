using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Restaurant.Model.Menu
{
    public class Menu
    {

        private static ObservableCollection<OrderableItem> _items;

        public static ObservableCollection<OrderableItem> Items 
        { 
            get
            {
                if (_items == null)
                {
                    _items = new ObservableCollection<OrderableItem>(GenerateItems());
                }
                return _items;
            }      
        }

        public IEnumerable<OrderableItem> GetFoodItems()
        {
            return GenerateItems().Where(x => x.GetType().Equals(typeof(Food)));
        }

        public IEnumerable<OrderableItem> GetBeverageItems()
        {
            return GenerateItems().Where(x => x.GetType().Equals(typeof(Beverage)));
        }

        private static List<OrderableItem> GenerateItems()
        {
            List<OrderableItem> items = new List<OrderableItem>();
            items.Add(new Food(1, "Shrimp Cocktail", 80));
            items.Add(new Food(2, "Mixed Green Salad", 60));
            items.Add(new Food(3, "New York Strip (200 g)", 150));
            items.Add(new Food(4, "Grilled Chicken Breast (250 g)", 120));

            items.Add(new Beverage(20, "Carlsberg Beer", 30));
            items.Add(new Beverage(21, "Coca Cola", 25));
            return items;
        }



    }
}
