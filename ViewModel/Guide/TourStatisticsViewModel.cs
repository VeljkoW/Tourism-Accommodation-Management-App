using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using VirtualKeyboard.Wpf;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace BookingApp.ViewModel.Guide
{
    public class TourStatisticsViewModel
    {
        public User User { get;set; }
        public RelayCommand ClickInGeneral => new RelayCommand(execute => ClickInGeneralExecute());
        public RelayCommand ClickSelectedYear => new RelayCommand(execute => ClickSelectedYearExecute());
        public RelayCommand ClickGoBack => new RelayCommand(execute => ClickGoBackExecute());
        public RelayCommand GeneratePDF => new RelayCommand(execute => GeneratePDFExecute());

        private void GeneratePDFExecute()
        {
            Dictionary<Tour, Domain.Model.TourStatistics> statistics = TourService.GetInstance().GetTourStatistics();

            using (MemoryStream stream = new MemoryStream())
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document();
                try
                {
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    Font titleFont = FontFactory.GetFont("Arial", 36, Font.BOLD, BaseColor.DARK_GRAY);
                    iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("Tour Statistics Report", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 30f;
                    document.Add(title);

                    PdfPTable table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.SpacingAfter = 20f;
                    table.SetWidths(new float[] { 2f, 1f, 1f, 1f, 1f });

                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD, BaseColor.WHITE);
                    PdfPCell headerCell1 = new PdfPCell(new Phrase("Tour Name", headerFont));
                    PdfPCell headerCell2 = new PdfPCell(new Phrase("Underage", headerFont));
                    PdfPCell headerCell3 = new PdfPCell(new Phrase("Adult", headerFont));
                    PdfPCell headerCell4 = new PdfPCell(new Phrase("Elderly", headerFont));
                    PdfPCell headerCell5 = new PdfPCell(new Phrase("Visitors", headerFont));

                    PdfPCell[] headerCells = { headerCell1, headerCell2, headerCell3, headerCell4, headerCell5 };
                    foreach (var cell in headerCells)
                    {
                        cell.BackgroundColor = new BaseColor(79, 129, 189);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }

                    Font cellFont = FontFactory.GetFont("Arial", 16, BaseColor.BLACK);
                    foreach (var stat in statistics)
                    {
                        Tour tour = stat.Key;
                        Domain.Model.TourStatistics stats = stat.Value;

                        PdfPCell cell1 = new PdfPCell(new Phrase(tour.Name, cellFont));
                        PdfPCell cell2 = new PdfPCell(new Phrase(stats.Underage.ToString(), cellFont));
                        PdfPCell cell3 = new PdfPCell(new Phrase(stats.Adult.ToString(), cellFont));
                        PdfPCell cell4 = new PdfPCell(new Phrase(stats.Elderly.ToString(), cellFont));
                        PdfPCell cell5 = new PdfPCell(new Phrase(stats.Visitors.ToString(), cellFont));

                        PdfPCell[] dataCells = { cell1, cell2, cell3, cell4, cell5 };
                        foreach (var cell in dataCells)
                        {
                            cell.Padding = 10f;
                            cell.BackgroundColor = new BaseColor(210, 230, 255);
                            table.AddCell(cell);
                        }
                    }

                    document.Add(table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating PDF: {ex.Message}");
                    return;
                }
                finally
                {
                    document.Close();
                }

                byte[] pdfBytes = stream.ToArray();

                OfferPdfAsDownload(pdfBytes, "TourStatisticsReport.pdf");
            }
        }
        private void OfferPdfAsDownload(byte[] pdfBytes, string fileName)
        {
            // Prompt user to save the PDF file
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = fileName; // Default file name
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "PDF documents (.pdf)|*.pdf"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                File.WriteAllBytes(dlg.FileName, pdfBytes);
                MessageBox.Show("PDF file downloaded successfully.");
            }
        }

        private void ClickGoBackExecute(){
            TourStatisticsPage.NavigationService.GoBack();
        }
        public void ClickSelectedYearExecute()
        {
            if (SelectedYear == "All")
            {
                ClickInGeneralExecute();
                return;
            }
            UserControlTourStatistics.Clear();
            MostVisited.Clear();
            int year = Convert.ToInt32(TourStatisticsPage.YearComboBox.SelectedValue);
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
        public string SelectedYear { get; set; }
        public View.Guide.Pages.TourStatistics TourStatisticsPage { get;set; }
        public TourStatisticsViewModel(View.Guide.Pages.TourStatistics tourStatistics, User user)
        {
            User = user;
            TourStatisticsPage = tourStatistics;
            UserControlTourStatistics = new ObservableCollection<UserControlTourStatistics>();
            MostVisited = new ObservableCollection<UserControlTourStatistics>();
            ClickInGeneralExecute();
            int currentYear = DateTime.Now.Year;
            ComboBoxYears.Add("All");
            for (int year = 2020; year <= currentYear; year++)
            {
                ComboBoxYears.Add(year.ToString());
            }
            SelectedYear = ComboBoxYears.LastOrDefault();
        }
        public ObservableCollection<string> ComboBoxYears { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<UserControlTourStatistics> UserControlTourStatistics { get; set; }
        public ObservableCollection<UserControlTourStatistics> MostVisited { get; set; }
    }
}