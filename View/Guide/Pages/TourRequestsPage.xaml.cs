using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for TourRequestsPage.xaml
    /// </summary>
    public partial class TourRequestsPage : Page
    {
        private TourRequestsPageViewModel viewModel;
        public TourRequestsPage()
        {
            InitializeComponent();
            TourRequestsPageViewModel viewModel = new TourRequestsPageViewModel(this);
            DataContext = viewModel;
        }
        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        public event Action FilterUpdateEvent;
        private void FilterUpdated(object sender, SelectionChangedEventArgs e)
        {
            FilterUpdateEvent?.Invoke();
        }

        private void TextBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            FilterUpdateEvent?.Invoke();
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        private bool IsTextAllowed(string text)
        {
            // Only allow numeric input
            Regex regex = new Regex("^[0-9]+$");
            return regex.IsMatch(text);
        }
    }
}
