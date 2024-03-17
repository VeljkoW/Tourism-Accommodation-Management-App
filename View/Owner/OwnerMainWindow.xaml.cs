using BookingApp.Model;
using BookingApp.Repository.AccommodationRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        public User user { get; set; }
        public Accommodation Accommodation { get; set; }
        public AccommodationRegistration AccommodationRegistration { get; set; }
        public RateGuest RateGuest { get; set; }
        public ReservedAccommodationRepository ReservedAccommodationRepository { get; set; }
        public List<ReservedAccommodation> ReservedAccommodations { get; set; }
        public OwnerMainWindow(User user)
        {
            InitializeComponent();
            this.DataContext = this;
            this.user = user;

            Accommodation = new Accommodation();
            AccommodationRegistration = new AccommodationRegistration(Accommodation, user);
            ReservedAccommodations = new List<ReservedAccommodation>();
            RateGuest = new RateGuest(this, user);
            ReservedAccommodationRepository = new ReservedAccommodationRepository();
            mainFrame.Navigate(AccommodationRegistration);
            NotificationListBox.ItemsSource = RateGuest.Update();

            if(NotificationListBox.Items.Count == 0)
            {
                NotificationListBox.BorderBrush = Brushes.Gray;
                NotificationListBox.BorderThickness = new Thickness(1);
            }
        }

        private void AccommodationReservationClick(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(AccommodationRegistration);
        }

        private void RateGuestClick(object sender, RoutedEventArgs e)
        {
            NotificationListBox.BorderBrush = Brushes.Gray;
            NotificationListBox.BorderThickness = new Thickness(1);
            mainFrame.Navigate(RateGuest);
        }
        private void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            NotificationListBox.BorderBrush = Brushes.Gray;
            NotificationListBox.BorderThickness = new Thickness(1);
            mainFrame.Navigate(RateGuest);
        }
    }
}
