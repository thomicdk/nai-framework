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
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using NAI.UI.Events;
using NAI.Client;
using System.Windows.Forms;

namespace WarmUp
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class MainWindow : SurfaceWindow
    {
        private Queue<string> log = new Queue<string>();
        private const byte LOG_MAX_LENGTH = 100;
        private int TouchCounter = 0;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindow()
        {
            SetupNaiFramework();
            InitializeComponent();


            // Add handlers for Application activation events
            AddActivationHandlers();


        }

        private void SetupNaiFramework()
        {
            NAI.Properties.Settings.ServerCertificateSubject = Properties.Settings.Default.NAIServerCertificateSubject;
            NAI.Properties.Settings.SimulatorMode = Properties.Settings.Default.SimulatorMode;

            /*
             * If you develop directly on the Surface unit, please remove the 
             * code snippet below.
             * If you have a screen attached to the Surface unit, this code
             * is relevant
             */
            if (!NAI.Properties.Settings.SimulatorMode)
            {
                foreach (Screen s in Screen.AllScreens)
                {
                    if (!s.Primary)
                    {
                        NAI.Properties.Settings.TabletopScreen = s;
                        break;
                    }
                }
            }
        }

        #region event handlers
        public void IdentifiedButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddTouchLogEntry(null);
        }
        public void IdentifiedButton_IdentifiedClick(object sender, RoutedIdentifiedEventArgs e)
        {
            this.AddTouchLogEntry(e.ClientId);
        }

        private void ScatterViewItem_ScatterManipulationDelta(object sender, ScatterManipulationDeltaEventArgs e)
        {
            MyIdentifiedViewPort.Moved();
        }

        private void BlockedButton_HoverOver(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            //mySecureButton.IsEnabled = true;
        }

        private void BlockedButton_HoverOut(object sender, RoutedIdentifiedHoverEventArgs e)
        {
            //mySecureButton.IsEnabled = false;
        }

        #endregion




        #region Touch Logging and Formatting

        private void AddTouchLogEntry(ClientIdentity id)
        {
            TouchCounter++;
            lock (log)
            {
                if (log.Count == LOG_MAX_LENGTH)
                    log.Dequeue();
                log.Enqueue(String.Format("{0}: {1}", TouchCounter, (id == null ? "[Unknown]" : id.Credentials.UserId)));
            }
            txtLog.Text = GetTouchLogAsString();
        }

        private String GetTouchLogAsString()
        {
            StringBuilder strB = new StringBuilder();
            lock (log)
            {
                foreach (string s in log.Reverse())
                {
                    strB.AppendLine(s);
                }
            }
            return strB.ToString();
        }
        #endregion 

        

        #region SurfaceWindow members
        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for Application activation events
            RemoveActivationHandlers();
        }

        /// <summary>
        /// Adds handlers for Application activation events.
        /// </summary>
        private void AddActivationHandlers()
        {
            // Subscribe to surface application activation events
            ApplicationLauncher.ApplicationActivated += OnApplicationActivated;
            ApplicationLauncher.ApplicationPreviewed += OnApplicationPreviewed;
            ApplicationLauncher.ApplicationDeactivated += OnApplicationDeactivated;
        }

        /// <summary>
        /// Removes handlers for Application activation events.
        /// </summary>
        private void RemoveActivationHandlers()
        {
            // Unsubscribe from surface application activation events
            ApplicationLauncher.ApplicationActivated -= OnApplicationActivated;
            ApplicationLauncher.ApplicationPreviewed -= OnApplicationPreviewed;
            ApplicationLauncher.ApplicationDeactivated -= OnApplicationDeactivated;
        }

        /// <summary>
        /// This is called when application has been activated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationActivated(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when application is in preview mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationPreviewed(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        ///  This is called when application has been deactivated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationDeactivated(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }
        #endregion
    }
}