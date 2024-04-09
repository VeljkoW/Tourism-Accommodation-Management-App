using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public User user { get; set; }
        public GuestRate GuestRate { get; set; }

        public GuestReservations GuestReservations { get; set; }

        public Accommodations Accommodations { get; set; }
        public GuestMainWindow(User user)
        {
            InitializeComponent();
            this.DataContext = this;
            this.user = user;
            GuestRate = new GuestRate();
            //GuestReservations = new GuestReservations(user);
            Accommodations = new Accommodations(user);
            mainFrame.Navigate(Accommodations);
        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
        }
        private void AccommodationsClick(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(Accommodations);
        }

        private void ReservationClick(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(GuestReservations);
        }

        private void ReviewsClick(object sender, RoutedEventArgs e)
        {

        }

        private void ForumClick(object sender, RoutedEventArgs e)
        {

        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        private void TutorialClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
