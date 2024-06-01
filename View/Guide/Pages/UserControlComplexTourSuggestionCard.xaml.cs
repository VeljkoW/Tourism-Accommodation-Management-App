using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
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
    public partial class UserControlComplexTourSuggestionCard : UserControl
    {
        public UserControlComplexTourSuggestionCard(ComplexTourRequestToursPage complexTourRequestToursPage, UserControlComplexTourSuggestionListing userControlComplexTourSuggestionListing, TourSuggestion suggestion)
        {
            InitializeComponent();
            UserControlComplexTourSuggestionCardViewModel viewModel = new UserControlComplexTourSuggestionCardViewModel(complexTourRequestToursPage, userControlComplexTourSuggestionListing, suggestion);
            DataContext= viewModel;
        }
        public Action GoBack;
    }
}
