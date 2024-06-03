using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.UserControls
{
    /// <summary>
    /// Interaction logic for ClearableDateBox.xaml
    /// </summary>
    public partial class ClearableDateBox : UserControl, INotifyPropertyChanged
    {
        public ClearableDateBox()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public string placeholder { get; set; }

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                PlaceholderTextBlock.Text = placeholder;
            }
        }
        private void InputDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(InputDateBox.Text))
                PlaceholderTextBlock.Visibility = Visibility.Visible;
            else
            {
                PlaceholderTextBlock.Visibility = Visibility.Hidden;
                InputDateBox.Foreground = Brushes.Black;
            }
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            InputDateBox.Foreground = Brushes.Transparent;
            InputDateBox.SelectedDate = null;
            InputDateBox.Focus();
        }
    }
}
