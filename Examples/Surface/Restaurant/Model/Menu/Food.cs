﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restaurant.Model.Menu
{
    class Food : OrderableItem
    {

        public Food(int nr, string name, double price) : base(nr, name, price)
        { }

    }
}
