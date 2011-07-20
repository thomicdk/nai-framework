using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAI
{
    internal class DebugSettings
    {
        /// <summary>
        /// Enable/disable debugging of specific areas 
        /// of the code here.
        /// Debugging is printed to the console
        /// </summary>
        public static bool DEBUG_COMMUNICATION = true;
        public static bool DEBUG_CALIBRATION = false;
        public static bool DEBUG_EVENTS = false;
        public static bool DEBUG_PAIRING = false;
        public static bool DEBUG_HITTEST = false;
        public static bool DEBUG_STREAMING = false;
    }
}
