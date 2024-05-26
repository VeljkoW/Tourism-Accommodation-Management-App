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
    public partial class Accommodations : Page
    {
        public GuestAccommodationsViewModel GuestAccommodationsViewModel { get; set; }
        public GuestMainWindow GuestMainWindow { get; set; }
        public User User { get; set; }
        public Accommodations(User user, GuestMainWindow guestMainWindow)
        {
            InitializeComponent();
            User = user;
            GuestAccommodationsViewModel = new GuestAccommodationsViewModel(this, user);
            this.DataContext = GuestAccommodationsViewModel;
            GuestMainWindow = guestMainWindow;
        }

        //private void AccommodationName_Clicked(Object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    if (textBox.Text == "Name")
        //    {
        //        textBox.Text = string.Empty;
        //        textBox.Foreground = Brushes.Black;
        //    }

        //}
        //private void AccommodationName_NotClicked(Object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    if (string.IsNullOrWhiteSpace(textBox.Text))
        //    {
        //        textBox.Text = "Name";
        //        textBox.Foreground = Brushes.Gray;
        //    }
        //}
        //private void AccommodationState_Clicked(Object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    if (textBox.Text == "State")
        //    {
        //        textBox.Text = string.Empty;
        //        textBox.Foreground = Brushes.Black;
        //    }

        //}
        //private void AccommodationState_NotClicked(Object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    if (string.IsNullOrWhiteSpace(textBox.Text))
        //    {
        //        textBox.Text = "State";
        //        textBox.Foreground = Brushes.Gray;
        //    }
        //}

        //private void AccommodationCity_Clicked(Object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    if (textBox.Text == "City")
        //    {
        //        textBox.Text = string.Empty;
        //        textBox.Foreground = Brushes.Black;
        //    }

        //}
        //private void AccommodationCity_NotClicked(Object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    if (string.IsNullOrWhiteSpace(textBox.Text))
        //    {
        //        textBox.Text = "City";
        //        textBox.Foreground = Brushes.Gray;
        //    }
        //}

        //private void GuestNumber_Clicked(Object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    if (textBox.Text == "Guest")
        //    {
        //        textBox.Text = string.Empty;
        //        textBox.Foreground = Brushes.Black;
        //    }

        //}
        //private void GuestNumber_NotClicked(Object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    if (string.IsNullOrWhiteSpace(textBox.Text))
        //    {
        //        textBox.Text = "Guest";
        //        textBox.Foreground = Brushes.Gray;
        //    }
        //}
        //private void ReservationDays_Clicked(Object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    if (textBox.Text == "Days")
        //    {
        //        textBox.Text = string.Empty;
        //        textBox.Foreground = Brushes.Black;
        //    }

        //}
        //private void ReservationDays_NotClicked(Object sender, RoutedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    if (string.IsNullOrWhiteSpace(textBox.Text))
        //    {
        //        textBox.Text = "Days";
        //        textBox.Foreground = Brushes.Gray;
        //    }
        //}

        public void ClickedOnCard(object sender, RoutedEventArgs e)
        {
            GuestAccommodationsViewModel.ClickedOnCard(sender, e);
        }

        private void AccommodationsClick(object sender, RoutedEventArgs e)
        {
            Accommodations accommodations = new Accommodations(User, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(accommodations);
        }

        private void AnyClick(object sender, RoutedEventArgs e)
        {
            AnywhereAnytime anywhereAnytime = new AnywhereAnytime(User, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(anywhereAnytime);
        }
    }
}
