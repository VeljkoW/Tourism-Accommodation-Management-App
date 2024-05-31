using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Guest.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guest
{
    public class AnywhereAnytimeWithoutDateViewModel
    {
        private int currentImageIndex = 0;
        public User user { get; set; }

        public ObservableCollection<string> ImagePaths { get; set; }
        public AnywhereAnytimeWithoutDate AnywhereAnytimeWithoutDate { get; set; }
        public ReservedAccommodation reservedAccommodation { get; set; }
        public Accommodation accommodation { get; set; }

        public AnywhereAnytimeViewModel AnywhereAnytimeViewModel { get; set; }
        public AccommodationForReservation AccommodationForReservation { get; set; }
        public string CurrentImagePath => ImagePaths.ElementAtOrDefault(CurrentImageIndex);
        public int TotalImages => ImagePaths.Count;

        public RelayCommand ReservationClickButton => new RelayCommand(execute => ReservationClick(), canExecute => CanReservationClick());

        public AnywhereAnytimeWithoutDateViewModel(AnywhereAnytimeViewModel anywhereAnytimeViewModel, AnywhereAnytimeWithoutDate anywhereAnytimeWithoutDate, AccommodationForReservation accommodationForReservation)
        {
            ImagePaths = new ObservableCollection<string>();
            this.AnywhereAnytimeWithoutDate = anywhereAnytimeWithoutDate;
            user = AnywhereAnytimeWithoutDate.user;
            AnywhereAnytimeViewModel = anywhereAnytimeViewModel;
            AccommodationForReservation = accommodationForReservation;
            reservedAccommodation = new ReservedAccommodation();
            accommodation = AccommodationService.GetInstance().GetById(accommodationForReservation.AccommodationId);
            AnywhereAnytimeWithoutDate.AccommodationName.Content += "Accommodation: " + accommodation.Name + ", " + accommodation.Location.State + " - " + accommodation.Location.City;
            foreach (Image image in accommodation.Images)
                ImagePaths.Add(image.Path);
            AnywhereAnytimeWithoutDate.AvailableDates.ItemsSource = AccommodationForReservation.AvailableDates;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

        public int CurrentImageIndex
        {
            get { return currentImageIndex; }
            set
            {
                if (value >= 0 && value < ImagePaths.Count)
                {
                    currentImageIndex = value;
                    OnPropertyChanged(nameof(CurrentImageIndex));
                }
            }
        }
        public void NextImage(object sender, RoutedEventArgs e)
        {
            if (CurrentImageIndex < TotalImages - 1)
            {
                CurrentImageIndex++;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }

        public void PreviousImage(object sender, RoutedEventArgs e)
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }

        public bool CanReservationClick()
        {
            if (AnywhereAnytimeWithoutDate.AvailableDates.SelectedValue == null) return false;
            return true;
        }
        public void ReservationClick()
        {
            string? selectedDate = AnywhereAnytimeWithoutDate.AvailableDates.SelectedValue.ToString();
            string[] dates = selectedDate.Split('-');
            reservedAccommodation.CheckInDate = Convert.ToDateTime(dates[0].Trim());
            reservedAccommodation.CheckOutDate = Convert.ToDateTime(dates[1].Trim());
            reservedAccommodation.Accommodation = accommodation;
            reservedAccommodation.GuestId = user.Id;
            foreach (Image image in accommodation.Images) reservedAccommodation.Images.Add(image);
            reservedAccommodation.GuestNumber = AccommodationForReservation.GuestNumber;

            foreach (GuestBonus guestBonus in GuestBonusService.GetInstance().GetAll())
            {
                if (guestBonus.GuestId == user.Id && guestBonus.Bonus > 0)
                {
                    guestBonus.Bonus--;
                    GuestBonusService.GetInstance().Update(guestBonus);
                    break;
                }
            }
            ReportOnReservations reportOnReservations = new ReportOnReservations();
            reportOnReservations.GuestId = user.Id;
            reportOnReservations.AccommodationId = reservedAccommodation.Accommodation.Id;
            reportOnReservations.Date = DateTime.Now;
            reportOnReservations.TypeReport = "Reserved";
            ReportOnReservationsService.GetInstance().Add(reportOnReservations);
            ReservedAccommodationService.GetInstance().Add(reservedAccommodation);
            AnywhereAnytimeViewModel.SearchExecute();
            AnywhereAnytimeWithoutDate.Close();
        }

    }
}
