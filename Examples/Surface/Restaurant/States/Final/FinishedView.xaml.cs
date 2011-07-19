using System.Windows;
using Microsoft.Surface.Presentation.Controls;
using Restaurant.Model;

namespace Restaurant.States.Final
{
    /// <summary>
    /// Interaction logic for FinishedView.xaml
    /// </summary>
    public partial class FinishedView : SurfaceUserControl
    {
        public FinishedView()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Session.Instance.Reset();
        }
    }
}
