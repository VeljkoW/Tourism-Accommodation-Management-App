using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ComplexTourRequestsPage.xaml
    /// </summary>
    public partial class ComplexTourRequestsPage : Page,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<UserControlComplexTourSuggestionListing> Cards { get; set; }
        public static NavigationService PageNavigationService;
        public ComplexTourRequestsPage()
        {
            InitializeComponent();
            Cards = new ObservableCollection<UserControlComplexTourSuggestionListing>();
            PageNavigationService = this.NavigationService;
            Load();
        }
        public void Load()
        {
            UserControlPanel.Children.Clear();
            List<TourSuggestion> complexSuggestions = TourSuggestionComplexService.GetInstance().GetAll();
            List<TourComplexSuggestion> complexTours = TourComplexSuggestionService.GetInstance().GetAll();
            foreach(TourComplexSuggestion tourComplexSuggestion in complexTours)
            {
                List<TourSuggestion> tempSuggestions = complexSuggestions.Where(t => t.ComplexTourId == tourComplexSuggestion.Id).ToList();
                bool guideAcceptedTour = false;
                foreach(TourSuggestion complexSuggestion in tempSuggestions)
                {
                    if(complexSuggestion.GuideId == GuideMainWindow.UserId)
                    {
                        guideAcceptedTour=true;
                        break;
                    }
                }
                if(!guideAcceptedTour)
                {
                    //
                    UserControlComplexTourSuggestionListing var = new UserControlComplexTourSuggestionListing(this, tourComplexSuggestion.Id);
                    UserControlPanel.Children.Add(var);
                }
            }
        }
    }
}
