using BookingApp.Model;
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
    /// Interaction logic for TourCard.xaml
    /// </summary>
    public partial class TourCard : UserControl
    {
        public TourCard()
        {
            InitializeComponent();
        }

        public void ClickedOnCard(object sender, RoutedEventArgs e)
        {
            var Selectedtour = DataContext as Tour;
            if (Selectedtour != null)
            {
                string name = Selectedtour.Name;
                string description = Selectedtour.Description;
                //string imagePaths = tour.ImagePath;
                TourDetailed tourDetailed = new TourDetailed(Selectedtour);
                tourDetailed.ShowDialog();
            }
        }

    }
}
