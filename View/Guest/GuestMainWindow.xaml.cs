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
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public User user { get; set; }
        public Image image { get; set; }
        public AccommodationRepository AccommodationRepository { get; set; }
        public Accommodation Accommodation { get; set; }

        public List<Accommodation> Accommodations { get; set; }
        public GuestMainWindow(User user)
        {
            InitializeComponent();
            this.DataContext = this;
            this.user = user;
            Accommodation = new Accommodation();
            Accommodations = new List<Accommodation>();
            this.AccommodationRepository = new AccommodationRepository();
            foreach (Accommodation accommodation in AccommodationRepository.GetAll())
            {
                Image image = new Image();
                image = accommodation.Images[0];
                accommodation.Images.Clear();
                accommodation.Images.Add(image);
                Accommodations.Add(accommodation);
            }
            accommodationItems.ItemsSource = Accommodations;
            TextBoxName.Text = "Name";
            TextBoxState.Text = "State";
            TextBoxCity.Text = "City";
            TextBoxGuestNumber.Text = "Guest Number";
            TextBoxReservationDays.Text = "Reservation Days";
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
            string Name="";
            string State="";
            string City="";
            if (!TextBoxName.Text.Trim().Equals("Name"))
            {
                Name = TextBoxName.Text.Trim();
            }
            if (!TextBoxState.Text.Trim().Equals("State"))
            {
                State = TextBoxState.Text.Trim();
            }
            if (!TextBoxCity.Text.Trim().Equals("City"))
            {
                City = TextBoxCity.Text.Trim();
            }
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
            if (!string.IsNullOrEmpty(TextBoxGuestNumber.Text) && !TextBoxGuestNumber.Text.Equals("Guest Number") && IsNumeric(TextBoxGuestNumber.Text))
            {
                GuestNumber = Convert.ToInt32(TextBoxGuestNumber.Text.Trim());
                if (GuestNumber <= 0)
                {
                    return;
                }
            }
            int ReservationDays = 0;
            if (!string.IsNullOrEmpty(TextBoxReservationDays.Text) && !TextBoxReservationDays.Text.Equals("Reservation Days") && IsNumeric(TextBoxReservationDays.Text))
            {
                ReservationDays = Convert.ToInt32(TextBoxReservationDays.Text.Trim());
                if (ReservationDays <= 0)
                {
                    return;
                }
            }
            List<Accommodation> searchResults = SearchAccommodation(Name, City, State,
                accommodationType, GuestNumber, ReservationDays);

            foreach (Accommodation accommodation in searchResults)
            {
                Image image = new Image();
                image = accommodation.Images[0];
                accommodation.Images.Clear();
                accommodation.Images.Add(image);
                Accommodations.Add(accommodation);
            }
            accommodationItems.ItemsSource = searchResults;
        }

        private List<Accommodation> SearchAccommodation(string Name, string City, string State,
            AccommodationType? AccommodationType, int GuestNumber, int ReservationDays)
        {
            return AccommodationRepository.GetAll().Where(accommodation =>
                (string.IsNullOrEmpty(Name) || accommodation.Name.ToLower().Contains(Name.ToLower())) &&
                (string.IsNullOrEmpty(City) || accommodation.Location.City.ToLower().Contains(City.ToLower())) &&
                (string.IsNullOrEmpty(State) || accommodation.Location.State.ToLower().Contains(State.ToLower())) &&
                (!AccommodationType.HasValue || accommodation.AccommodationType == AccommodationType.Value) &&
                (GuestNumber<= 0 || accommodation.MaxGuestNumber >= GuestNumber) &&
                (ReservationDays <= 0 || accommodation.MinReservationDays <= ReservationDays)
            ).ToList();
        }
        public bool IsNumeric(string text)
        {
            try
            {
                int number = int.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ClickedOnCard(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as Accommodation;
            GuestReservation guestReservation = new GuestReservation(selectedCard, user);
            guestReservation.Owner = this;
            guestReservation.Show();
        }
        public void Gallery(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as Accommodation;
            ImageGallery imagegallery = new ImageGallery(selectedCard);
            imagegallery.Owner = this;
            imagegallery.Show();
        }
    }
}
