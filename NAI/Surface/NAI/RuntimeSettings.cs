using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAI.Client.Authentication;

namespace NAI.Properties
{

    public static class RuntimeSettings 
    {
        private static Screen _screen;
        public static Screen TabletopScreen
        {
            get
            {
                if (_screen == null)
                {
                    _screen = Screen.PrimaryScreen;
                }
                return _screen;
            }
            set
            {
                _screen = value;
            }
        }

        private static IAuthenticationHandler _authenticationHandler = new BasicAuthenticationHandler(); 
        public static IAuthenticationHandler AuthenticationHandler 
        {
            get 
            {
                if (_authenticationHandler == null)
                {
                    _authenticationHandler = new BasicAuthenticationHandler();
                }
                return _authenticationHandler;
            }
            set
            {
                _authenticationHandler = value;
            }
        }

        private static int _streamingFrameRate = 15;
        public static int StreamingFrameRate
        {
            get
            {
                return _streamingFrameRate;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("StreamingFrameRate must be at least 1");
                }
                _streamingFrameRate = value;
            }
        }


    }
}
