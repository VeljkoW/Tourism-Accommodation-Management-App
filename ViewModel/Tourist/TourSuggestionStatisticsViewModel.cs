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
using Microsoft.Win32;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

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
        public string GetSaveFilePath()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF";
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }
        public void PrintPDFExecute()
        {
            string filePath = GetSaveFilePath();
            if (!string.IsNullOrEmpty(filePath))
            {
                GeneratePDF(filePath);
                MessageBox.Show($"PDF generated successfully at {filePath}");
            }
        }
        public void GeneratePDF(string filePath)
        {
            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                page.Orientation = PdfSharpCore.PageOrientation.Portrait;

                const double leftMargin = 40;
                const double rightMargin = 40;
                const double topMargin = 40;
                const double bottomMargin = 40;

                var borderLeft = leftMargin - 10;
                var borderTop = topMargin - 10;
                var borderRight = page.Width - rightMargin + 10;
                var borderBottom = page.Height - bottomMargin + 10; 

                using (var gfx = XGraphics.FromPdfPage(page))
                {
                    var font = new XFont("Arial", 12, XFontStyle.Bold);
                    var TitleFont = new XFont("Arial", 20, XFontStyle.Bold);
                    var brush = XBrushes.Black;
                    var accentBrush = XBrushes.Red;

                    var borderPen = new XPen(XColors.Black, 2);
                    gfx.DrawRectangle(borderPen, borderLeft, borderTop, borderRight - borderLeft, borderBottom - borderTop);

                    var titleFormat = new XStringFormat();
                    titleFormat.Alignment = XStringAlignment.Center;
                    gfx.DrawString("Tour Suggestion Statistics", TitleFont, accentBrush, new XRect(0, topMargin, page.Width, 20), XStringFormats.Center);

                    gfx.DrawLine(borderPen, borderLeft, topMargin + 29, borderRight, topMargin + 29);

                    string yearText1 = string.IsNullOrEmpty(TourSuggestionStatistics.Year1ComboBox.Text) ? "general" : TourSuggestionStatistics.Year1ComboBox.Text;
                    string yearText2 = string.IsNullOrEmpty(TourSuggestionStatistics.Year2ComboBox.Text) ? "general" : TourSuggestionStatistics.Year2ComboBox.Text;

                    var yPos = topMargin + 55;
                    gfx.DrawString($"Percentage of Tours Accepted in {yearText1}: {TourSuggestionStatistics.PercentageToursAccepted.Text} %", font, brush, new XPoint(leftMargin, yPos));
                    yPos += 20;
                    gfx.DrawString($"Percentage of Tours Rejected in {yearText1}: {TourSuggestionStatistics.PercentageToursRejected.Text} %", font, brush, new XPoint(leftMargin, yPos));
                    yPos += 20;
                    gfx.DrawString($"Average Number of Tourists Accepted in {yearText2}: {TourSuggestionStatistics.AverageNumberOfTouristsAccepted.Text}", font, brush, new XPoint(leftMargin, yPos));

                    yPos += 40; 
                    var flavorFont = new XFont("Arial", 10, XFontStyle.Regular);
                    var flavorBrush = XBrushes.Gray;
                    gfx.DrawString("Thank you for using our tour statistics service.", flavorFont, flavorBrush, new XPoint(leftMargin, yPos));
                    yPos += 15;
                    gfx.DrawString("We strive to provide the best tour experiences based on your preferences and feedback.", flavorFont, flavorBrush, new XPoint(leftMargin, yPos));
                    yPos += 15;
                    gfx.DrawString("If you have any questions or need further assistance, please contact our support team.", flavorFont, flavorBrush, new XPoint(leftMargin, yPos));
                    yPos += 15;
                    gfx.DrawString("We hope you enjoy your tours and look forward to serving you again!", flavorFont, flavorBrush, new XPoint(leftMargin, yPos));
                    yPos += 15;
                    gfx.DrawString("Best regards,", flavorFont, flavorBrush, new XPoint(leftMargin, yPos));
                    yPos += 15;
                    gfx.DrawString("The Tour Statistics Team", flavorFont, flavorBrush, new XPoint(leftMargin, yPos));
                }

                document.Save(filePath);
            }
        }
    }
}
