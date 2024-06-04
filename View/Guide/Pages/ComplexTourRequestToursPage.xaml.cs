using BookingApp.Domain.Model;
using BookingApp.Services;
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
    /// Interaction logic for ComplexTourRequestToursPage.xaml
    /// </summary>
    public partial class ComplexTourRequestToursPage : Page
    {
        //TourSuggestions
        private NavigationService navService;
        private int id;
        private UserControlComplexTourSuggestionListing userControlComplexTourSuggestionListing;
        public Action RequestRefresh;
        public ComplexTourRequestToursPage(UserControlComplexTourSuggestionListing userControlComplexTourSuggestionListing, NavigationService navigationService, int id)
        {
            InitializeComponent();
            this.userControlComplexTourSuggestionListing = userControlComplexTourSuggestionListing;
            this.id = id;
            navService = navigationService;
            Load();
        }
        public void Load()
        {
            TourSuggestions.Children.Clear();
            TourComplexSuggestion complexSuggestions = TourComplexSuggestionService.GetInstance().GetById(id);
            List<TourSuggestion> suggestions = TourSuggestionComplexService.GetInstance().GetAll();
            suggestions=suggestions.Where(suggestion => suggestion.ComplexTourId == id).ToList();
            foreach(TourSuggestion suggestion in suggestions)
            {
                if(suggestion.Status != TourSuggestionStatus.Pending)
                {
                    continue;
                }
                if(suggestion.ToDate < DateTime.Now.AddDays(2))
                {
                    continue;
                }
                UserControlComplexTourSuggestionCard card = new UserControlComplexTourSuggestionCard(this, userControlComplexTourSuggestionListing, suggestion);
                TourSuggestions.Children.Add(card);
            }
        }
        public void Dimm()
        {
            this.DimOverlay.Visibility = Visibility.Visible;
        }
        public void Undimm()
        {
            this.DimOverlay.Visibility = Visibility.Collapsed;
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            userControlComplexTourSuggestionListing.complexTourRequestsPage.Load();
            navService.GoBack();
        }
    }
}
