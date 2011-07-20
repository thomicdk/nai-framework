using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using NAI.Client;
using Restaurant.Model.Menu;

namespace Restaurant.Model
{
    public class Session
    {
        private Restaurant.MainWindow _restaurantView;

        public States GlobalState { get; private set; }
        public ObservableCollection<Person> Persons { get; private set; }
        public List<OrderLine> OrderLines { get; private set; }

        #region Singleton members
        private static readonly Session _instance = new Session();
        public static Session Instance
        {
            get
            {
                return _instance;
            }            
        }

        private Session()
        {
            GlobalState = States.Ordering;
            Persons = new ObservableCollection<Person>();
            OrderLines = new List<OrderLine>();
        }
        #endregion

        public void SetRestaurantView(MainWindow restaurant)
        {
            this._restaurantView = restaurant;
        }


        public bool HasPerson(ClientIdentity clientId)
        {
            return Persons.FirstOrDefault(x => x.ClientId.Equals(clientId)) != null;
        }

        public Person GetPerson(ClientIdentity clientId)
        {
            Person p = Persons.FirstOrDefault(x => x.ClientId.Equals(clientId));
            if (p == null)
            {
                p = new Person(clientId);
                Persons.Add(p);
            }
            return p;
        }

        public void Reset()
        {
            OrderLines.Clear();
            GlobalState = States.Ordering;
            this._restaurantView.GoToState(this.GlobalState);
        }


        #region State: Ordering

        /// <summary>
        /// Used when a person wants to order an item from the menu
        /// </summary>
        public void OrderOneItem(Person person, OrderableItem item)
        {
            if (person.State != States.Ordering)
            {
                person.State = States.Ordering;
                person.OnPropertyChanged("Status");
            }

            OrderLines.Add(new OrderLine(person, item));
            person.OnPropertyChanged("OrderLines");
        }

        /// <summary>
        /// Used when a person wants to remove an item he has ordered
        /// </summary>
        public void RemoveOneOrderedItem(Person person, OrderableItem item)
        {
            OrderLine itemToRemove = OrderLines.FirstOrDefault(x => x.Owner.Equals(person) && x.Item.Equals(item));
            if (itemToRemove != null)
            {
                if (person.State != States.Ordering)
                {
                    person.State = States.Ordering;
                    person.OnPropertyChanged("Status");
                }

                OrderLines.Remove(itemToRemove);
                person.OnPropertyChanged("OrderLines");
            }
            
        }

        #endregion

        #region State: Checkout



        /// <summary>
        /// Used when a person wants to pay for everything another person has ordered
        /// </summary>
        public void PayForPerson(Person payer, Person target)
        {
            if (!payer.Equals(target) && target.State == States.Checkout)
            {
                if (payer.State != States.Checkout)
                {
                    payer.State = States.Checkout;
                    payer.OnPropertyChanged("Status");
                }

                foreach (OrderLine line in OrderLines.Where(x => x.Owner.Equals(target)))
                {
                    line.PayForAll(payer);
                }
                payer.OnPropertyChanged("OrderLines");
                target.OnPropertyChanged("OrderLines");
            }
        }

        /// <summary>
        /// Used when a person transfers an orderline to himself
        /// </summary>
        public void TransferOrderLine(Person newOwner, OrderLine orderLine)
        {
            if (newOwner.State == States.Checkout && orderLine.Owner.State == States.Checkout)
            {
                orderLine.PayForAll(newOwner);
                //orderLine.TransferOwnerShip(newOwner);
            }
        }

        /// <summary>
        /// Used when a person take share paying for an item
        /// </summary>
        //public void TakeShareInOrderLine(Person shareTaker, OrderLine orderLine, float share)
        //{
        //    if (shareTaker.State == States.Checkout && orderLine.Owner.State == States.Checkout)
        //    {
        //        orderLine.AddPayer(shareTaker, share);
        //    }
        //}

        public void Pay(ClientIdentity clientId)
        {
            Person p = GetPerson(clientId);
            OrderLines.RemoveAll(x => x.Owner.Equals(p));
            p.OnPropertyChanged("OrderLines");
            p.OnPropertyChanged("Status");
        }


        #endregion


        #region State: 'ANY'

        public void AddPerson(Person person)
        {
            Persons.Add(person);
        }


        public void RemovePerson(ClientIdentity clientId)
        {
            RemovePerson(GetPerson(clientId));
        }

        public void RemovePerson(Person person)
        {
            Persons.Remove(person);
            OrderLines.RemoveAll(x => x.Owner.Equals(person));
        }



        /// <summary>
        /// Used when a person pushed the confirm button,
        /// either for confirming an order or a bill
        /// </summary>
        public void NextStateForPerson(Person person, States newState)
        {
            person.State = newState;
            if (Persons.Count(x => x.State.Equals(person.State)) == Persons.Count ||
                (GlobalState == States.Checkout && OrderLines.Count == 0))
            {
                NextGlobalState();
            }
            person.OnPropertyChanged("Status");
        }

        public void NextStateForPerson(Person person)
        {
            States newState = StateMachineHelper.GetNextState(person.State);
            NextStateForPerson(person, newState);
        }

        public void NextStateForPerson(ClientIdentity clientId)
        {
            NextStateForPerson(GetPerson(clientId));
        }

        /// <summary>
        /// Used when a person wants to edit something that he has 
        /// already confirmed.
        /// </summary>
        //public void PreviousStateForPerson(Person person)
        //{
        //    States previousState = StateMachineHelper.GetPreviousState(person.State);
        //    if ((int)previousState < (int)GlobalState)
        //    {
        //        throw new Exception("Cannot go to this state");
        //    }
        //    person.State = previousState;
        //}


        public void NextGlobalState()
        {
            this.GlobalState = StateMachineHelper.GetNextState(this.GlobalState);
            this._restaurantView.GoToState(this.GlobalState);
            Debug.WriteLine(string.Format("Changing global state to '{0}'", this.GlobalState));
        }

        #endregion




    }

 

}
