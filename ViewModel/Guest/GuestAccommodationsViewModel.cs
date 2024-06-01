using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.ViewModel.Guest;
using BookingApp.Repository.AccommodationRepositories;
using System.Windows.Controls.Primitives;
using System.Windows;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Guest.Windows;
using GuestAccommodationsPage = BookingApp.View.Guest.Pages.Accommodations;
using OwnerModel = BookingApp.Domain.Model.Owner;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Windows.Controls;
using System.Windows.Input;
using Image = BookingApp.Domain.Model.Image;
using Acc = BookingApp.View.Guest.Pages.Accommodations;
using Any = BookingApp.View.Guest.Pages.AnywhereAnytime;

namespace BookingApp.ViewModel.Guest
{
    public class GuestAccommodationsViewModel
    {
        public User user { get; set; }

        public GuestAccommodationsPage GuestAccommodationsPage { get; set; }

        public ObservableCollection<Accommodation> Accommodations { get; set; }

        ObservableCollection<Accommodation> superOwnerAccommodations { get; set; }
        ObservableCollection<Accommodation> noSuperOwnerAccommodations { get; set; }

        public RelayCommand SearchButtonClick => new RelayCommand(execute => SearchExecute());

        public RelayCommand AccommodationTab => new RelayCommand(execute => AccommodationsTab());
        public RelayCommand AnyTab => new RelayCommand(execute => AnysTab());
        public RelayCommand NameBox => new RelayCommand(execute => NameSearchBox());
        public RelayCommand CardsSelect => new RelayCommand(execute => SelectCard());
        public GuestAccommodationsViewModel(GuestAccommodationsPage GuestAccommodationsPage, User user)
        {
            this.user = user;
            this.GuestAccommodationsPage = GuestAccommodationsPage;
            Accommodations = new ObservableCollection<Accommodation>();
            superOwnerAccommodations = new ObservableCollection<Accommodation>();
            noSuperOwnerAccommodations = new ObservableCollection<Accommodation>();
            GuestAccommodationsPage.ErrorLabelNoSearch.Visibility = System.Windows.Visibility.Collapsed;
            foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
            {
                Image image = new Image();
                image = accommodation.Images[0];
                accommodation.Images.Clear();
                accommodation.Images.Add(image);
                SuperOwnerAccommodation(accommodation);
                //Accommodations.Add(accommodation);
            }
            AddSortAccommodations();
            GuestAccommodationsPage.accommodationItems.ItemsSource = Accommodations;
        }
        public void AccommodationsTab()
        {
            Acc accommodations = new Acc(user, GuestAccommodationsPage.GuestMainWindow);
            GuestAccommodationsPage.GuestMainWindow.mainFrame.Navigate(accommodations);
        }
        public void AnysTab() 
        {
            Any anywhereAnytime = new Any(user, GuestAccommodationsPage.GuestMainWindow);
            GuestAccommodationsPage.GuestMainWindow.mainFrame.Navigate(anywhereAnytime);
        }
        public void NameSearchBox()
        {
            GuestAccommodationsPage.TextBoxName.InputTextBox.Focus();
        }
        private void SelectFirstCard()
        {
            var container = GuestAccommodationsPage.accommodationItems.ItemContainerGenerator.ContainerFromIndex(0) as ContentPresenter;
            if (container != null)
            {
                var contentTemplate = container.ContentTemplate;
                var textBlock = contentTemplate.FindName("BorderBlock", container) as Border;
                if (textBlock != null)
                {
                    Keyboard.Focus(textBlock); // Focus the TextBlock or other inner element
                }
            }
        }
        public void SelectCard()
        {
            SelectFirstCard();
        }
        public void AddSortAccommodations()
        {
            foreach (Accommodation superAccommodation in superOwnerAccommodations)
            {
                superAccommodation.Recommended = "Recommended";
                Accommodations.Add(superAccommodation);
            }
            foreach (Accommodation noSuperAccommodation in noSuperOwnerAccommodations) Accommodations.Add(noSuperAccommodation);
        }
        public void SuperOwnerAccommodation(Accommodation accommodation)
        {
            foreach (OwnerModel owner in OwnerService.GetInstance().GetAll())
            {
                if(accommodation.OwnerId == owner.Id)
                {
                    if (owner.IsSuperOwner == true) superOwnerAccommodations.Add(accommodation);
                    else if (owner.IsSuperOwner == false) noSuperOwnerAccommodations.Add(accommodation);
                }
            }
        }
        public AccommodationType ReturnType() 
        {
            if (GuestAccommodationsPage.ComboBoxType.SelectionBoxItem.Equals("Apartment")) return AccommodationType.Apartment;

            else if (GuestAccommodationsPage.ComboBoxType.SelectionBoxItem.Equals("House")) return AccommodationType.House;

            else return AccommodationType.Hut;
        }

