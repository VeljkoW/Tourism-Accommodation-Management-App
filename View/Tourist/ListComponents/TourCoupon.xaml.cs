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
    /// Interaction logic for TourCoupon.xaml
    /// </summary>
    public partial class TourCoupon : UserControl
    {
        public TourCoupon()
        {
            InitializeComponent();
        }

        public void ClickedOnCoupon(object sender, RoutedEventArgs e)
        {
            
            var Selectedcoupon = DataContext as TourCoupon;
            if (Selectedcoupon != null)
            {

            }
            
        }
    }
}
