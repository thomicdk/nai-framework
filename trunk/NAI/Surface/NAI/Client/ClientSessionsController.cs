using System;
using Microsoft.Surface.Presentation;
using NAI.Storage;
using NAI.UI.Client;
using System.Collections.Generic;
using NAI.Communication;
using System.Diagnostics;
using System.Linq;
using NAI.Client;
using NAI.Client.Calibration;
using NAI.Client.Pairing;
using NAI.Client.Streaming;

namespace NAI.Client
{
    // Singleton
    internal sealed class ClientSessionsController 
    {
        private Dictionary<PairingCodeType, Dictionary<string, ClientTagVisualization>> _pairingCodeToVisualisationLookUp = new Dictionary<PairingCodeType, Dictionary<string, ClientTagVisualization>>();
        private List<ClientSession> _clients = new List<ClientSession>();

        // Synchronization locks
        static readonly object _pairingCodeToVisualisationDictionaryLock = new object();

        #region Singleton members
            private static readonly ClientSessionsController _instance = new ClientSessionsController();
            public static ClientSessionsController Instance
            {
                get
                {
                    return _instance;
                }            
            }
            private ClientSessionsController() 
            { }
        #endregion

        public ClientState GetClientState(TagData clientId)
        {
            ClientSession session = GetClient(clientId);
            if (session != null)
            {
                return session.State;
            }
            return null;
        }

        public void AddClient(ClientSession clientSession)
        {
            _clients.Add(clientSession);
        }

        public ClientSession GetClient(TagData tag)
        {
            foreach (ClientSession cs in _clients)
            {
                if (cs.Tag.Equals(tag))
                {
                    return cs;
                }
            }
            return null;
        }

        public List<ClientSession> GetClientsInState(Type stateType)
        {
            throw new NotImplementedException();
        }

        public List<ClientSession> GetClientSessionsInStreamingState()
        {
            List<ClientSession> _clientSessions = new List<ClientSession>();

            foreach (ClientSession cs in _clients)
            {
                if (cs.State != null && cs.State is StreamingState)                
                {
                    _clientSessions.Add(cs);
                }
            }
            return _clientSessions;
        }

        public void RemoveClient(ClientSession clientSession)
        {
            if (_clients.Contains(clientSession))
            {
                _clients.Remove(clientSession);
            }
        }

        #region Pairing Handling

        public ClientTagVisualization GetMatchForPinCode(PairingCode pairingCode)
        {
            if (pairingCode.Type == PairingCodeType.COLOR_CODE)
                throw new NotSupportedException("Color code pairing not supported yet");

            ClientTagVisualization result = null; // Assume pairing code does not match a visualization
            lock (_pairingCodeToVisualisationDictionaryLock)
            {
                if (_pairingCodeToVisualisationLookUp.ContainsKey(pairingCode.Type))
                {
                    // Get the dictionary containing the pairing codes of the given type which are currently registered in use
                    IDictionary<string, ClientTagVisualization> codesOfTypeInUse = _pairingCodeToVisualisationLookUp[pairingCode.Type];
                    if (codesOfTypeInUse.ContainsKey(pairingCode.Code))
                    {
                        result = codesOfTypeInUse[pairingCode.Code];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// When a set of pairing codes have been generated, they should be 
        /// registered, so that no one else can use them.
        /// A bool is returned indicating whether the registration went well.
        /// </summary>
        public bool RegisterPairingCodes(ClientTagVisualization visualization, PairingCodeSet codes)
        {
            bool codeNotInUse = true;
            lock (_pairingCodeToVisualisationDictionaryLock)
            {
                // Check if the pin code is already registered
                if (_pairingCodeToVisualisationLookUp.ContainsKey(codes.PinCode.Type))
                {
                    if (_pairingCodeToVisualisationLookUp[codes.PinCode.Type].ContainsKey(codes.PinCode.Code))
                    {
                        codeNotInUse = false; // Codes already in use
                    }
                }
                else
                {
                    // If there is not any Pin Codes registered
                    _pairingCodeToVisualisationLookUp.Add(codes.PinCode.Type, new Dictionary<string, ClientTagVisualization>());
                }
                if (codeNotInUse)
                {
                    _pairingCodeToVisualisationLookUp[codes.PinCode.Type].Add(codes.PinCode.Code, visualization);
                }
            }
            return codeNotInUse;
        }

        /// <summary>
        /// When a set of pairing codes are no longer needed, they should be 
        /// unregistered, so they can be used again by someone else.
        /// </summary>
        public void UnregisterPairingCodes(ClientTagVisualization visualization)
        {
            lock (_pairingCodeToVisualisationDictionaryLock)
            {
                foreach (PairingCodeType codeType in _pairingCodeToVisualisationLookUp.Keys)
                {
                    List<string> keys = new List<string>(_pairingCodeToVisualisationLookUp[codeType].Keys);
                    foreach (string key in keys)
                    {
                        if (_pairingCodeToVisualisationLookUp[codeType][key].Equals(visualization))
                        {
                            _pairingCodeToVisualisationLookUp[codeType].Remove(key);
                        }
                    }
                }
            }
        }

        public bool IsPairingCodeInUse(PairingCodeType type, string code)
        {
            bool result = false;
            lock (_pairingCodeToVisualisationDictionaryLock)
            {
                if (_pairingCodeToVisualisationLookUp.ContainsKey(type))
                {
                    if (_pairingCodeToVisualisationLookUp[type].ContainsKey(code))
                    {
                        result = true; // Code already in use
                    }
                }
            }
            return result;
        }

        #endregion 
    }
}
