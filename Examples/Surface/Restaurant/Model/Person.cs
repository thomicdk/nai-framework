using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Collections.ObjectModel;
using NAI.Client;

namespace Restaurant.Model
{
    public class Person : INotifyPropertyChanged
    {
        
        public States State { get; set; }
        public ClientIdentity ClientId { get; set; }

        public string Test { get; set; }

        public Person(ClientIdentity clientId)
        {
            this.ClientId = clientId;
            State = States.Ordering;
        }

        public ObservableCollection<OrderLine> OrderLines
        {
            get 
            {
                ObservableCollection<OrderLine> result = new ObservableCollection<OrderLine>(Session.Instance.OrderLines.Where(x => x.Owner.Equals(this)));
                if (result == null)
                {
                    result = new ObservableCollection<OrderLine>();
                }
                _paymentAmount = result.Sum(x => x.Item.Price);
                OnPropertyChanged("PaymentAmount");
                OnPropertyChanged("Status");
                return result;
            }
        }

        private double _paymentAmount;
        public double PaymentAmount
        {
            get 
            {
                return _paymentAmount;
            }
        }

        public string NameAndStatus
        {
            get {
                return string.Format("{0} ({1})", Name, Status);
            }
        }

        public string Name
        {   
            get{
                return ClientId.Credentials.UserId;
            }
        }



        public string Status
        {
            get
            {
                switch (Session.Instance.GlobalState)
                {
                    case States.Ordering:
                        if (State == States.Ordering)
                        {
                            if (Session.Instance.OrderLines.Count(x => x.Owner.Equals(this)) == 0)
                            {
                                return "No items ordered";
                            }
                            else
                            {
                                return "Awaiting order confirmation";
                            }
                        }
                        else if (State == States.Eating)
                            return "Order placed";
                        break;

                    case States.Eating:
                        if (State == States.Eating)
                            return "Waiting or eating";
                        break;

                    case States.Checkout:
                        if (State == States.Checkout)
                        {
                            if (PaymentAmount > 0)
                            {
                                return "Awaiting payment";
                            }
                            else
                            {
                                return "Bill settled";
                            }
                        }
                        else if (State == States.Finished)
                            return "Bill settled";
                        break;
                    default:
                        break;
                }
                throw new NotSupportedException(string.Format("This situation not supported: globalState='{0}'. person.State='{1}'.", Session.Instance.GlobalState, State.ToString()));
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
