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
        public void Cancel(object sender, RoutedEventArgs e)
        {
            TourReviewWindowViewModel.Cancel(sender, e);
        }
        public void Submit(object sender, RoutedEventArgs e)
        {
            TourReviewWindowViewModel.Submit(sender, e);
        }
        public void Attach(object sender, RoutedEventArgs e)
        {

        }
    }
}
