using System;
using Microsoft.Surface.Presentation.Controls;
using Restaurant.Model;

namespace Restaurant.States.Checkout
{
    /// <summary>
    /// Interaction logic for CheckOutView.xaml
    /// </summary>
    public partial class CheckoutView : ScatterView
    {
        public CheckoutView()
        {
            InitializeComponent();


        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            foreach(Person person in Session.Instance.Persons)
            {
                this.Items.Add(new Bill(person));
            }            
        }
    }
}
