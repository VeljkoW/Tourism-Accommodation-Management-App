using BookingApp.Domain.Model;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows;

namespace BookingApp.ViewModel.Guide
{
    public class UserControlTourCardViewModel
    {
        public event EventHandler TourSelected;
        public Tour Tour { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public Image Image { get; set; }
        public RelayCommand MonitoringSelectedTour => new RelayCommand(execute => MonitoringSelectedTourExecute());

        private string _tourName;
        public string TourName
        {
            get => _tourName;
            set
            {
                if (value != _tourName)
                {
                    _tourName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _date;
        public string DateString
        {
            get => _date;
            set
            {
                if (value != _date)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _scheduleId;
        public int ScheduleId
        {
            get => _scheduleId;
            set
            {
                if (value != _scheduleId)
                {
                    _scheduleId = value;
                    OnPropertyChanged();
                }
            }
        }
        public TourSchedule TourSchedule { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public UserControlTourCard UserControlTourCard { get; set; }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public UserControlTourCardViewModel() { }
        public UserControlTourCardViewModel(UserControlTourCard userControlTourCard, Tour t, User user, TourSchedule schedule)
        {
            UserControlTourCard = userControlTourCard;
            Tour = t;
            User = user;
            TourName = t.Name;
            DateString = t.DateTime.ToString();
            Description = t.Description;
            Image = t.Images[0];
            ScheduleId = schedule.Id;
            TourSchedule = schedule;
            if (schedule.ScheduleStatus == ScheduleStatus.Ongoing)
            {
                UserControlTourCard.LiveIcon.Opacity = 1;
            }
            else
            {
                UserControlTourCard.LiveIcon.Opacity = 0;
            }
            var converter = new ImageSourceConverter();
            UserControlTourCard.MainImage.Source = (ImageSource)converter.ConvertFromString(Image.Path);
        }
        private void MonitoringSelectedTourExecute()
        {
            MonitoringTour monitoringTour = new MonitoringTour(Tour, User, TourSchedule);
            NavigationService.GetNavigationService(UserControlTourCard).Navigate(monitoringTour);
            monitoringTour.OnFinishedTour += MonitoringTour_OnFinishedTour;
            monitoringTour.OnLastKeypoint += MonitoringTour_OnFinishedTour;
            monitoringTour.OnClickGoBack += ClickGoBackMonitoringTour;
        }
        public Action OnClickedGoBackMonitoringTour { get; set; }
        private void ClickGoBackMonitoringTour()
        {
            OnClickedGoBackMonitoringTour?.Invoke();
        }

        public event EventHandler OnFinishedTour;
        private void MonitoringTour_OnFinishedTour(object? sender, EventArgs e)
        {
            OnFinishedTour?.Invoke(this, e);
        }
    }
}