        public void SearchExecute()
        {

            GuestAccommodationsPage.ErrorLabelNoSearch.Visibility = System.Windows.Visibility.Collapsed;
            string Name = "";
            string State = "";
            string City = "";
            if (!GuestAccommodationsPage.TextBoxName.InputTextBox.Text.Trim().Equals("")) Name = GuestAccommodationsPage.TextBoxName.InputTextBox.Text.Trim();

            if (!GuestAccommodationsPage.TextBoxState.InputTextBox.Text.Trim().Equals("")) State = GuestAccommodationsPage.TextBoxState.InputTextBox.Text.Trim();

            if (!GuestAccommodationsPage.TextBoxCity.InputTextBox.Text.Trim().Equals("")) City = GuestAccommodationsPage.TextBoxCity.InputTextBox.Text.Trim();

            AccommodationType? accommodationType = null;
            if (GuestAccommodationsPage.ComboBoxType.SelectedItem != null && !GuestAccommodationsPage.ComboBoxType.SelectionBoxItem.Equals("")) accommodationType = ReturnType();

            int GuestNumber = 0;
            if (!string.IsNullOrEmpty(GuestAccommodationsPage.TextBoxGuestNumber.InputTextBox.Text) && IsNumeric(GuestAccommodationsPage.TextBoxGuestNumber.InputTextBox.Text))
            {
                GuestNumber = Convert.ToInt32(GuestAccommodationsPage.TextBoxGuestNumber.InputTextBox.Text.Trim());
                if (GuestNumber <= 0) return;
            }
            int ReservationDays = 0;
            if (!string.IsNullOrEmpty(GuestAccommodationsPage.TextBoxReservationDays.InputTextBox.Text) && IsNumeric(GuestAccommodationsPage.TextBoxReservationDays.InputTextBox.Text))
            {
                ReservationDays = Convert.ToInt32(GuestAccommodationsPage.TextBoxReservationDays.InputTextBox.Text.Trim());
                if (ReservationDays <= 0) return;
            }
            List<Accommodation> searchResults = SearchAccommodation(Name, City, State, accommodationType, GuestNumber, ReservationDays);

            SearchResults(searchResults);

        }

        public void SearchResults(List<Accommodation> searchResults)
        {
            Accommodations.Clear();
            superOwnerAccommodations.Clear();
            noSuperOwnerAccommodations.Clear();
            foreach (Accommodation accommodation in searchResults)
            {
                Image image = new Image();
                image = accommodation.Images[0];
                accommodation.Images.Clear();
                accommodation.Images.Add(image);
                SuperOwnerAccommodation(accommodation);
                //Accommodations.Add(accommodation);
            }
            AddSortAccommodations();
            if (Accommodations.Count == 0)
            {
                GuestAccommodationsPage.ErrorLabelNoSearch.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {

                GuestAccommodationsPage.ErrorLabelNoSearch.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private List<Accommodation> SearchAccommodation(string Name, string City, string State,
            AccommodationType? AccommodationType, int GuestNumber, int ReservationDays)
        {
            return AccommodationService.GetInstance().GetAll().Where(accommodation =>
                (string.IsNullOrEmpty(Name) || accommodation.Name.ToLower().Contains(Name.ToLower())) &&
                (string.IsNullOrEmpty(City) || accommodation.Location.City.ToLower().Contains(City.ToLower())) &&
                (string.IsNullOrEmpty(State) || accommodation.Location.State.ToLower().Contains(State.ToLower())) &&
                (!AccommodationType.HasValue || accommodation.AccommodationType == AccommodationType.Value) &&
                (GuestNumber <= 0 || accommodation.MaxGuestNumber >= GuestNumber) &&
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
            guestReservation.Show();
            guestReservation.Focus();
        }
    }
}
