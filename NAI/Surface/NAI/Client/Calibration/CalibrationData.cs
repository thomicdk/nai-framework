using System;
using System.Text;
using System.Windows;

namespace NAI.Client.Calibration
{
    internal class CalibrationData
    {
        public Vector OffsetInches { get; set; }
        public double Orientation { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public CalibrationData() { }

        public CalibrationData(string offsetInchesX, string offsetInchesY, string orientation, string screenWidth, string screenHeight)
        {
            try
            {
                OffsetInches = new Vector(double.Parse(offsetInchesX), double.Parse(offsetInchesY));
                Orientation = double.Parse(orientation);
                Width = double.Parse(screenWidth);
                Height = double.Parse(screenHeight);
            }
            catch (Exception) { }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Offset: ({0},{1})", OffsetInches.X, OffsetInches.Y));
            sb.AppendLine(string.Format("Orientation: {0}", Orientation));
            sb.AppendLine(string.Format("Width: {0}", Width));
            sb.AppendLine(string.Format("Height: {0}", Height));
            return sb.ToString();
        }


    }
}
