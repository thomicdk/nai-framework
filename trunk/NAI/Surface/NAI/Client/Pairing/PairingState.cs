using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Surface.Presentation;
using NAI.UI.Client;
using NAI.Client.Calibration;
using NAI.Client.Streaming;
using NAI.Communication.MessageLayer.Messages.Incoming;
using NAI.Storage;

namespace NAI.Client.Pairing
{
    internal class PairingState : ClientState
    {
        // Settings
        public static readonly byte PIN_CODE_LENGTH = 4;
        public static readonly byte COLOR_CODE_LENGTH = 0; // TODO: Set to 8 or something when COLOR_CODES are a valid input.

        public PairingState(ClientSession session)
            : base(session)
        {
            _session.Communication.SendRequestPairingCode(PIN_CODE_LENGTH, COLOR_CODE_LENGTH);
        }

        public override void HandleIncomingMessage(IncomingMessage message)
        {
            if (message is PinCodeMessage)
            {
                PinCodeMessage pincodeMessage = (message as PinCodeMessage);
                ClientTagVisualization visualizationMatch = ClientSessionsController.Instance.GetMatchForPinCode(pincodeMessage.PinCode);
                if (visualizationMatch != null)
                {
                    ClientSessionsController.Instance.UnregisterPairingCodes(visualizationMatch);
                    _session.SetClientTag(visualizationMatch.VisualizedTag);
                    _session.Communication.SendPairingCodeAccepted();
                    if (ClientDataStorage.Instance.GetClientCalibration(visualizationMatch.VisualizedTag) == null)
                    {
                        _session.State = new CalibrationState(_session, visualizationMatch);
                    }
                    else
                    {
                        _session.State = new StreamingState(_session, visualizationMatch);
                    }
                    visualizationMatch.AssociateClient(_session);
                }
                else
                {
                    _session.Communication.SendPincodeRejected();
                }
            }
            else if (message is ColorCodeMessage)
            {
                throw new NotSupportedException("COLOR_CODE not supported yet");
            }
            else if (message is ClientPausedMessage ||
                    message is ClientResumedMessage)
            {
                // Ignore, but not reject!
            }
            else
            {
                string errorMsg = string.Format("Message type '{0}' is not supported at this point", message.GetType().Name);
                Debug.WriteLine(errorMsg);
                throw new NotSupportedException(errorMsg);
            }
        }

        public override void OnSessionEnd()
        { }
    }
}
