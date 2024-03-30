using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
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
using static System.Net.Mime.MediaTypeNames;
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Tourist.ListComponents
{
    /// <summary>
    /// Interaction logic for TourCardReserved.xaml
    /// </summary>
    public partial class TourCardReserved : UserControl  
    {
        public TourCardReserved()
        {
            InitializeComponent();
           
        }

        public void ClickedOnCard(object sender, RoutedEventArgs e)
        {
            var Selectedtour = DataContext as Tour;
            if (Selectedtour != null)
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
                TourReservationDetailed tourReservationDetailed = new TourReservationDetailed(Selectedtour,user);
                tourReservationDetailed.ShowDialog();
            }
        }
    }
}
