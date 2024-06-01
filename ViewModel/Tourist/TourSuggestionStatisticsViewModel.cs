using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Tourist;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace BookingApp.ViewModel.Tourist
{
    public class TourSuggestionStatisticsViewModel
    {
        public TourSuggestionStatistics TourSuggestionStatistics { get; set; }
        public TouristMainWindowViewModel TouristMainWindowViewModel { get; set; }
        public SeriesCollection LanguageData { get; set; } = new SeriesCollection();
        public SeriesCollection LocationData { get; set; } = new SeriesCollection();
        public RelayCommand ClickBoBack => new RelayCommand(execute => GoBackExecute());
        public RelayCommand ClickInGeneral => new RelayCommand(execute => InGeneralExecute());
        public RelayCommand ClickPrintPDF => new RelayCommand(execute => PrintPDFExecute());
        public User User;
        public bool Demo;
        public CancellationTokenSource CancellationTokenSource;
        public TourSuggestionStatisticsViewModel(TourSuggestionStatistics tourSuggestionStatistics, User user,bool demo,TouristMainWindowViewModel touristMainWindowViewModel)
        {
            TourSuggestionStatistics = tourSuggestionStatistics;
            TouristMainWindowViewModel = touristMainWindowViewModel;
            this.User = user;
            this.Demo = demo;
            AddYearsToComboBoxes();
            PopulateRequestsByLanguageGraph();
            PopulateRequestsByLocationGraph();
            InGeneralExecute();
            if (Demo)
            {
                StartDemo();
            }
        }
        public void EndDemoMode()
        {
            TourSuggestionStatistics.TourSuggestionStatisticsOverlay.Visibility = Visibility.Collapsed;
            CancellationTokenSource.Cancel();
            TouristMainWindowViewModel.EndDemoMode();
            GoBackExecute();
        }
    
        public async void StartDemo()
        {
            TourSuggestionStatistics.TourSuggestionStatisticsOverlay.Visibility = Visibility.Visible;
            CancellationTokenSource = new CancellationTokenSource();
            try
            {
                CancellationToken cancellationToken = CancellationTokenSource.Token;
                await Task.Delay(1000, cancellationToken);
                try
                {
                    TourSuggestionStatistics.Year1ComboBox.SelectedIndex = 0;
                }
                catch
                {

                }
                await Task.Delay(1000, cancellationToken);
                try
                {
                    TourSuggestionStatistics.Year2ComboBox.SelectedIndex = 0;
                }
                catch
                {
                
                }
                await Task.Delay(1000, cancellationToken);
                InGeneralExecute();
                await Task.Delay(1000, cancellationToken);
                GoBackExecute();
            }
            catch
            {
                EndDemoMode();
            }

        }
        public void AddYearsToComboBoxes()
        {
            List<String> Years = TourSuggestionService.GetInstance().GetAllTourSuggestionYears(User.Id);
            TourSuggestionStatistics.Year1ComboBox.Items.Clear();
            TourSuggestionStatistics.Year2ComboBox.Items.Clear();
            foreach (String s in Years)
            {
                TourSuggestionStatistics.Year1ComboBox.Items.Add(s);
                TourSuggestionStatistics.Year2ComboBox.Items.Add(s);
            }
        }
        public void PopulateRequestsByLanguageGraph()
        {
            foreach (string language in TourSuggestionService.GetInstance().GetAllLanguages(User.Id))
            {
                var series = new ColumnSeries
                {
                    Title = language,
                    Values = new ChartValues<int>(),
                    DataLabels = true
                };
                int requests = TourSuggestionService.GetInstance().CountRequestsByLanguage(language, User.Id);
                series.Values.Add(requests);
                LanguageData.Add(series);
            }
        }
        public void PopulateRequestsByLocationGraph()
        {
            foreach (string location in TourSuggestionService.GetInstance().GetAllLocations(User.Id))
            {
                var series = new ColumnSeries
                {
                    Title = location,
                    Values = new ChartValues<int>(),
                    DataLabels = true
                };
                int requests = TourSuggestionService.GetInstance().CountRequestsByLocation(location, User.Id);
                series.Values.Add(requests);
                LocationData.Add(series);
            }
        }
        public void Year1ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int Year = Convert.ToInt32(TourSuggestionStatistics.Year1ComboBox.SelectedItem);
            List<TourSuggestion> tourSuggestions = TourSuggestionService.GetInstance().GetAllByUserIdAndYear(User.Id, Year);
            double percentageOfToursAccepted = TourSuggestionService.GetInstance().GetPercentageOfToursAccepted(tourSuggestions);
            double percentageOfToursRejected = TourSuggestionService.GetInstance().GetPercentageOfToursRejected(tourSuggestions);
            TourSuggestionStatistics.PercentageToursAccepted.Text = percentageOfToursAccepted.ToString();
            TourSuggestionStatistics.PercentageToursRejected.Text = percentageOfToursRejected.ToString();

        }
        public void Year2ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int Year = Convert.ToInt32(TourSuggestionStatistics.Year2ComboBox.SelectedItem);
            List<TourSuggestion> tourSuggestions = TourSuggestionService.GetInstance().GetAllByUserIdAndYear(User.Id, Year);
            double averageNumberOfTouristsAccepted = TourSuggestionService.GetInstance().GetAverageNumberOfTouristsAccepted(tourSuggestions);
            TourSuggestionStatistics.AverageNumberOfTouristsAccepted.Text = averageNumberOfTouristsAccepted.ToString();
        }

        public void GoBackExecute()
        {
            TourSuggestionStatistics.Close();
        }
        public void InGeneralExecute()
        {
            TourSuggestionStatistics.Year1ComboBox.SelectedIndex = -1;
            TourSuggestionStatistics.Year2ComboBox.SelectedIndex = -1;
            List<TourSuggestion> tourSuggestions = TourSuggestionService.GetInstance().GetAllByUserId(User.Id);
            double percentageOfToursAccepted = TourSuggestionService.GetInstance().GetPercentageOfToursAccepted(tourSuggestions);
            double percentageOfToursRejected = TourSuggestionService.GetInstance().GetPercentageOfToursRejected(tourSuggestions);
            double averageNumberOfTouristsAccepted = TourSuggestionService.GetInstance().GetAverageNumberOfTouristsAccepted(tourSuggestions);
            TourSuggestionStatistics.PercentageToursAccepted.Text = percentageOfToursAccepted.ToString();
            TourSuggestionStatistics.PercentageToursRejected.Text = percentageOfToursRejected.ToString();
            TourSuggestionStatistics.AverageNumberOfTouristsAccepted.Text = averageNumberOfTouristsAccepted.ToString();
        }
        public void PrintPDFExecute()
        {

        }
    }
}
