using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guide
{
    public class TourStatisticsViewModel
    {
        public User User { get;set; }
        public TourStatisticsViewModel(User user) {
            User = user;
            UserControlTourStatistics = new ObservableCollection<UserControlTourStatistics>();
            Dictionary<Tour, Domain.Model.TourStatistics> userControlData =TourService.GetInstance().GetTourStatistics();
            MostVisited = new UserControlTourStatistics(user,userControlData.Keys.ElementAt(0), userControlData.Values.ElementAt(0));
            UserControlTourStatistics.Add(MostVisited);
        }

        public ObservableCollection<UserControlTourStatistics> UserControlTourStatistics { get; set; }
        public UserControlTourStatistics MostVisited { get; set; }
    }
}