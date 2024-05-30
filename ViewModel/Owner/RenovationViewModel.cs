using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Owner;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Schema;

namespace BookingApp.ViewModel.Owner
{
    public class RenovationViewModel
    {
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";
        public INotificationManager notificationManager = App.GetNotificationManager();
        public RelayCommand SearchClick => new RelayCommand(execute => SearchExecute(), canExecute => SearchCanExecute());
        public RelayCommand RenovateClick => new RelayCommand(execute => RenovateExecute(), canExecute => RenovateCanExecute());
        //public RelayCommand DeleteRowCommand => new RelayCommand(canExecute => CanDeleteRowExecute());
        //public RelayCommand CancelCommand => new RelayCommand(execute => CancelExecute());
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public ObservableCollection<AvailableDate> AvailableDates { get; set; }
        public Renovation Renovation { get; set; }
        public User User { get; set; }
        public ScheduledRenovation SelectedScheduledRenovation {  get; set; }
        public ObservableCollection<ScheduledRenovation> ScheduledRenovations {  get; set; }
        public RenovationViewModel(Renovation renovation)
        {
            Renovation = renovation;
            User = Renovation.User;
            AvailableDates = new ObservableCollection<AvailableDate>();
            Accommodations = new ObservableCollection<Accommodation>();
            ScheduledRenovations = new ObservableCollection<ScheduledRenovation>();
            ObservableCollection<Accommodation> allAccommodations = AccommodationService.GetInstance().GetAllByUser(User);
            Accommodations = ScheduledRenovationService.GetInstance().GetUnrenovatedAccommodations(allAccommodations);
            ScheduledRenovationService.GetInstance().UpdateUpcomingRenovations(User, ScheduledRenovations);

            Renovation.CommentTextBox.IsEnabled = false;
            Renovation.AvailableDatesListBox.IsEnabled = false;
        }
        public void DeleteRowExecute(ScheduledRenovation scheduledRenovation)
        {
            ScheduledRenovationService.GetInstance().Delete(scheduledRenovation);
            foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
            {
                if (accommodation.Id == scheduledRenovation.AccommodationId)
                {
                    Accommodations.Add(accommodation);
                    break;
                }
            }
            ScheduledRenovationService.GetInstance().UpdateUpcomingRenovations(User, ScheduledRenovations);
        }
        public void RenovateExecute()
        {
            ScheduledRenovation scheduledRenovation = new ScheduledRenovation();
            string? selectedDate = Renovation.AvailableDatesListBox.SelectedValue.ToString();
            string[] dates = selectedDate.Split('-');
            scheduledRenovation.StartDate = Convert.ToDateTime(dates[0].Trim());
            scheduledRenovation.EndDate = Convert.ToDateTime(dates[1].Trim());
            scheduledRenovation.AccommodationId = SelectedAccommodation.Id;
            scheduledRenovation.Duration = Convert.ToInt32(Renovation.DurationTextBox.NumTextBox.Text);
            scheduledRenovation.Details = Renovation.CommentTextBox.Text;
            ScheduledRenovationService.GetInstance().Add(scheduledRenovation);
            foreach(Accommodation accommodation in Accommodations)
            {
                if(accommodation.Id == scheduledRenovation.AccommodationId)
                {
                    Accommodations.Remove(accommodation);
                    break;
                }
            }
            ScheduledRenovationService.GetInstance().UpdateUpcomingRenovations(User, ScheduledRenovations);
            ResetInputs();

            if (App.currentLanguage() == ENG)
                notificationManager.Show("Success", "Renovation successfully scheduled!", NotificationType.Success);
            else
                notificationManager.Show("Success", "Renoviranje uspešno zakazano!", NotificationType.Success);
        }
        public void ResetInputs()
        {
            Renovation.StartDatePicker.SelectedDate = null;
            Renovation.EndDatePicker.SelectedDate = null;
            Renovation.DurationTextBox.NumTextBox.Text = string.Empty;
            AvailableDates.Clear();
            Renovation.CommentTextBox.IsEnabled = false;
            Renovation.AvailableDatesListBox.IsEnabled = false;
            Renovation.CommentTextBox.Text = string.Empty;

            Renovation.RenovationDetailsValidation.Visibility = Visibility.Hidden;
            Renovation.AvailableDatesValidation.Visibility = Visibility.Hidden;
        }
        public bool RenovateCanExecute()
        {
            if(string.IsNullOrEmpty(Renovation.CommentTextBox.Text) || 
               Renovation.AvailableDatesListBox.SelectedValue == null)
                return false;
            return true;
        }
        public void SearchExecute()
        {
            AvailableDates.Clear();
            DateTime startDate = Renovation.StartDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = Renovation.EndDatePicker.SelectedDate ?? DateTime.Now;
            int durationDays = Convert.ToInt32(Renovation.DurationTextBox.NumTextBox.Text);

            List<DateTime> availableDates = FindAvailableDates(startDate, endDate, durationDays);
            foreach (DateTime availableDate in availableDates)
            {
                AvailableDate date = new AvailableDate();
                date.checkInDate = availableDate;
                date.checkOutDate = availableDate.AddDays(durationDays);
                AvailableDates.Add(date);
            }

            Renovation.CommentTextBox.IsEnabled = true;
            Renovation.AvailableDatesListBox.IsEnabled = true;
            Renovation.Validation2();
        }
        public bool SearchCanExecute()
        {
            if (Renovation.AccommodationComboBox.SelectedItem == null ||
                Renovation.StartDatePicker.SelectedDate == null ||
                Renovation.EndDatePicker.SelectedDate == null ||
               !IsNumeric(Renovation.DurationTextBox.NumTextBox.Text))
                return false;
            return true;
        }

