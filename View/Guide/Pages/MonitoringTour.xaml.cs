using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
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
    /// Interaction logic for MonitoringTour.xaml
    /// </summary>
    public partial class MonitoringTour : Page
    {
        public event EventHandler OnFinishedTour;
        public MonitoringTour()
        {
            InitializeComponent();
        }
        public MonitoringTour(Tour tour, User user, TourSchedule schedule)
        {
            InitializeComponent();
            MonitoringTourViewModel monitoringTourViewModel = new MonitoringTourViewModel(this, tour, user, schedule);
            monitoringTourViewModel.OnFinishedTour += ClickGoBack;
            monitoringTourViewModel.OnClickGoBack += ClickGoBack;
            this.DataContext = monitoringTourViewModel;
        }

        public EventHandler OnLastKeypoint {  get; set; }
        private void ClickedOnLastKeypoint(object s, EventArgs e)
        {
            OnLastKeypoint?.Invoke(this, e);
        }
        private void RaiseEventHandler(object sender, RoutedEventArgs e)
        {
            OnFinishedTour?.Invoke(sender, e);
        }
        public Action OnClickGoBack { get;set; }
        private void ClickGoBack()
        {
            OnClickGoBack?.Invoke();
            //NavigationService.GoBack();
        }
    }
}
