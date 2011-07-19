using System.Text;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;

namespace NAI.UI.Client
{
    /// <summary>
    /// Interaction logic for HiddenPincodeUserControl.xaml
    /// </summary>
    internal partial class HiddenPincodeUserControl : SurfaceUserControl
    {

        public string PinCode { private get; set; }

        public HiddenPincodeUserControl()
        {
            InitializeComponent();
            PinCode = "1234";
            HidePinCode();
        }

        private void ShowPinCode()
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in PinCode.ToCharArray())
            {
                sb.AppendFormat("{0} ", c);
            }
            sb.Remove(sb.Length-1,1);
            TxtCode.Text = sb.ToString();
            CodeBackground.Opacity = 0.7;
        }

        private void HidePinCode()
        {
            TxtCode.Text = "* * * *";
            TxtCode.Background = Brushes.Transparent;
            CodeBackground.Opacity = 0.4;
        }

        private void SurfaceButton_ContactDown(object sender, ContactEventArgs e)
        {
            ShowPinCode();
        }

        private void SurfaceButton_ContactUp(object sender, ContactEventArgs e)
        {
            HidePinCode();
        }
    }
}
