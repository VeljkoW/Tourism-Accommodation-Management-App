using BookingApp.Domain.Model;
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
    /// Interaction logic for TourCardFinished.xaml
    /// </summary>
    public partial class TourCardFinished : UserControl
    {
        public TourCardFinished()
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
                TourFinishedDetailed tourFinishedDetailed = new TourFinishedDetailed(Selectedtour, user);
                tourFinishedDetailed.ShowDialog();
            }
        }
    }
}
