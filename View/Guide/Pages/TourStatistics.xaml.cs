using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for TourStatistics.xaml
    /// </summary>
    public partial class TourStatistics : Page
    {
        public ObservableCollection<UserControlTourStatistics> UserControlTourStatistics { get; set; } = new ObservableCollection<UserControlTourStatistics>();
        public User User { get; }
        public TourStatistics(User user)
        {
            InitializeComponent();
            User = user;
            TourStatisticsViewModel tourStatisticsViewModel = new TourStatisticsViewModel(this,User);
            DataContext = tourStatisticsViewModel;
        }
    }
}
