using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Diagnostics;
using Restaurant.Model;
using NAI.UI.Events;

namespace Restaurant.States.Checkout
{
    /// <summary>
    /// Interaction logic for ConfirmPaymentPersonalView.xaml
    /// </summary>
    public partial class ConfirmPaymentPersonalView : SurfaceUserControl
    {



        public ConfirmPaymentPersonalView(Person owner)
        {
            InitializeComponent();
            ((ObjectDataProvider)this.Resources["Owner"]).ObjectInstance = owner;
        }

        private void Pay_Click(object sender, RoutedIdentifiedEventArgs e)
        {
            Session.Instance.Pay(e.ClientId);
            Session.Instance.NextStateForPerson(e.ClientId);
            e.ClientId.PersonalizedView.Remove();
        }
    }
}
