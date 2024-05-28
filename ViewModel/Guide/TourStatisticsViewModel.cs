using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VirtualKeyboard.Wpf;

namespace BookingApp.ViewModel.Guide
{
    public class TourStatisticsViewModel
    {
        public User User { get;set; }
        public RelayCommand ClickInGeneral => new RelayCommand(execute => ClickInGeneralExecute());
        public RelayCommand ClickSelectedYear => new RelayCommand(execute => ClickSelectedYearExecute());
        public RelayCommand ClickGoBack => new RelayCommand(execute => ClickGoBackExecute());

        private void ClickGoBackExecute(){
            TourStatisticsPage.NavigationService.GoBack();
        }
        private void ClickSelectedYearExecute()
        {
            UserControlTourStatistics.Clear();
            MostVisited.Clear();
            int year = Convert.ToInt32(TourStatisticsPage.YearComboBox.Text);
            if(year > 0){
                Dictionary<Tour, Domain.Model.TourStatistics> userControlData = TourService.GetInstance().GetTourStatistics(year);
                if (userControlData.Count < 1){
                    userControlData.Clear();
                    return;
                }
                KeyValuePair<Tour, Domain.Model.TourStatistics> mostVisitedEntry = userControlData.FirstOrDefault();
                foreach (var entry in userControlData){
                    if (entry.Value.Visitors > mostVisitedEntry.Value.Visitors){
                        mostVisitedEntry = entry;
                    }
                }
                MostVisited.Add(new UserControlTourStatistics(User, mostVisitedEntry.Key, mostVisitedEntry.Value));

                foreach (var entry in userControlData){
                    if (entry.Key != mostVisitedEntry.Key){
                        UserControlTourStatistics.Add(new UserControlTourStatistics(User, entry.Key, entry.Value));
                    }
                }
            }
        }

        private void ClickInGeneralExecute()
        {
            UserControlTourStatistics.Clear();
            MostVisited.Clear();
            Dictionary<Tour, Domain.Model.TourStatistics> userControlData = TourService.GetInstance().GetTourStatistics();
            KeyValuePair<Tour, Domain.Model.TourStatistics> mostVisitedEntry = userControlData.FirstOrDefault();
            foreach (var entry in userControlData){
                if (entry.Value.Visitors > mostVisitedEntry.Value.Visitors){
                    mostVisitedEntry = entry;
                }
            }
            MostVisited.Add(new UserControlTourStatistics(User, mostVisitedEntry.Key, mostVisitedEntry.Value));
            foreach (var entry in userControlData){
                if (entry.Key != mostVisitedEntry.Key){
                    UserControlTourStatistics.Add(new UserControlTourStatistics(User, entry.Key, entry.Value));
                }
            }
        }
        public int SelectedYear { get; set; }
        public View.Guide.Pages.TourStatistics TourStatisticsPage { get;set; }
        public TourStatisticsViewModel(View.Guide.Pages.TourStatistics tourStatistics, User user)
        {
            User = user;
            TourStatisticsPage = tourStatistics;
            UserControlTourStatistics = new ObservableCollection<UserControlTourStatistics>();
            MostVisited = new ObservableCollection<UserControlTourStatistics>();
            ClickInGeneralExecute();
            int currentYear = DateTime.Now.Year;
            for (int year = 2020; year <= currentYear; year++)
            {
                ComboBoxYears.Add(year);
            }
            SelectedYear = ComboBoxYears.FirstOrDefault();
        }
        public ObservableCollection<int> ComboBoxYears { get; set; } = new ObservableCollection<int>();
        public ObservableCollection<UserControlTourStatistics> UserControlTourStatistics { get; set; }
        public ObservableCollection<UserControlTourStatistics> MostVisited { get; set; }
    }
}