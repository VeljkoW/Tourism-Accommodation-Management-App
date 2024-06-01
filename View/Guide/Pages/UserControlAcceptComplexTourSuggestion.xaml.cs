using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide.Pages;
using BookingApp.ViewModel;
using BookingApp.ViewModel.Guide;
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

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for UserControlAcceptComplexTourSuggestion.xaml
    /// </summary>
    public partial class UserControlAcceptComplexTourSuggestion : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Pages.ComplexTourRequestToursPage complexTourRequestToursPage;
        public ObservableCollection<string> Dates { get; set; } = new ObservableCollection<string>();
        public string SelectedDate { get; set; } = "";
        public RelayCommand AcceptComplextour => new RelayCommand(execute => AcceptComplextourExecute(),canExecute => AcceptComplextourCanExecute());
        private TourSuggestion suggestion;
        public Action requestRefresh;
        public UserControlAcceptComplexTourSuggestion(Pages.ComplexTourRequestToursPage complexTourRequestToursPage, TourSuggestion suggestion)
        {
            this.suggestion = suggestion;
            this.complexTourRequestToursPage = complexTourRequestToursPage;
            List<DateTime> usableDates = TourService.GetInstance().GetDatesForGuide(GuideMainWindow.UserId, suggestion);
            foreach(DateTime date in usableDates)
            {
                Dates.Add(date.ToShortDateString());
            }
            DataContext = this;
            InitializeComponent();
        }

        private void CancelComplexTour(object sender, RoutedEventArgs e)
        {
            complexTourRequestToursPage.PopupPanel.Children.Clear();
        }
        private void AcceptComplextourExecute()
        {
            suggestion.Status = TourSuggestionStatus.Accepted;
            suggestion.Date = Convert.ToDateTime(SelectedDate);
            suggestion.GuideId = GuideMainPage.UserId;
            TourSuggestionComplexService.GetInstance().Update(suggestion);
            complexTourRequestToursPage.PopupPanel.Children.Clear();
            requestRefresh?.Invoke();
        }
        private bool AcceptComplextourCanExecute()
        {
            if (SelectedDate.Equals(""))
            {
                return false;
            }
            return true;
        }
    }
}
