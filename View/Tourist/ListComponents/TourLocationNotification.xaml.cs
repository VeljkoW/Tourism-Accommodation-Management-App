using BookingApp.Domain.Model;
using BookingApp.Services;
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

namespace BookingApp.View.Tourist.ListComponents
{
    /// <summary>
    /// Interaction logic for TourLocationNotification.xaml
    /// </summary>
    public partial class TourLocationNotification : UserControl
    {
        public TourLocationNotification()
        {
            InitializeComponent();
        }
        public void DeleteNotification(object sender, RoutedEventArgs e)
        {
            var SelectedNotification = DataContext as TourNotification;

            if (SelectedNotification != null)
            {
                SelectedNotification.Status = NotificationStatus.Read;
                TourNotificationService.GetInstance().Update(SelectedNotification);
                //(Window.GetWindow(this) as NotificationWindow).NotificationWindowViewModel.UpdateTourSuggestionNotifications(); does not work as intended
            }

        }
        public void ClickedOnCard(object sender, RoutedEventArgs e)
        {
            var SelectedTourNotification = DataContext as TourNotification;
            if (SelectedTourNotification != null)
            {
                User user = new User();

                if (TouristMainWindow.User != null)
                {
                    user = TouristMainWindow.User;
                }
                else
                {
                    throw new Exception();
                }
                Tour tour = TourService.GetInstance().GetById(SelectedTourNotification.TourId);
                bool exists = false;
                foreach (TourSchedule tourSchedule in TourScheduleService.GetInstance().GetAll())
                {
                    if (SelectedTourNotification.TourId == tourSchedule.TourId && tourSchedule.ScheduleStatus == ScheduleStatus.Ready)
                    {
                        tour = TourService.GetInstance().MakeTour(tour, tourSchedule);
                        exists = true;
                        break;
                    }
                }
                if (exists)
                {
                    TourDetailed tourDetailed = new TourDetailed(tour, user);
                    tourDetailed.ShowDialog();
                }
            }
        }
    }
}
