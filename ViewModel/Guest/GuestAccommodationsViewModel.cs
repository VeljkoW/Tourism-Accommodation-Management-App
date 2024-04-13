﻿using BookingApp.Domain.Model;
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
using System.Collections.ObjectModel;
using System.DirectoryServices;

namespace BookingApp.ViewModel.Guest
{
    public class GuestAccommodationsViewModel
    {
        public User user {  get; set; }

        public GuestAccommodationsPage GuestAccommodationsPage { get; set; }

        public ObservableCollection<Accommodation> Accommodations { get; set; }

        public RelayCommand SearchButtonClick => new RelayCommand(execute => SearchExecute());
        public GuestAccommodationsViewModel(GuestAccommodationsPage GuestAccommodationsPage, User user) 
        { 
            this.user = user;
            this.GuestAccommodationsPage = GuestAccommodationsPage;
            Accommodations = new ObservableCollection<Accommodation>();
            foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
            {
                Image image = new Image();
                image = accommodation.Images[0];
                accommodation.Images.Clear();
                accommodation.Images.Add(image);
                Accommodations.Add(accommodation);
            }
            GuestAccommodationsPage.accommodationItems.ItemsSource = Accommodations;
            GuestAccommodationsPage.TextBoxName.Text = "Name";
            GuestAccommodationsPage.TextBoxState.Text = "State";
            GuestAccommodationsPage.TextBoxCity.Text = "City";
            GuestAccommodationsPage.TextBoxGuestNumber.Text = "Guest Number";
            GuestAccommodationsPage.TextBoxReservationDays.Text = "Reservation Days";
        }

        public AccommodationType ReturnType() 
        {
            if (GuestAccommodationsPage.ComboBoxType.SelectionBoxItem.Equals("Apartment")) return AccommodationType.Apartment;

            else if (GuestAccommodationsPage.ComboBoxType.SelectionBoxItem.Equals("House")) return AccommodationType.House;

            else return AccommodationType.Hut;
        }

        public void SearchExecute()
        {
            string Name = "";
            string State = "";
            string City = "";
            if (!GuestAccommodationsPage.TextBoxName.Text.Trim().Equals("Name")) Name = GuestAccommodationsPage.TextBoxName.Text.Trim();

            if (!GuestAccommodationsPage.TextBoxState.Text.Trim().Equals("State")) State = GuestAccommodationsPage.TextBoxState.Text.Trim();

            if (!GuestAccommodationsPage.TextBoxCity.Text.Trim().Equals("City")) City = GuestAccommodationsPage.TextBoxCity.Text.Trim();
            
            AccommodationType? accommodationType = null;
            if (GuestAccommodationsPage.ComboBoxType.SelectedItem != null) accommodationType = ReturnType();

            int GuestNumber = 0;
            if (!string.IsNullOrEmpty(GuestAccommodationsPage.TextBoxGuestNumber.Text) && !GuestAccommodationsPage.TextBoxGuestNumber.Text.Equals("Guest Number") && IsNumeric(GuestAccommodationsPage.TextBoxGuestNumber.Text))
            {
                GuestNumber = Convert.ToInt32(GuestAccommodationsPage.TextBoxGuestNumber.Text.Trim());
                if (GuestNumber <= 0) return;
            }
            int ReservationDays = 0;
            if (!string.IsNullOrEmpty(GuestAccommodationsPage.TextBoxReservationDays.Text) && !GuestAccommodationsPage.TextBoxReservationDays.Text.Equals("Reservation Days") && IsNumeric(GuestAccommodationsPage.TextBoxReservationDays.Text))
            {
                ReservationDays = Convert.ToInt32(GuestAccommodationsPage.TextBoxReservationDays.Text.Trim());
                if (ReservationDays <= 0) return;
            }
            List<Accommodation> searchResults = SearchAccommodation(Name, City, State, accommodationType, GuestNumber, ReservationDays);

            SearchResults(searchResults);
        }

        public void SearchResults(List<Accommodation> searchResults)
        {
            foreach (Accommodation accommodation in searchResults)
            {
                Image image = new Image();
                image = accommodation.Images[0];
                accommodation.Images.Clear();
                accommodation.Images.Add(image);
                Accommodations.Add(accommodation);
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
        public void Gallery(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as Accommodation;
            ImageGallery imagegallery = new ImageGallery(selectedCard);
            imagegallery.Show();
        }
    }
}
