using BookingApp.ViewModel.Guide;
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

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for BurgerMenu.xaml
    /// </summary>
    public partial class BurgerMenu : UserControl
    {
        private GuideMainPage page;
        public BurgerMenu(GuideMainPage page)
        {
            InitializeComponent();
            this.page = page;
        }
        private void UpcommingTours(object sender, RoutedEventArgs e)
        {
            page.ClickUpcommingTour(sender, e);
        }
        private void CreateTour(object sender, RoutedEventArgs e)
        {
            page.ClickCreateTour(sender, e);
        }
        private void TourRequests(object sender, RoutedEventArgs e)
        {
            page.ClickTourSuggestions(sender, e);
        }
        private void TourStatistics(object sender, RoutedEventArgs e)
        {
            page.ClickTourStatistics(sender, e);
        }
        private void TourRequestStatistics(object sender, RoutedEventArgs e)
        {
            page.ClickTourSuggestionsStatistics(sender, e);
        }
        private void ComplexRequests(object sender, RoutedEventArgs e)
        {
            //TODO: VELJKO
            page.ClickTourSuggestions(sender, e);
        }
        private void FinishedTours(object sender, RoutedEventArgs e)
        {
            page.ClickTourReviews(sender, e);
        }
        private void HideBurger(object sender, RoutedEventArgs e)
        {
            page.HideBurger(sender, e);
        }
    }
}