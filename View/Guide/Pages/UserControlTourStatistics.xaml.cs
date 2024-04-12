using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guide.Pages
{
    public partial class UserControlTourStatistics : UserControl
    {
        public UserControlTourStatisticsViewModel UserControlTourStatisticsViewModel { get; set; }
        public User User { get; }
        public Tour Tour { get; }
        public Domain.Model.TourStatistics TourStatistics { get; }

        public UserControlTourStatistics()
        {
            InitializeComponent();
            UserControlTourStatisticsViewModel = new UserControlTourStatisticsViewModel();
            DataContext = UserControlTourStatisticsViewModel;
        }

        public UserControlTourStatistics(User user, Tour tour, Domain.Model.TourStatistics tourStatistics)
        {
            InitializeComponent();
            User = user;
            Tour = tour;
            TourStatistics = tourStatistics;
            UserControlTourStatisticsViewModel = new UserControlTourStatisticsViewModel(user,tour,tourStatistics);
            DataContext = UserControlTourStatisticsViewModel;
        }
    }
}