using BookingApp.Repository.TourRepositories;
using BookingApp.Repository;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;
using BookingApp.View.Guide.Pages;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

namespace BookingApp.ViewModel.Guide
{
    public class GuideMainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public GuideComplexService guideComplexService = GuideComplexService.GetInstance();
        public User User { get; set; }
        public string UserName { get; set; }
        public List<Tour> Tours { get; set; }
        public List<UserControlTourCard> SelectedItems { get; set; } = new List<UserControlTourCard>(); 
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
        public GuideMainPageViewModel(User user)
        {
            User = user;
            UserName = user.Username;
            TourList = new ObservableCollection<UserControlTourCard>();
            Load();
        }
        public void Load()
        {
            TourList.Clear();
            Dictionary<TourSchedule, Tour> t = GuideComplexService.GetInstance().LoadTours(User);
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
            TourList.Add(userControlTourCard);
        }
        private void UserControlTourCard_OnFinishedTour(object? sender, EventArgs e)
        {
            Load();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
