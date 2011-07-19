using Microsoft.Surface.Presentation.Controls;
using Restaurant.Model;
using NAI.UI.Events;

namespace Restaurant.States.Ordering
{
    /// <summary>
    /// Interaction logic for ConfirmOrderPersonalView.xaml
    /// </summary>
    public partial class ConfirmOrderPersonalView : SurfaceUserControl
    {
        public ConfirmOrderPersonalView()
        {
            InitializeComponent();
        }

        private void ConfirmOrder_Click(object sender, RoutedIdentifiedEventArgs e)
        {
            Session.Instance.NextStateForPerson(e.ClientId);
            e.ClientId.PersonalizedView.Remove();
        }
    }
}
