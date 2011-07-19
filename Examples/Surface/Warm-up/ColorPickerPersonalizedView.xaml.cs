using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.ComponentModel;
using NAI.UI.Events;

namespace WarmUp
{
    /// <summary>
    /// Interaction logic for ColorPickerPersonalizedView.xaml
    /// </summary>
    public partial class ColorPickerPersonalizedView : StackPanel
    {        
        private Ellipse _ellipse;

        public ColorPickerPersonalizedView(Ellipse ellipse)
        {
            this._ellipse = ellipse;            
            InitializeComponent();

            DependencyPropertyDescriptor widthDescriptor = DependencyPropertyDescriptor.FromProperty(StackPanel.WidthProperty, typeof(StackPanel));
            widthDescriptor.AddValueChanged(this, delegate {
                OnWidthHeightChanged();    
            });

            DependencyPropertyDescriptor heightDescriptor = DependencyPropertyDescriptor.FromProperty(StackPanel.HeightProperty, typeof(StackPanel));
            widthDescriptor.AddValueChanged(this, delegate
            {
                OnWidthHeightChanged();
            });


        }

        private void OnWidthHeightChanged()
        {

            //RotateTransform rt = (RotateTransform)this.RenderTransform;
            //rt.CenterX = this.Width / 2;
            //rt.CenterY = this.Height / 2;
        }

        
        private void ColorChange_Identified_Click(object sender, RoutedIdentifiedEventArgs e)
        {
            if (sender == btnGreen)
                _ellipse.Stroke = Brushes.Green;
            else if (sender == btnRed)
                _ellipse.Stroke = Brushes.Red;
            else if (sender == btnOrange)
                _ellipse.Stroke  = Brushes.Orange;    
        }
    }
}
