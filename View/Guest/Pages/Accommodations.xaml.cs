using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
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

namespace BookingApp.View.Guest.Pages
{
    /// <summary>
    /// Interaction logic for Accommodations.xaml
    /// </summary>
    public partial class Accommodations : Page
    {
        public GuestAccommodationsViewModel GuestAccommodationsViewModel { get; set; }

        //public GuestRate GuestRate { get; set; }
        public Accommodations(User user)
        {
            InitializeComponent();
            GuestAccommodationsViewModel = new GuestAccommodationsViewModel(this, user);
            this.DataContext = GuestAccommodationsViewModel;
        }

        private void AccommodationName_Clicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Name")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }

        }
        private void AccommodationName_NotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Name";
                textBox.Foreground = Brushes.Gray;
            }
        }
        private void AccommodationState_Clicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "State")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }

        }
        private void AccommodationState_NotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "State";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void AccommodationCity_Clicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "City")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }

        }
        private void AccommodationCity_NotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "City";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void GuestNumber_Clicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Guest Number")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }

        }
        private void GuestNumber_NotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Guest Number";
                textBox.Foreground = Brushes.Gray;
            }
        }
        private void ReservationDays_Clicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Reservation Days")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }

        }
        private void ReservationDays_NotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Reservation Days";
                textBox.Foreground = Brushes.Gray;
            }
        }

        public void ClickedOnCard(object sender, RoutedEventArgs e)
        {
            GuestAccommodationsViewModel.ClickedOnCard(sender, e);
        }
        public void Gallery(object sender, RoutedEventArgs e)
        {
            GuestAccommodationsViewModel.Gallery(sender, e);
        }

    }
}
