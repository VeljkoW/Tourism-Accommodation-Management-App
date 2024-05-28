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
    /// Interaction logic for TourComplexSuggestionDetailed.xaml
    /// </summary>
    public partial class TourComplexSuggestionDetailed : Window
    {
        public TourComplexSuggestionDetailedViewModel TourComplexSuggestionDetailedViewModel { get; set; }
        public TourComplexSuggestionDetailed(TourComplexSuggestion tourComplexSuggestion, User user)
        {
            InitializeComponent();
            TourComplexSuggestionDetailedViewModel = new TourComplexSuggestionDetailedViewModel(this,user,tourComplexSuggestion);
            this.DataContext = TourComplexSuggestionDetailedViewModel;
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
    }
}
