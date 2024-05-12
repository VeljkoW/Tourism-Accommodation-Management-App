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
    /// Interaction logic for ClearableTextBox.xaml
    /// </summary>
    public partial class ClearableTextBox : UserControl, INotifyPropertyChanged
    {
        public ClearableTextBox()
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

        public string placeholder {  get; set; }

        public string Placeholder
        {
            get { return placeholder; }
            set 
            {
                placeholder = value;
                PlaceholderTextBlock.Text = placeholder;
            }
        }


        /*private void ClearClick(object sender, RoutedEventArgs e)
        {
            InputTextBox.Clear();
            InputTextBox.Focus();
        }*/

        private void InputTextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(InputTextBox.Text))
                PlaceholderTextBlock.Visibility = Visibility.Visible;
            else
                PlaceholderTextBlock.Visibility = Visibility.Hidden;

        }
    }
}
