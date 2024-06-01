using BookingApp.Domain.Model;
using BookingApp.ViewModel;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for GuestCreatePdf.xaml
    /// </summary>
    public partial class GuestCreatePdf : Window
    {
        public int pdfChecked {  get; set; }
        public User User { get; set; }
        public GuestCreatePdfViewModel GuestCreatePdfViewModel { get; set; }

        public GuestCreatePdf(User user)
        {
            InitializeComponent();
            pdfChecked = 0;
            User = user;
            GuestCreatePdfViewModel = new GuestCreatePdfViewModel(this, User);
            DataContext = GuestCreatePdfViewModel;
            ValidateEndDate.Text = "*Select date!";
            ValidateEndDate.Visibility = Visibility.Visible;
            ValidateStartDate.Text = "*Select date!";
            ValidateStartDate.Visibility = Visibility.Visible;
            ValidateRadioButton.Text = "*Check option!";
            ValidateRadioButton.Visibility = Visibility.Visible;
        }

        private void PdfRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton.IsChecked == true)
                {
                    if (radioButton.Name == "PdfRadioButton1")
                    {
                        pdfChecked = 1;
                        ValidateRadioButton.Visibility = Visibility.Hidden;
                    }
                    else if (radioButton.Name == "PdfRadioButton2")
                    {

                        pdfChecked = 2;
                        ValidateRadioButton.Visibility = Visibility.Hidden;
                    }
                }
            }
        }
       
        private void changedEndDate(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(endDatePicker.Text) || string.IsNullOrWhiteSpace(endDatePicker.Text))
            {
                ValidateEndDate.Text = "*Select date!";
                ValidateEndDate.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                ValidateEndDate.Visibility = Visibility.Hidden;
            }
        }

        private void changedStartDate(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(startDatePicker.Text) || string.IsNullOrWhiteSpace(startDatePicker.Text))
            {
                ValidateStartDate.Text = "*Select date!";
                ValidateStartDate.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                ValidateStartDate.Visibility = Visibility.Hidden;
            }
        }
    }
}
