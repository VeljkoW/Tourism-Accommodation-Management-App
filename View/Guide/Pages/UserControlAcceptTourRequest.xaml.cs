using BookingApp.Domain.Model;
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
    /// Interaction logic for UserControlAcceptTourRequest.xaml
    /// </summary>
    public partial class UserControlAcceptTourRequest : UserControl
    {
        public UserControlAcceptTourRequest(TourRequestsPageViewModel tourRequestsPageViewModel, TourSuggestion tourSuggestion)
        {
            InitializeComponent();
            TourSuggestion = tourSuggestion;
            UserControlAcceptTourRequestViewModel viewModel = new UserControlAcceptTourRequestViewModel(this,tourRequestsPageViewModel,tourSuggestion);
            DataContext=viewModel;
        }

        public TourSuggestion TourSuggestion { get; }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
