using BookingApp.Domain.Model;
using BookingApp.View.Owner;
using BookingApp.View.Tourist;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for UserControlTourCard.xaml
    /// </summary>
    public partial class UserControlTourCard : UserControl
    {
        public UserControlTourCard() { }
        public UserControlTourCard(Tour t, User user,TourSchedule schedule)
        {
            InitializeComponent();
            UserControlTourCardViewModel userControlTourCardViewModel = new UserControlTourCardViewModel(this,t,user,schedule);
            userControlTourCardViewModel.OnClickedGoBackMonitoringTour += ClickGoBackMonitoringTour;
            userControlTourCardViewModel.OnFinishedTour += MonitoringTour_OnFinishedTour;
            this.DataContext = userControlTourCardViewModel;
        }
        public Action OnClickedGoBackMonitoringTour { get; set; }
        private void ClickGoBackMonitoringTour()
        {
            OnClickedGoBackMonitoringTour?.Invoke();
        }
        public event Action OnFinishedTour;
        private void MonitoringTour_OnFinishedTour(object? sender, EventArgs e)
        {
            OnFinishedTour?.Invoke();
        }
    }
}