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
    /// Interaction logic for TourSuggestionWindow.xaml
    /// </summary>
    public partial class TourSuggestionWindow : Window
    {
        TourSuggestionWindowViewModel TourSuggestionWindowViewModel { get; set; }
        public TourSuggestionWindow(User user, bool isComplex, int complexId,bool demo, TouristMainWindowViewModel touristMainWindowViewModel)
        {
            InitializeComponent();
            TourSuggestionWindowViewModel = new TourSuggestionWindowViewModel(this, user, isComplex, complexId,demo, touristMainWindowViewModel);
            this.DataContext = TourSuggestionWindowViewModel;
            SetDatePickerBlackoutDates();
        }
        private void SetDatePickerBlackoutDates()
        {
            DateTime timestamp = DateTime.Today.AddDays(2);
            CalendarDateRange blackoutRange = new CalendarDateRange(new DateTime(1, 1, 1), timestamp.AddDays(-1));
            StartDatePicker.BlackoutDates.Add(blackoutRange);
            EndDatePicker.BlackoutDates.Add(blackoutRange);
        }
        private void LoadedFunctions(object sender, RoutedEventArgs e)
        {
            CenterWindow();
        }
        public void EndDemoMode(object sender, MouseButtonEventArgs e)
        {
            TourSuggestionWindowViewModel.EndDemoMode();
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
        private void StateComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TourSuggestionWindowViewModel.StateComboBoxSelectionChanged(sender, e);
        }
        private void GenerateTextBoxes(object sender, TextChangedEventArgs e)
        {
            TourSuggestionWindowViewModel.GenerateTextBoxes(sender, e);
        }
        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
