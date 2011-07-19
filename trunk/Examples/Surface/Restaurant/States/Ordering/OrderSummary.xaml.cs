using System.Windows.Data;
using Microsoft.Surface.Presentation.Controls;
using System.Collections.ObjectModel;
using Restaurant.Model;

namespace Restaurant.States.Ordering
{
    /// <summary>
    /// Interaction logic for OrderSummary.xaml
    /// </summary>
    public partial class OrderSummary : ScatterViewItem
    {

        public ObservableCollection<Person> Persons
        {
            get
            {
                return Session.Instance.Persons;
            }
        }

        public OrderSummary()
        {
            InitializeComponent();
            ((ObjectDataProvider)this.Resources["Persons"]).ObjectInstance = Session.Instance.Persons;
        }
    }
}
