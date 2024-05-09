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
    /// Interaction logic for TourSuggestionStatistics.xaml
    /// </summary>
    public partial class TourSuggestionStatistics : Window
    {
        public TourSuggestionStatisticsViewModel TourSuggestionStatisticsViewModel { get; set; }
        public User User;
        public TourSuggestionStatistics(User user)
        {
            InitializeComponent();
            User = user;
            TourSuggestionStatisticsViewModel = new TourSuggestionStatisticsViewModel(this,User);
            this.DataContext = TourSuggestionStatisticsViewModel;
        }
        private void LoadedFunctions(object sender, RoutedEventArgs e)
        {
            CenterWindow();
        }

        private void Year1ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TourSuggestionStatisticsViewModel.Year1ComboBoxSelectionChanged(sender, e);
        }
        private void Year2ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TourSuggestionStatisticsViewModel.Year2ComboBoxSelectionChanged(sender, e);
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
    }
}
