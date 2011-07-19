using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAI.UI.Client;
using System.Diagnostics;
using NAI.Communication.MessageLayer.Messages.Incoming;
using NAI.Client.Streaming;

namespace NAI.Client.Calibration
{
    internal class CalibrationState : PairedState
    {

        public CalibrationState(ClientSession session, ClientTagVisualization visualization)
            : base(session, visualization)
        { }

        /// <summary>
        /// Factory method which ensures that the UI is properly updated
        /// upon initialization
        /// </summary>
        /// <param name="session">The session which state is to be updated</param>
        /// <param name="visualization">The visualization of the client</param>
        public static void SetAsState(ClientSession session, ClientTagVisualization visualization)
        {
            CalibrationState cs = new CalibrationState(session, visualization);
            session.State = cs;
            cs.UpdateUI();
        }

        public override void HandleIncomingMessage(IncomingMessage message)
        {
            if (message is CalibrationAcceptedMessage)
            {
                Visualization.CalibrationAccepted();
            }
            else if (message is TouchEventMessage ||
                     message is ColorCodeMessage ||
                     message is PinCodeMessage || 
                     message is ClientPausedMessage ||
                     message is ClientResumedMessage)
            {
                // Ignored messages, but not rejected...
            }
            else
            {
                Debug.WriteLine(string.Format("Message type '{0}' is not supported at this point", message.GetType().Name));
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Callback the UI uses, when the calibration has been saved
        /// </summary>
        public void OnCalibrationSaved()
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, "CalibrationState.OnCalibrationSaved");
            StreamingState.SetAsState(_session, Visualization);
        }
    }
}
