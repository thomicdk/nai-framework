using System.Windows;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;
using NAI.UI.Client;
using System.Diagnostics;
using NAI.Client;
using NAI.Client.Pairing;

namespace NAI.UI.Client
{
    /// <summary>
    /// Interaction logic for PincodeTagvisualization.xaml
    /// </summary>
    internal partial class PairingUserControl : SurfaceUserControl
    {
        private ClientTagVisualization _parent;
        private ClientSessionsController _clientHandler;
        private PairingCodeSet _pairingCodes;

        public PairingUserControl(ClientTagVisualization parent)
        {
            InitializeComponent();
            //this._clientId = clientId;
            this._parent = parent;
            this._clientHandler = ClientSessionsController.Instance;
            InitializePairing();
        }

        private void InitializePairing()
        {
            do
            {
                this._pairingCodes = PairingCodeSet.GenerateRandom();
            }
            while (!_clientHandler.RegisterPairingCodes(_parent, _pairingCodes));
            // Update UI
            this.PincodeTextBlock.Text = _pairingCodes.PinCode.Code;
            this.PinCodeShowerBottom.PinCode = _pairingCodes.PinCode.Code;
            this.PinCodeShowerTop.PinCode = _pairingCodes.PinCode.Code;
        }
    }
}
