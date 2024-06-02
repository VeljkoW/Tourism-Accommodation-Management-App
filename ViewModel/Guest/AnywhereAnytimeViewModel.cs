using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Guest.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OwnerModel = BookingApp.Domain.Model.Owner;
using System.Windows;
using LiveCharts.Wpf;
using System.Windows.Controls;
using System.Windows.Input;
using Image = BookingApp.Domain.Model.Image;
using Acc = BookingApp.View.Guest.Pages.Accommodations;
using Any = BookingApp.View.Guest.Pages.AnywhereAnytime;
using Notification.Wpf;

namespace BookingApp.ViewModel.Guest
{
    public class AnywhereAnytimeViewModel
    {
        public User user { get; set; }
        public AnywhereAnytime AnywhereAnytime { get; set; }
        public INotificationManager notificationManager = App.GetNotificationManager();
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        ObservableCollection<Accommodation> superOwnerAccommodations { get; set; }
        ObservableCollection<Accommodation> noSuperOwnerAccommodations { get; set; }

        public List<AccommodationForReservation> accommodationForReservations { get; set; }
        public RelayCommand AccommodationTab => new RelayCommand(execute => AccommodationsTab());
        public RelayCommand AnyTab => new RelayCommand(execute => AnysTab());
        public RelayCommand StartDate => new RelayCommand(execute => StartDatePicker());
        public RelayCommand EndDate => new RelayCommand(execute => EndDatePicker());
        public RelayCommand CardsSelect => new RelayCommand(execute => SelectCard());
        public ObservableCollection<AvailableDate> printDates { get; set; }
        public RelayCommand SearchButtonClick => new RelayCommand(execute => SearchExecute());
        public AnywhereAnytimeViewModel(AnywhereAnytime anywhereAnytime) {
            AnywhereAnytime = anywhereAnytime;
            user = anywhereAnytime.User;
            Accommodations = new ObservableCollection<Accommodation>();
            superOwnerAccommodations = new ObservableCollection<Accommodation>();
            noSuperOwnerAccommodations = new ObservableCollection<Accommodation>();
            accommodationForReservations = new List<AccommodationForReservation>();
            AnywhereAnytime.ErrorLabelNoData.Visibility = System.Windows.Visibility.Visible;
            AnywhereAnytime.ErrorLabelNoSearch.Visibility = System.Windows.Visibility.Collapsed;
            //printDates = new ObservableCollection<AvailableDate>();
            //foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
            //{
            //    Image image = new Image();
            //    image = accommodation.Images[0];
            //    accommodation.Images.Clear();
            //    accommodation.Images.Add(image);
            //    SuperOwnerAccommodation(accommodation);
            //    //Accommodations.Add(accommodation);
            //}
            //AddSortAccommodations();
            //AnywhereAnytime.accommodationItems.ItemsSource = Accommodations;
        }
        public void AccommodationsTab()
        {
            Acc accommodations = new Acc(user, AnywhereAnytime.GuestMainWindow);
            AnywhereAnytime.GuestMainWindow.mainFrame.Navigate(accommodations);
        }
        public void AnysTab()
        {
            Any anywhereAnytime = new Any(user, AnywhereAnytime.GuestMainWindow);
            AnywhereAnytime.GuestMainWindow.mainFrame.Navigate(anywhereAnytime);
        }
        public void StartDatePicker()
        {
            AnywhereAnytime.CheckInDate.InputDateBox.IsDropDownOpen = true;
        }
        public void EndDatePicker()
        {
            AnywhereAnytime.CheckOutDate.InputDateBox.IsDropDownOpen = true;
        }
        private void SelectFirstCard()
        {
            var container = AnywhereAnytime.accommodationItems.ItemContainerGenerator.ContainerFromIndex(0) as ContentPresenter;
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
                if (accommodation.OwnerId == owner.Id)
                {
                    if (owner.IsSuperOwner == true) superOwnerAccommodations.Add(accommodation);
                    else if (owner.IsSuperOwner == false) noSuperOwnerAccommodations.Add(accommodation);
                }
            }
        }

