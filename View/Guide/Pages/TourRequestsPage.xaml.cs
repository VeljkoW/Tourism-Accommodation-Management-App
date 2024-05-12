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
    /// Interaction logic for TourRequestsPage.xaml
    /// </summary>
    public partial class TourRequestsPage : Page
    {
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
    }
}
