using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Reflection;

namespace NAI.UI.Helpers
{
    public class AnimationHelper
    {

        public static void SetSurfaceButtonIsPressed(SurfaceButton button, Boolean pressed)
        {
            typeof(SurfaceButton).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(button, new object[] { pressed });
        }

    }
}