        public void SearchExecute()
        {
            AnywhereAnytime.ErrorLabelNoData.Visibility = System.Windows.Visibility.Collapsed;
            AnywhereAnytime.ErrorLabelNoSearch.Visibility = System.Windows.Visibility.Collapsed;
            DateTime CheckInDate = DateTime.Now;
            DateTime CheckOutDate = DateTime.Now;
            if (!string.IsNullOrEmpty(AnywhereAnytime.CheckInDate.InputDateBox.Text) && !string.IsNullOrEmpty(AnywhereAnytime.CheckOutDate.InputDateBox.Text))
            {
                CheckInDate = Convert.ToDateTime(AnywhereAnytime.CheckInDate.InputDateBox.SelectedDate);
                CheckOutDate = Convert.ToDateTime(AnywhereAnytime.CheckOutDate.InputDateBox.SelectedDate);
            }
            int GuestNumber = 0;
            if (!string.IsNullOrEmpty(AnywhereAnytime.TextBoxGuestNumber.InputTextBox.Text) && IsNumeric(AnywhereAnytime.TextBoxGuestNumber.InputTextBox.Text))
            {
                GuestNumber = Convert.ToInt32(AnywhereAnytime.TextBoxGuestNumber.InputTextBox.Text.Trim());
                if (GuestNumber <= 0) return;
            }
            int ReservationDays = 0;
            if (!string.IsNullOrEmpty(AnywhereAnytime.TextBoxReservationDays.InputTextBox.Text) && IsNumeric(AnywhereAnytime.TextBoxReservationDays.InputTextBox.Text))
            {
                ReservationDays = Convert.ToInt32(AnywhereAnytime.TextBoxReservationDays.InputTextBox.Text.Trim());
                if (ReservationDays <= 0) return;
            }
            if(string.IsNullOrEmpty(AnywhereAnytime.CheckInDate.InputDateBox.Text) && string.IsNullOrEmpty(AnywhereAnytime.CheckOutDate.InputDateBox.Text)
                && string.IsNullOrEmpty(AnywhereAnytime.TextBoxGuestNumber.InputTextBox.Text) && string.IsNullOrEmpty(AnywhereAnytime.TextBoxReservationDays.InputTextBox.Text))
            {
                AnywhereAnytime.ErrorLabelNoData.Visibility = System.Windows.Visibility.Visible;
                return;
            }
            if (string.IsNullOrEmpty(AnywhereAnytime.CheckInDate.InputDateBox.Text) && string.IsNullOrEmpty(AnywhereAnytime.CheckOutDate.InputDateBox.Text))
            {
                AnywhereAnytime.openWindow = false;
                List<Accommodation> searchResults = SearchAccommodation(GuestNumber, ReservationDays);
                SearchResults(searchResults);
            }
            else
            {
                AnywhereAnytime.openWindow = true;
                List<Accommodation> searchResults = SearchAccommodationWithDate(CheckInDate, CheckOutDate, GuestNumber, ReservationDays);
                SearchResults(searchResults);
            }

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
            if (Accommodations.Count != 0)
            {
                AnywhereAnytime.accommodationItems.ItemsSource = Accommodations;
                AnywhereAnytime.ErrorLabelNoSearch.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                AnywhereAnytime.ErrorLabelNoSearch.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private List<Accommodation> SearchAccommodation(int GuestNumber, int ReservationDays)
        {
            List<Accommodation> accommodations = new List<Accommodation>();
            int reservationDays = ReservationDays;
            List<DateTime> availableDates = new List<DateTime>();
            int countReserved = 0;
            int countSchedule = 0;
            accommodationForReservations.Clear();
            foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
            {
                if (accommodation.MaxGuestNumber >= GuestNumber)
                {
                    countReserved = 0;
                    countSchedule = 0;
                    countReserved = ReservedAccommodationService.GetInstance().GetAll().Where(t => t.Accommodation.Id == accommodation.Id).Count();
                    countSchedule = ScheduledRenovationService.GetInstance().GetAll().Where(t => t.AccommodationId == accommodation.Id).Count();
                    if (countReserved == 0 && countSchedule == 0)
                    {
                        AccommodationForReservation accommodationForReservation = new AccommodationForReservation();
                        accommodationForReservation.AccommodationId = accommodation.Id;
                        accommodationForReservation.GuestNumber = GuestNumber;
                        for (int i = 1; i <= 5; i++)
                        {
                            AvailableDate availableDate = new AvailableDate();
                            availableDate.checkInDate = DateTime.Today.AddDays(i).AddHours(12);
                            availableDate.checkOutDate = availableDate.checkInDate.AddDays(reservationDays).AddHours(-2);
                            accommodationForReservation.AvailableDates.Add(availableDate);
                        }
                        accommodationForReservations.Add(accommodationForReservation);
                        accommodations.Add(accommodation);
                    }
                }
            }
            int countList = 0;
            foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
            {
                countList = 0;
                foreach (Accommodation a in accommodations)
                {
                    if (a.Id == accommodation.Id)
                    {
                        countList++;
                    }
                }
                if (countList == 0)
                {
                    if (accommodation.MaxGuestNumber >= GuestNumber)
                    {
                        availableDates = FindAvailableDates(DateTime.Now, DateTime.Now, reservationDays, accommodation.Id);
                        if (availableDates.Count > 0)
                        {
                            AccommodationForReservation accommodationForReservation = new AccommodationForReservation();
                            accommodationForReservation.AccommodationId = accommodation.Id;
                            accommodationForReservation.GuestNumber = GuestNumber;
                            foreach(DateTime available in availableDates)
                            {
                                AvailableDate availableDate = new AvailableDate();
                                availableDate.checkInDate = available.AddHours(12);
                                availableDate.checkOutDate = available.AddDays(reservationDays).AddHours(10);
                                accommodationForReservation.AvailableDates.Add(availableDate);
                            }
                            accommodationForReservations.Add(accommodationForReservation);
                            accommodations.Add(accommodation);
                        }
                        //foreach (DateTime availableDate in availableDates)
                        //{
                        //    AvailableDate dates = new AvailableDate();
                        //    dates.checkInDate = availableDate;
                        //    dates.checkOutDate = availableDate.AddDays(reservationDays - 1).AddHours(22);
                        //    printDates.Add(dates);
                        //}
                    }
                }
            }
            return accommodations;
        }
        private List<Accommodation> SearchAccommodationWithDate(DateTime CheckInDate, DateTime CheckOutDate, int GuestNumber, int ReservationDays)
        {
            List<Accommodation> accommodations = new List<Accommodation>();
            int reservationDays = ReservationDays;
            List<DateTime> availableDates = new List<DateTime>();
            int countReserved = 0;
            int countSchedule = 0;
            accommodationForReservations.Clear();
            foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
            {
                if (accommodation.MaxGuestNumber >= GuestNumber)
                {
                    countReserved = 0;
                    countSchedule = 0;
                    countReserved = ReservedAccommodationService.GetInstance().GetAll().Where(t => t.Accommodation.Id == accommodation.Id).Count();
                    countSchedule = ScheduledRenovationService.GetInstance().GetAll().Where(t => t.AccommodationId == accommodation.Id).Count();
                    if (countReserved == 0 && countSchedule == 0)
                    {
                        AccommodationForReservation accommodationForReservation = new AccommodationForReservation();
                        accommodationForReservation.AccommodationId = accommodation.Id;
                        accommodationForReservation.GuestNumber = GuestNumber;
                        AvailableDate availableDate = new AvailableDate();
                        availableDate.checkInDate = CheckInDate.AddHours(12);
                        availableDate.checkOutDate = CheckInDate.AddDays(reservationDays).AddHours(10);
                        accommodationForReservation.AvailableDates.Add(availableDate);
                        accommodationForReservations.Add(accommodationForReservation);
                        accommodations.Add(accommodation);
                    }
                }
            }
            int countList = 0;
            foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
            {
                countList = 0;
                foreach(Accommodation a in accommodations)
                {
                    if(a.Id == accommodation.Id)
                    {
                        countList++;
                    }
                }
                if (countList == 0)
                {
                    if (accommodation.MaxGuestNumber >= GuestNumber)
                    {
                        availableDates = FindAvailableDate(CheckInDate, CheckOutDate, reservationDays, accommodation.Id);
                        if (availableDates.Count > 0)
                        {
                            AccommodationForReservation accommodationForReservation = new AccommodationForReservation();
                            accommodationForReservation.AccommodationId = accommodation.Id;
                            accommodationForReservation.GuestNumber = GuestNumber;
                            AvailableDate availableDate = new AvailableDate();
                            availableDate.checkInDate = availableDates[0].AddHours(12);
                            availableDate.checkOutDate = availableDates[0].AddDays(reservationDays).AddHours(10);
                            accommodationForReservation.AvailableDates.Add(availableDate);
                            accommodationForReservations.Add(accommodationForReservation);

                            accommodations.Add(accommodation);
                        }
                        //foreach (DateTime availableDate in availableDates)
                        //{
                        //    AvailableDate dates = new AvailableDate();
                        //    dates.checkInDate = availableDate;
                        //    dates.checkOutDate = availableDate.AddDays(reservationDays - 1).AddHours(22);
                        //    printDates.Add(dates);
                        //}
                    }
                }
            }
            return accommodations;
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
            //var selectedCard = ((FrameworkElement)sender).DataContext as Accommodation;
            //GuestReservation guestReservation = new GuestReservation(selectedCard, user);
            //guestReservation.Show();
            //guestReservation.Focus();
        }

        private bool AreDatesAvailable(DateTime startDate, DateTime endDate, int reservationDays, int accommodationId)
        {
            if (!CheckDates(startDate, endDate, reservationDays)) return false;
            List<ReservedAccommodation> reservedAccommodations = ReservedAccommodationService.GetInstance().GetAll().Where(t => t.Accommodation.Id == accommodationId).ToList();
            List<ScheduledRenovation> scheduledRenovations = ScheduledRenovationService.GetInstance().GetAll().Where(t => t.AccommodationId == accommodationId).ToList();

            for (DateTime date = startDate; date <= startDate.AddDays(reservationDays); date = date.AddDays(1))
            {
                foreach (ReservedAccommodation reservedAccommodation in reservedAccommodations)
                    if (!CheckReservedDates(date, reservedAccommodation)) return false;

                foreach (ScheduledRenovation scheduledRenovation in scheduledRenovations)
                    if (!CheckRenovationDates(date, scheduledRenovation)) return false;
            }
            return true;
        }

        public bool CheckReservedDates(DateTime date, ReservedAccommodation reservedAccommodation)
        {
            if (date > reservedAccommodation.CheckInDate && date < reservedAccommodation.CheckOutDate) return false;
            return true;
        }

        public bool CheckRenovationDates(DateTime date, ScheduledRenovation scheduledRenovation)
        {
            if (date > scheduledRenovation.StartDate && date < scheduledRenovation.EndDate) return false;
            return true;
        }


        public bool CheckDates(DateTime startDate, DateTime endDate, int reservationDays)
        {
            if (endDate <= startDate) return false;

            if ((endDate - startDate).Days < reservationDays) return false;

            return true;
        }
        private List<DateTime> FindAvailableDates(DateTime startDate, DateTime endDate, int reservationDays, int accommodationId)
        {
            List<DateTime> availableDates = new List<DateTime>();

            DateTime currentStartDate = DateTime.Today.AddHours(0);
            // Postavljanje currentEndDate na današnji dan + reservationDays - 2 sata
            DateTime currentEndDate = DateTime.Now;

            int counterDates = 0;

            while (true)
            {
                currentStartDate = currentStartDate.AddDays(1);
                currentEndDate = currentStartDate.AddDays(reservationDays);

                if (currentStartDate == startDate.AddDays(31) || counterDates == 5)
                {
                    notificationManager.Show("Info", "In the entered date range, there are no available slots. Here are some recommended dates", NotificationType.Information);
                    break;
                } 

                if (AreDatesAvailable(currentStartDate, currentEndDate, reservationDays, accommodationId))
                {
                    availableDates.Add(currentStartDate);
                    counterDates++;
                }
            }
            return availableDates;
        }

        private List<DateTime> FindAvailableDate(DateTime startDate, DateTime endDate, int reservationDays, int accommodationId)
        {
            List<DateTime> availableDates = new List<DateTime>();

            DateTime currentStartDate = startDate, currentEndDate = endDate;

            while (currentStartDate <= currentEndDate.AddHours(2))
            {
                if (AreDatesAvailable(currentStartDate, currentEndDate.AddHours(2), reservationDays, accommodationId)) availableDates.Add(currentStartDate);

                currentStartDate = currentStartDate.AddDays(1);
            }
            return availableDates;
        }

    }
}