        private List<DateTime> FindAvailableDates(DateTime startDate, DateTime endDate, int durationDays)
        {
            List<DateTime> availableDates = new List<DateTime>();
            DateTime currentStartDate = startDate, currentEndDate = endDate;
            int counterDates = 0;
            while (currentStartDate <= currentEndDate)
            {
                if (counterDates == 10) break;
                if (AreDatesAvailable(currentStartDate, currentEndDate, durationDays))
                {
                    availableDates.Add(currentStartDate);
                    counterDates++;
                }

                currentStartDate = currentStartDate.AddDays(1);
            }
            if (availableDates.Count == 0)
            {
                currentStartDate = startDate;

                while (true)
                {
                    currentStartDate = currentStartDate.AddDays(1);
                    currentEndDate = currentStartDate.AddDays(durationDays);

                    if (counterDates == 5) break;

                    if (AreDatesAvailable(currentStartDate, currentEndDate, durationDays))
                    {
                        availableDates.Add(currentStartDate);
                        counterDates++;
                    }
                }
            }
            return availableDates;
        }
        private bool AreDatesAvailable(DateTime startDate, DateTime endDate, int durationDays)
        {
            if (!CheckDates(startDate, endDate, durationDays)) 
                return false;
            for (DateTime date = startDate; date <= startDate.AddDays(durationDays); date = date.AddDays(1))
            {
                foreach (ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().GetAll())
                {
                    if (SelectedAccommodation.Id == reservedAccommodation.Accommodation.Id)
                    {
                        if (!CheckReservedDates(date, reservedAccommodation)) 
                            return false;
                    }
                }
            }
            return true;
        }
        public bool CheckReservedDates(DateTime date, ReservedAccommodation reservedAccommodation)
        {
            if (date > reservedAccommodation.CheckInDate && date < reservedAccommodation.CheckOutDate) 
                return false;
            return true;
        }
        private bool CheckDates(DateTime startDate, DateTime endDate, int durationDays)
        {
            if (endDate <= startDate) 
                return false;
            if ((endDate - startDate).Days < durationDays) 
                return false;
            return true;
        }
        private bool IsNumeric(string text)
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
    }
}
