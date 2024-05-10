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
    /// Interaction logic for TourLanguageNotification.xaml
    /// </summary>
    public partial class TourLanguageNotification : UserControl
    {
        public TourLanguageNotification()
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
    }
}
