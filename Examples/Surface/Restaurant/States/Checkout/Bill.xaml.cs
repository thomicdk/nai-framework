using System;
using System.Windows.Data;
using Microsoft.Surface.Presentation.Controls;
using System.Collections.ObjectModel;
using Restaurant.Model;
using NAI.UI.Events;
using NAI.UI.Controls;

namespace Restaurant.States.Checkout
{
    /// <summary>
    /// Interaction logic for Bill.xaml
    /// </summary>
    public partial class Bill : ScatterViewItem
    {

        //public static readonly DependencyProperty OwnerProperty =
        //    DependencyProperty.Register(
        //    "Owner", typeof(Person), typeof(Bill));

        //public Person Owner
        //{
        //    get { return (Person)GetValue(OwnerProperty); }
        //    set { SetValue(OwnerProperty, value); }
        //}

        public Person Owner
        {
            get;
            set;
        }

        public String NameAndStatus
        {
            get
            {
                return Owner.NameAndStatus;
            }
        }

        public ObservableCollection<OrderLine> OrderLines
        {
            get
            {
                return Owner.OrderLines;
            }
        }

        public int OrderLinesCount
        {
            get
            {
                return Owner.OrderLines.Count;
            }
        }

        public Bill() {}

        public Bill(Person owner) 
        {
            this.Owner = owner;
            InitializeComponent();
            ((ObjectDataProvider)this.Resources["Owner"]).ObjectInstance = owner;
        }

        private void Transfer_Click(object sender, RoutedIdentifiedEventArgs e)
        {
            Person target = (Person)((IdentifiedSurfaceButton)sender).Tag;
            Person payer = Session.Instance.GetPerson(e.ClientId);
            if (!target.Equals(payer))
            {
                Session.Instance.PayForPerson(payer, target);
            }
        }

        private void Transfer_HoverOver(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            if (!this.Owner.ClientId.Equals(e.ClientId) && OrderLinesCount > 0)
            {
                //TransferButton.IsEnabled = true;
            }
        }

        private void Transfer_HoverOut(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            if (!this.Owner.ClientId.Equals(e.ClientId))
            {
                //TransferButton.IsEnabled = false;
            }
        }

        private void bill_ScatterManipulationDelta(object sender, ScatterManipulationDeltaEventArgs e)
        {
            MyPrivateIdentifiedViewPort.Moved();
        }
        

        // For testing...
        // Transfer to the other guy... 
        //private void IdentifiedSurfaceButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Person p = Session.Instance.Persons.FirstOrDefault(x => !x.Equals(Owner));
        //    if (p != null)
        //    {
        //        Session.Instance.PayForPerson(p, Owner);
        //    }
        //}




    }
}
