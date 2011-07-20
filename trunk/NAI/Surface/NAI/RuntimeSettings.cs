using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAI.Client.Authentication;
using System.Drawing;

namespace NAI.Properties
{

    public static class Settings 
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
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("StreamingFrameRate must be at least 1");
                }
                _streamingFrameRate = value;
            }
        }


        private static bool _LoadCalibrations = false;
        public static bool LoadCalibrations
        {
            get { return _LoadCalibrations;}
            set { _LoadCalibrations = value; }
        }

        private static bool _SimulatorMode = true;
        public static bool SimulatorMode
        {
            get { return _SimulatorMode; }
            set { _SimulatorMode = value; }
        }


        private static bool _LoadAndSaveCalibrations = true;
        public static bool LoadAndSaveCalibrations
        {
            get { return _LoadAndSaveCalibrations; }
            set { _LoadAndSaveCalibrations = value;}
        }

        private static Point _SimulatorOriginOffset;
        public static Point SimulatorOriginOffset
        {
            get 
            { 
                if (_SimulatorOriginOffset == null) 
                        _SimulatorOriginOffset = new Point(25, 101);                        
                    return _SimulatorOriginOffset;  
            }
            set
            {
                _SimulatorOriginOffset = value;   
            }
        }

        //TODO: Michael, Skal der overhovedet være en defaul value her????
        private static string _ServerCertificateSubject;
        public static string ServerCertificateSubject
        {
            get {
                if (_ServerCertificateSubject == null)
                    _ServerCertificateSubject = "CN=Test Certificate, O=ITU, E=mtho@itu.dk, L=Copenhagen, C=DK";
                    return _ServerCertificateSubject;
                }
            set { _ServerCertificateSubject = value; }
        }


        private static string _UdpToken = "NAIServer";
        public static string UdpToken
        {
            get {return _UdpToken;}
            set { 
                if (value== null || value.Length == 0)
                    throw new ArgumentException("UdpToken cannot be null, and must be at least 1 charater long");
                _UdpToken = value; }
        }






    }
}
