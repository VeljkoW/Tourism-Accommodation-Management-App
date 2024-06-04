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
using BookingApp.Services;
using Notification.Wpf;

namespace BookingApp.ViewModel.Guide
{
    public class UserControlTourCardViewModel
    {
        public INotificationManager notificationManager = App.GetNotificationManager();
        public event EventHandler TourSelected;
        public Tour Tour { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public Image Image { get; set; }
        public RelayCommand MonitoringSelectedTour => new RelayCommand(execute => MonitoringSelectedTourExecute() ,canExecute => MonitoringSelectedTourCanExecute());
        private bool MonitoringSelectedTourCanExecute()
        {
            if (Tour.DateTime.Day == DateTime.Now.Day)
            {
                if(TourScheduleService.GetInstance().GetAll().Where(t=>t.ScheduleStatus==ScheduleStatus.Ongoing).Count() == 0)
                {
                    return true;
                }
                if(TourScheduleService.GetInstance().GetById(TourSchedule.Id).ScheduleStatus == ScheduleStatus.Ongoing)
                {
                return true;
                }
            }
            return false;
        }

        private bool DeleteSelectedTourCanExecute()
        {
            if(Tour.DateTime> DateTime.Now.AddDays(2)) {
                return true;
            }
            return false;
        }

        public RelayCommand DeleteSelectedTour => new RelayCommand(execute => DeleteSelectedTourExecute(),canExecute => DeleteSelectedTourCanExecute());

        private void DeleteSelectedTourExecute()
        {
            var reply = MessageBox.Show("Are you sure you want to delete this tour", "Deleting tour",MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(reply == MessageBoxResult.Yes)
            {
                TourService.GetInstance().HandoutCoupons(ScheduleId);
                OnFinishedTour?.Invoke(this, new EventArgs());
            }
            notificationManager.Show("Success", "You have successfully deleted this tour!", NotificationType.Success);
        }
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
        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _length;
        public string Length
        {
            get => _length;
            set
            {
                if (value != _length)
                {
                    _length = value;
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
            this.Location = t?.Location?.State + " " + t?.Location?.City;
            Length = t.Duration.ToString() + " Hours";
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
