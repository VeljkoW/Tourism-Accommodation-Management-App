using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VirtualKeyboard.Wpf;

namespace BookingApp.ViewModel.Guide
{
    public class UpcomingTourViewModel : INotifyPropertyChanged
    {
        public UpcomingTourViewModel(User user)
        {
            User = user;
            UserName = user.Username;
            TourList = new ObservableCollection<UserControlTourCard>();
            Load();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public User User { get; set; }
        public string UserName { get; set; }
        public List<Tour> Tours { get; set; }
        private ObservableCollection<UserControlTourCard> _tourList;
        public ObservableCollection<UserControlTourCard> TourList
        {
            get { return _tourList; }
            set
            {
                _tourList = value;
                OnPropertyChanged(nameof(TourList));
            }
        }
        public void Load()
        {
            TourList.Clear();
            Dictionary<TourSchedule, Tour> t = TourService.GetInstance().LoadToursForGuide(User);
            foreach (var entry in t)
            {
                CreateTourCard(entry.Value, entry.Key);
            }
        }
        private void CreateTourCard(Tour tour, TourSchedule schedule)
        {
            UserControlTourCard userControlTourCard = new UserControlTourCard(tour, User, schedule);
            userControlTourCard.Margin = new Thickness(0, 0, 0, 15);
            userControlTourCard.OnFinishedTour += UserControlTourCard_OnFinishedTour;
            userControlTourCard.OnClickedGoBackMonitoringTour += UserControlTourCard_OnFinishedTour;
            if (schedule.ScheduleStatus == ScheduleStatus.Ongoing)
            {
                TourList.Insert(0, userControlTourCard);
            }
            else
            {
            TourList.Add(userControlTourCard);
            }
        }
        private void UserControlTourCard_OnFinishedTour()
        {
            Load();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
