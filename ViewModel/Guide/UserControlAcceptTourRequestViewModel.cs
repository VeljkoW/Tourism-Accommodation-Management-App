using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guide
{
    public class UserControlAcceptTourRequestViewModel : INotifyPropertyChanged
    {
        public List<int> HoursList { get; set; }
        public List<int> MinutesList { get; set; }
        public List<string> AmPm { get; set; }
        public UserControlAcceptTourRequestViewModel(TourRequestsPageViewModel tourRequestsPageViewModel, TourSuggestion tourSuggestion)
        {
            TourRequestsPageViewModel = tourRequestsPageViewModel;
            TourSuggestion = tourSuggestion;
            HoursList = Enumerable.Range(1, 12).ToList();
            MinutesList = new List<int>() { 0, 15, 30, 45 };
            AmPm = new List<string>() { "AM", "PM" };
        }
        public string SelectedAmPm { get; set; }
        public string SelectedHour { get; set; }
        public string SelectedMinute { get; set; }
        private bool _isDateSelected = false;
        public bool IsDateSelected
        {
            get { return _isDateSelected; }
            set
            {
                if (_isDateSelected != value)
                {
                    _isDateSelected = value;
                    OnPropertyChanged(nameof(IsDateSelected));
                }
            }
        }
        public DateTime SelectedDate { get; set; } = DateTime.Now;
        public TourSuggestion TourSuggestion { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RelayCommand AcceptTourRequest => new RelayCommand(execute => AcceptTourRequestExecute(),canExecute => AcceptTourRequestCanExecute());
        private bool AcceptTourRequestCanExecute()
        {
            try
            {

                if (SelectedAmPm == null || SelectedHour == null || SelectedMinute == null)
                {
                    IsDateSelected=false;
                    return false;
                }
                int selectedHour = Convert.ToInt32(SelectedHour);
                int selectedMinute = Convert.ToInt32(SelectedMinute);
                string selectedAmPm = SelectedAmPm;
                DateTime date = SelectedDate;
                if (selectedAmPm == "PM" && selectedHour != 12)
                {
                    selectedHour += 12;
                }
                else if (selectedAmPm == "AM" && selectedHour == 12)
                {
                    selectedHour = 0;
                }
                this.selectedDate = new DateTime(date.Year, date.Month, date.Day, selectedHour, selectedMinute, 0);
                IsDateSelected =true;
                return TourSuggestionService.GetInstance().IsGuideFree(this.selectedDate);
            }
            catch { return false; }
        }
        private DateTime selectedDate;
        public RelayCommand CancelCommand => new RelayCommand(execute => CancelCommandExecute());

        private void CancelCommandExecute()
        {
            TourRequestsPageViewModel.ClearPopup();
        }

        public TourRequestsPageViewModel TourRequestsPageViewModel { get; }

        private void AcceptTourRequestExecute()
        {
            TourSuggestion.Status = TourSuggestionStatus.Accepted;
            TourSuggestion.Date = selectedDate;
            TourSuggestionService.GetInstance().Update(TourSuggestion);
            TourSuggestionNotificationService.GetInstance().Add(new TourSuggestionNotification(TourSuggestion.Id, DateTime.Now));
            TourRequestsPageViewModel.ClearPopup();
        }
    }
}