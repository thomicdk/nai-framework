using Microsoft.Surface.Presentation.Controls;
using Restaurant.Model;
using NAI.UI.Events;

namespace Restaurant.States.Eating
{
    /// <summary>
    /// Interaction logic for GoToPaymentPersonalView.xaml
    /// </summary>
    public partial class GoToPaymentPersonalView : SurfaceUserControl
    {
        public GoToPaymentPersonalView()
        {
            InitializeComponent();
        }

        private void Checkout_Click(object sender, RoutedIdentifiedEventArgs e)
        {
            Session.Instance.NextStateForPerson(e.ClientId);
            e.ClientId.PersonalizedView.Remove();
        }
    }
}
