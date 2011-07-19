using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAI.Client;
using NAI.Client.Streaming;
using System.Windows;
using Microsoft.Surface;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;
using NAI.UI.Client;

namespace NAI.UI.Helpers
{
    public class UIHelper
    {
        //private static UIHelper _instance;
        //static public UIHelper Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new UIHelper();
        //        return _instance;
        //    }
        //    private set { _instance = value; }
        //}
        //private UIHelper() { }


        public static List<ClientIdentity> StreamingClientsOverUIElement(UIElement TargetElement)
        {
            List<ClientIdentity> clients = new List<ClientIdentity>();
            List<ClientSession> _ClientSessions = ClientSessionsController.Instance.GetClientSessionsInStreamingState();
            // foreach of the Session, do the rectangle hit test
            foreach (ClientSession cs in _ClientSessions)
            {
                try
                {
                    StreamingState ss = (StreamingState)cs.State;
                    if (ss.Visualization != null && ss.Visualization.IsOverUIElement(TargetElement))
                        clients.Add(cs.ClientId);
                }
                catch (Exception) { }                
            }
            return clients;
        }
    }

}
