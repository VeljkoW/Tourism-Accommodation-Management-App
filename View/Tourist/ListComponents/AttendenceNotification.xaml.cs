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
    /// Interaction logic for AttendenceNotification.xaml
    /// </summary>
    public partial class AttendenceNotification : UserControl
    {
        public AttendenceNotification()
        {
            InitializeComponent();
        }
        public void ConfirmAttendence(object sender,RoutedEventArgs e)
        {
            var SelectedNotification = DataContext as TourAttendenceNotification;

            if (SelectedNotification != null)
            {
                SelectedNotification.ConfirmedAttendence = true;
                TourAttendenceNotificationService.GetInstance().Update(SelectedNotification);
                //Border.BorderBrush = new SolidColorBrush(Colors.DarkGray);
                //Border.Background = new SolidColorBrush(Colors.Gray);
            }

        }
    }
}
