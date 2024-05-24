using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;

namespace BookingApp.Validation
{
    public static class ComboBoxValidation
    {
        public static bool GetIsRequired(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsRequiredProperty);
        }

        public static void SetIsRequired(DependencyObject obj, bool value)
        {
            obj.SetValue(IsRequiredProperty, value);
        }

        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.RegisterAttached("IsRequired", typeof(bool), typeof(ComboBoxValidation), new PropertyMetadata(false, OnIsRequiredChanged));

        private static void OnIsRequiredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBox comboBox && (bool)e.NewValue)
            {
                comboBox.Loaded += ComboBox_Loaded;
                comboBox.SelectionChanged += ComboBox_SelectionChanged;
            }
        }

        private static void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            Validate(sender as ComboBox);
        }

        private static void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Validate(sender as ComboBox);
        }

        private static void Validate(ComboBox comboBox)
        {
            if (comboBox != null)
            {
                bool isValid = comboBox.SelectedIndex >= 0;
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(comboBox);
                if (adornerLayer != null)
                {
                    adornerLayer.RemoveAllAdorners(comboBox);
                    if (!isValid)
                    {
                        adornerLayer.Add(new StarAdorner(comboBox));
                    }
                }
            }
        }
    }
    public static class AdornerLayerExtensions
    {
        public static void RemoveAllAdorners(this AdornerLayer adornerLayer, UIElement element)
        {
            Adorner[] adorners = adornerLayer.GetAdorners(element);
            if (adorners != null)
            {
                foreach (var adorner in adorners)
                {
                    adornerLayer.Remove(adorner);
                }
            }
        }
    }
}
