using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Guest.Windows;
using Notification.Wpf;
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
    public class AnywhereAnytimeWithDateViewModel : INotifyPropertyChanged
    {
        private int currentImageIndex = 0;
        public INotificationManager notificationManager = App.GetNotificationManager();
        public User user { get; set; }

        public ObservableCollection<string> ImagePaths { get; set; }
        public AnywhereAnytimeWithDate anywhereAnytimeWithDate {  get; set; }
        public ReservedAccommodation reservedAccommodation { get; set; }
        public Accommodation accommodation {  get; set; }

        public AnywhereAnytimeViewModel AnywhereAnytimeViewModel { get; set; }
        public AccommodationForReservation AccommodationForReservation { get; set; }
        public string CurrentImagePath => ImagePaths.ElementAtOrDefault(CurrentImageIndex);
        public int TotalImages => ImagePaths.Count;

        public RelayCommand ReservationClickButton => new RelayCommand(execute => ReservationClick());

        public AnywhereAnytimeWithDateViewModel(AnywhereAnytimeViewModel anywhereAnytimeViewModel, AnywhereAnytimeWithDate anywhereAnytimeWithDate, AccommodationForReservation accommodationForReservation)
        {
            ImagePaths = new ObservableCollection<string>();
            this.anywhereAnytimeWithDate = anywhereAnytimeWithDate;
            user = anywhereAnytimeWithDate.user;
            AnywhereAnytimeViewModel = anywhereAnytimeViewModel;
            AccommodationForReservation = accommodationForReservation;
            reservedAccommodation = new ReservedAccommodation();
            accommodation = AccommodationService.GetInstance().GetById(accommodationForReservation.AccommodationId);
            anywhereAnytimeWithDate.AccommodationName.Content += "Accommodation: " + accommodation.Name + ", " + accommodation.Location.State + " - " + accommodation.Location.City + "\n" + accommodationForReservation.AvailableDates[0].checkInDate.ToString() + " - " + accommodationForReservation.AvailableDates[0].checkOutDate.ToString();
            foreach (Image image in accommodation.Images)
                ImagePaths.Add(image.Path);
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

        public void ReservationClick()
        {
            reservedAccommodation.CheckInDate = AccommodationForReservation.AvailableDates[0].checkInDate;
            reservedAccommodation.CheckOutDate = AccommodationForReservation.AvailableDates[0].checkOutDate;
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
            anywhereAnytimeWithDate.Close();
            notificationManager.Show("Success", "Accommodation Successfully reserved!", NotificationType.Success);
        }
    }
}
