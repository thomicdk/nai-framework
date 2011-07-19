using System;
using NAI.UI.Client;
using System.Diagnostics;
using System.Threading;
using NAI.Communication.MessageLayer.Messages.Incoming;
using NAI.Client.Calibration;

namespace NAI.Client.Streaming
{
    internal class StreamingState : PairedState
    {
        private StreamingProcessor _streamingProcessor;
        private bool _isStreamingProcessorRunning = false;

        public StreamingState(ClientSession session, ClientTagVisualization visualization)
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
            StreamingState ss = new StreamingState(session, visualization);
            session.State = ss;
            ss.UpdateUI();
        }

        public override void HandleIncomingMessage(IncomingMessage message)
        {
            if (message is TouchEventMessage)
            {
                if (Visualization != null)
                {
                    Visualization.TouchEvent(message as TouchEventMessage);
                }
            }
            else if (message is CalibrationAcceptedMessage)
            {
                if (Visualization != null)
                {
                    Visualization.CalibrationAccepted();
                }
            }
            else if (message is ClientPausedMessage)
            {
                StopStreaming();
            }
            else if (message is ClientResumedMessage)
            {
                StartStreaming();
            }
            else if (message is RequestCalibrationMessage)
            {
                Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, "RequestCalibrationMessage");
                StopStreaming();
                CalibrationState.SetAsState(_session, Visualization);
            }
            else if (message is ColorCodeMessage ||
                     message is PinCodeMessage)
            {
                // Ignored messages, but not rejected...
            }
            else
            {
                Debug.WriteLine(string.Format("Message type '{0}' is not supported at this point", message.GetType().Name));
                throw new NotSupportedException();
            }
        }

        public void StartStreaming()
        {
            if (!_isStreamingProcessorRunning && _streamingProcessor == null)
            {
                _isStreamingProcessorRunning = true;
                _streamingProcessor = new StreamingProcessor(Visualization, _session.Communication);
                new Thread(new ThreadStart(_streamingProcessor.Run)).Start();
            }
        }

        public void StopStreaming()
        {
            _isStreamingProcessorRunning = false;
            if (_streamingProcessor != null)
            {
                _streamingProcessor.KeepRunning = false;
                _streamingProcessor = null;
            }
        }
        
        public override void OnSessionEnd()
        {
            StopStreaming();
            base.OnSessionEnd();
        }

        public override void OnLostTag()
        {
            StopStreaming();
            base.OnLostTag();
        }
    }
}
