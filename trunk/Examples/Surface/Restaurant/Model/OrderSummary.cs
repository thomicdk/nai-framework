using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Restaurant.Model
{
    public class OrderSummary : INotifyPropertyChanged
    {
        public OrderSummary()
        { }

        public ObservableCollection<Person> GetPersons()
        {
            return Session.Instance.Persons;    
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
