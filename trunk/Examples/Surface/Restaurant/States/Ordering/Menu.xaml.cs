using Microsoft.Surface.Presentation.Controls;
using Restaurant.Model;
using Restaurant.Model.Menu;
using NAI.UI.Events;

namespace Restaurant.States.Ordering
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : ScatterViewItem
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void AddItem(object sender, RoutedIdentifiedEventArgs e)
        {
            if (sender is SurfaceButton)
            {
                Person p = Session.Instance.GetPerson(e.ClientId);
                //Console.WriteLine("Add: " + ((SurfaceButton)sender).Tag.ToString());
                Session.Instance.OrderOneItem(p, (OrderableItem)((SurfaceButton)sender).Tag);
            }
            
        }

        private void RemoveItem(object sender, RoutedIdentifiedEventArgs e)
        {
            if (sender is SurfaceButton)
            {
                Person p = Session.Instance.GetPerson(e.ClientId);
                //Console.WriteLine("Remove: " + ((SurfaceButton)sender).Tag.ToString());
                Session.Instance.RemoveOneOrderedItem(p, (OrderableItem)((SurfaceButton)sender).Tag);
            }
        }



        private void OrderOne_HoverOver(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            //((IdentifiedSurfaceButton)e.Source).IsEnabled = true;
        }

        private void OrderOne_HoverOut(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            //((IdentifiedSurfaceButton)e.Source).IsEnabled = false;
        }


        private void CancelOne_HoverOver(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            //((IdentifiedSurfaceButton)e.Source).IsEnabled = true;
        }

        private void CancelOne_HoverOut(object sender, RoutedIdentifiedHoverEventArgs e)
        {
           // ((IdentifiedSurfaceButton)e.Source).IsEnabled = false;
        }

    }
}
