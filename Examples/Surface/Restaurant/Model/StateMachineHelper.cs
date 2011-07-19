using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restaurant.Model
{
    class StateMachineHelper
    {
        //public static States GetPreviousState(States currentState)
        //{
        //    switch (currentState)
        //    { 
        //        case States.Eating:
        //            return States.Ordering;
        //        case States.Checkout:
        //            return States.Eating;
        //        case States.Paying:
        //            return States.Checkout;
        //        default:
        //            throw new NotSupportedException("currentState not supported: " + currentState.ToString());
        //    }
        //}

        public static States GetNextState(States currentState)
        {
            switch (currentState)
            {
                case States.Ordering:
                    return States.Eating;
                case States.Eating:
                    return States.Checkout;
                case States.Checkout:
                    return States.Finished;
                default:
                    throw new NotSupportedException("currentState not supported: " + currentState.ToString());
            }
        }
    }

    public enum States : int
    {
        Ordering = 1,
        Eating = 2,
        Checkout = 3,
        Finished = 4
    }

}
