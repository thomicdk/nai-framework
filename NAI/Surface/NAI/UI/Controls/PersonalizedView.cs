using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace NAI.UI.Controls
{
    /// <summary>
    /// Interface used when raising events
    /// </summary>
    public interface PersonalizedView
    {
        void AddPersonalizedView(UIElement view);

        void RemovePersonalizedView();
    }
}
