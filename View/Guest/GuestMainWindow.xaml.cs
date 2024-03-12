using BookingApp.Model;
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

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public User user { get; set; }
        public AccommodationRepository AccommodationRepository { get; set; }
        public Accommodation Accommodation { get; set; }
        public GuestMainWindow(User user)
        {
            InitializeComponent();
            this.DataContext = this;
            this.user = user;
            Accommodation = new Accommodation();
            this.AccommodationRepository = new AccommodationRepository();
        }

        private void SearchButton(object sender, RoutedEventArgs e)
        {
            /* if (!string.IsNullOrEmpty(SearchBoxName.Text))
             {
                 foreach (Accommodation accommodation in AccommodationRepository.GetAll())
                 {
                     if (SearchBoxName.Text.ToLower().Equals(accommodation.Name.ToLower()))
                     {
                         Accommodation.Name = accommodation.Name.ToLower();
                         Accommodation.Location = accommodation.Location;
                         Accommodation.MaxGuestNumber = accommodation.MaxGuestNumber;
                         Accommodation.MinReservationDays = accommodation.MinReservationDays;
                         Accommodation.AccommodationType = accommodation.AccommodationType;

                     }
                 }
             }*/
            string Name = TextBoxName.Text.Trim();
            string State = TextBoxState.Text.Trim();
            string City = TextBoxCity.Text.Trim();
            AccommodationType? accommodationType = null;
            if (ComboBoxType.SelectedItem != null)
            {
                if (ComboBoxType.SelectionBoxItem.Equals("Apartment"))
                {
                    accommodationType = AccommodationType.Apartment;
                }
                else if(ComboBoxType.SelectionBoxItem.Equals("House"))
                {
                    accommodationType = AccommodationType.House;
                }
                else if (ComboBoxType.SelectionBoxItem.Equals("Hut"))
                {
                    accommodationType = AccommodationType.Hut;
                }
            }
            int GuestNumber = 0;
            if (!string.IsNullOrEmpty(TextBoxGuestNumber.Text))
            {
                GuestNumber = Convert.ToInt32(TextBoxGuestNumber.Text.Trim());
            }
            int ReservationDays = 0;
            if (!string.IsNullOrEmpty(TextBoxReservationDays.Text))
            {
                ReservationDays = Convert.ToInt32(TextBoxReservationDays.Text.Trim());
            }
            List<Accommodation> searchResults = SearchAccommodation(Name, City, State,
                accommodationType, GuestNumber, ReservationDays);

            PrintAccommodation.ItemsSource = searchResults;
        }

        private List<Accommodation> SearchAccommodation(string Name, string City, string State,
            AccommodationType? AccommodationType, int GuestNumber, int ReservationDays)
        {
            return AccommodationRepository.GetAll().Where(accommodation =>
                (string.IsNullOrEmpty(Name) || accommodation.Name.Contains(Name)) &&
                (string.IsNullOrEmpty(City) || accommodation.Location.City.Contains(City)) &&
                (string.IsNullOrEmpty(State) || accommodation.Location.State.Contains(State)) &&
                (!AccommodationType.HasValue || accommodation.AccommodationType == AccommodationType.Value) &&
                (GuestNumber<= 0 || accommodation.MaxGuestNumber >= GuestNumber) &&
                (ReservationDays <= 0 || accommodation.MinReservationDays <= ReservationDays)
            ).ToList();
        }
    }
}
