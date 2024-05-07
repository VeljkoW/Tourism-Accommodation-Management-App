using System;
using System.Collections.Generic;
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
    /// Interaction logic for NumberTextBox.xaml
    /// </summary>
    public partial class NumberTextBox : UserControl
    {
        public NumberTextBox()
        {
            InitializeComponent();
        }
        private void UpClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NumTextBox.Text))
                NumTextBox.Text = "0";
            if (int.TryParse(NumTextBox.Text, out int number))
                NumTextBox.Text = (number + 1).ToString();
        }

        private void DownClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NumTextBox.Text))
                NumTextBox.Text = "0";
            if (int.TryParse(NumTextBox.Text, out int number))
                if(number > 0)
                    NumTextBox.Text = (number - 1).ToString();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
                e.Handled = true;
        }

        private void StartClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void StopClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
