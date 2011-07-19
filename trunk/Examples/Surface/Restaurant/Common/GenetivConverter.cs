using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Restaurant.Common
{
    public class GenetivConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = value.ToString();
            return string.Format("{0}'{1}", name, (name.ToLower().EndsWith("s")? "" : "s"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = value.ToString();
            return name.Substring(0, name.LastIndexOf("'"));
        }

        #endregion
    }
}
