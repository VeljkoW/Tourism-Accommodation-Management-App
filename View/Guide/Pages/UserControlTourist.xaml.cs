using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using BookingApp.Services;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for UserControlKeyPoint.xaml
    /// </summary>
    public partial class UserControlTourist : UserControl
    {
        UserControlTouristViewModel UserControlTouristViewModel { get; set; }
        public UserControlTourist(TourPerson tourist,int currentKeypointId)
        {
            InitializeComponent();
            UserControlTouristViewModel = new UserControlTouristViewModel(tourist, currentKeypointId);
            UserControlTouristViewModel.touristVisitedKeypoint += touristVisiting;
            DataContext = UserControlTouristViewModel;
        }
        public Action touristVisitedKeypoint { get; set; }
        private void touristVisiting()
        {
            touristVisitedKeypoint?.Invoke();
        }
    }
}
