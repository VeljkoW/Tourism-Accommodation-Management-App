using BookingApp.Domain.Model;
using BookingApp.ViewModel.Tourist;
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
using System.Windows.Shapes;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourReviewWindow.xaml
    /// </summary>
    public partial class TourReviewWindow : Window
    {
        TourReviewWindowViewModel TourReviewWindowViewModel { get; set; }
        public TourReviewWindow(Tour tour,User user)
        {
            InitializeComponent();
            this.TourReviewWindowViewModel = new TourReviewWindowViewModel(this,tour,user);
            this.DataContext = TourReviewWindowViewModel;
        }
        private void LoadedFunctions(object sender, RoutedEventArgs e)
        {
            CenterWindow();
        }

        private void CenterWindow()
        {
            double SWidth = SystemParameters.PrimaryScreenWidth;
            double SHeight = SystemParameters.PrimaryScreenHeight;
            double WWidth = this.Width;
            double WHeight = this.Height;

            this.Left = (SWidth - WWidth) / 2;
            this.Top = (SHeight - WHeight) / 2;
        }
        public void BtnSelectFiles_ClickExecute(object sender, RoutedEventArgs e)
        {
            TourReviewWindowViewModel.BtnSelectFiles_ClickExecute(sender,e);
        }
        public void StarMouseEnter(object sender, MouseEventArgs e)
        {
            TourReviewWindowViewModel.StarMouseEnter(sender, e);
        }
        public void StarMouseLeave(object sender, MouseEventArgs e)
        {
            TourReviewWindowViewModel.StarMouseLeave(sender, e);
        }
        public void StarClick(object sender, MouseEventArgs e)
        {
            TourReviewWindowViewModel.StarClick(sender, e);
        }
    }
}
