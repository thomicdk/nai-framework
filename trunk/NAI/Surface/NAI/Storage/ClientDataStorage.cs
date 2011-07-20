using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;
using NAI.UI.Client;
using Microsoft.Surface.Presentation;
using System;
using NAI.Client.Calibration;
using NAI.Properties;

namespace NAI.Storage
{
    // Singleton
    internal sealed class ClientDataStorage
    {
        private const string VersionNr = "1.0";
        private const string DataPath = "Resources/Data.xml";

        private Dictionary<string, CalibrationData> _calibrations; // { get; private set; }
        
        #region Singleton members
        private static readonly ClientDataStorage _instance = new ClientDataStorage();
        public static ClientDataStorage Instance
        {
            get
            {
                return _instance;
            }            
        }

        private ClientDataStorage() 
        {
            _calibrations = new Dictionary<string, CalibrationData>();//new ClientIdentityComparer());
            LoadData();
        }
        #endregion

        #region Public members

        public void AddCalibration(TagData tag, CalibrationData calibration)
        {
            string key = KeyFromTagData(tag);
            _calibrations[key] = calibration;
            SaveData();
        }

        public void RemoveCalibration(TagData tag)
        {
            string key = KeyFromTagData(tag);
            if (_calibrations.ContainsKey(key))
            {
                _calibrations.Remove(key);
                SaveData();
            }
        }

        public CalibrationData GetClientCalibration(TagData tag)
        {
            CalibrationData result = null;
            string key = KeyFromTagData(tag);
            _calibrations.TryGetValue(key, out result);
            return result;
        }

        #endregion

        #region Private members

        private string KeyFromTagData(TagData tagData)
        {
            string key = tagData.Type.ToString();

            if (tagData.Type == TagType.Byte)
            {
                return string.Format("{0};{1}", key, tagData.Byte.Value);
            }
            else if (tagData.Type == TagType.Identity)
            {
                return string.Format("{0};{1}:{2}", key, tagData.Identity.Series, tagData.Identity.Value);
            }
            else 
            {
                throw new NotSupportedException("Cannot create key from tag data");
            }
        }

        private void LoadData()
        {
            if (!Settings.LoadCalibrations && !Settings.LoadAndSaveCalibrations) return;
            if (!File.Exists(DataPath)) return; // File does not exist
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(DataPath);
                XmlNode root = doc.DocumentElement;
                string versionNr = root.SelectSingleNode("/NAIFrameworkData/@version").Value;
                if (versionNr.Equals(ClientDataStorage.VersionNr))
                {

                    XmlNodeList clients = root.SelectNodes("/NAIFrameworkData/Clients/Client");
                    if (clients != null)
                    {
                        foreach (XmlNode client in clients)
                        {
                            // Client Identity
                            string tagType = client.SelectSingleNode("@TagType").Value;
                            string tagValue = client.SelectSingleNode("@TagValue").Value;
                            //ClientIdentity clientId = ClientHandler.Instance.GetClientIdentity(tagType, tagValue);
                            string key = string.Format("{0};{1}", tagType, tagValue);

                            // Calibration
                            string offsetInchesX = client.SelectSingleNode("Calibration/@CenterOffsetInchesX").Value;
                            string offsetInchesY = client.SelectSingleNode("Calibration/@CenterOffsetInchesY").Value;
                            string orientation = client.SelectSingleNode("Calibration/@Orientation").Value;
                            string screenWidth = client.SelectSingleNode("Calibration/@ScreenWidth").Value;
                            string screenHeight = client.SelectSingleNode("Calibration/@ScreenHeight").Value;
                            CalibrationData calibrationData = new CalibrationData(offsetInchesX, offsetInchesY, orientation, screenWidth, screenHeight);
                            _calibrations[key] = calibrationData;
                            //Debug.WriteLine(string.Format("Client ({0}) calibration loaded:{1}{2}", clientId.ToString(), Environment.NewLine, calibrationData.ToString()));
                        }
                        Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, string.Format("{0} calibration{1} loaded", clients.Count, (clients.Count != 1 ? "s" : "")));
                    }
                }
            }
            catch (XmlException) { }
        }

        private void SaveData()
        {
            if (!Settings.LoadAndSaveCalibrations) return;
            XmlTextWriter xtw = new XmlTextWriter(DataPath, Encoding.UTF8);
            xtw.Formatting = Formatting.Indented;
            xtw.WriteStartDocument();
            xtw.WriteStartElement("NAIFrameworkData");
            xtw.WriteAttributeString("version", ClientDataStorage.VersionNr);
            xtw.WriteStartElement("Clients");

            foreach (var key in _calibrations.Keys)
            {
                //ClientIdentity id = (ClientIdentity)key;
                
                xtw.WriteStartElement("Client");
                xtw.WriteAttributeString("TagType", key.Substring(0, key.IndexOf(";")));
                xtw.WriteAttributeString("TagValue", key.Substring(key.IndexOf(";") + 1));
                CalibrationData calibrationData = (CalibrationData)_calibrations[key];
                xtw.WriteStartElement("Calibration");
                xtw.WriteAttributeString("CenterOffsetInchesX", calibrationData.OffsetInches.X.ToString());
                xtw.WriteAttributeString("CenterOffsetInchesY", calibrationData.OffsetInches.Y.ToString());
                xtw.WriteAttributeString("Orientation", calibrationData.Orientation.ToString());
                xtw.WriteAttributeString("ScreenWidth", calibrationData.Width.ToString());
                xtw.WriteAttributeString("ScreenHeight", calibrationData.Height.ToString());
                xtw.WriteEndElement(); // End Calibration
                xtw.WriteEndElement(); // End Client
            }
            xtw.WriteEndElement(); // End Clients
            xtw.WriteEndElement(); // End SecureTouchData
            xtw.WriteEndDocument();
            xtw.Flush();
            xtw.Close();
        }
        #endregion
    }
}
