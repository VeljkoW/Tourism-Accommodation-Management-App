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
            AccommodationRegistration = new AccommodationRegistration(Accommodation);
            ReservedAccommodations = new List<ReservedAccommodation>();
            RateGuest = new RateGuest(user);
            ReservedAccommodationRepository = new ReservedAccommodationRepository();

            mainFrame.Navigate(AccommodationRegistration);

            DateTime yesterday = DateTime.Now.AddDays(-1).Date;
            foreach(ReservedAccommodation reservedAccommodation in ReservedAccommodationRepository.GetAll())
            {
                if ((DateTime.Now > reservedAccommodation.checkOutDate) &&
                           (DateTime.Now - reservedAccommodation.checkOutDate).Days <= 5)
                {
                    ReservedAccommodations.Add(reservedAccommodation);
                }
            }
            NotificationListBox.ItemsSource = ReservedAccommodations;
            //mainFrame.Navigate(new Uri("../../../View/Owner/AccommodationRegistration.xaml", UriKind.Relative));
        }

        private void AccommodationReservationClick(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(AccommodationRegistration);
        }

        private void RateGuestClick(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(RateGuest);
        }


        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("../../../View/Owner/RateGuest.xaml", UriKind.Relative));
        }*/
    }
}
