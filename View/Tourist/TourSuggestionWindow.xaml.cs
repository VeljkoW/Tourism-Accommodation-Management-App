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
        public TourSuggestionWindow(User user)
        {
            InitializeComponent();
            TourSuggestionWindowViewModel = new TourSuggestionWindowViewModel(this, user);
            this.DataContext = TourSuggestionWindowViewModel;
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

        private void GenerateTextBoxes(object sender, TextChangedEventArgs e)
        {
            TourSuggestionWindowViewModel.GenerateTextBoxes(sender, e);
        }

        public void Cancel(object sender, RoutedEventArgs e)
        {
            TourSuggestionWindowViewModel.Cancel(sender, e);
        }
        public void Suggest(object sender, RoutedEventArgs e)
        {
            TourSuggestionWindowViewModel.Suggest(sender, e);
        }
    }
}
