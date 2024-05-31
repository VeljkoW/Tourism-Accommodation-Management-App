using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Windows;
using BookingApp.ViewModel.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;
using QuestPDF.Previewer;
using Notification.Wpf;

namespace BookingApp.ViewModel.Guest
{
    public class GuestCreatePdfViewModel
    {
        public INotificationManager notificationManager = App.GetNotificationManager();
        public GuestCreatePdf GuestCreatePdf { get; set; }
        public RelayCommand createPdf => new RelayCommand(execute => CreatePdf(), canExecute => CanCreatePdf());
        public GuestCreatePdfViewModel(GuestCreatePdf guestCreatePdf)
        {
            GuestCreatePdf = guestCreatePdf;
        }

        public bool CanCreatePdf()
        {
            if(string.IsNullOrEmpty(GuestCreatePdf.startDatePicker.Text) || string.IsNullOrEmpty(GuestCreatePdf.endOutDatePicker.Text) ||
                GuestCreatePdf.pdfChecked == 0)
                return false;
            return true;
        }

        public void CreatePdf()
        {
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\reservationsReport.pdf";
            DateTime startDate = GuestCreatePdf.startDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = GuestCreatePdf.endOutDatePicker.SelectedDate ?? DateTime.Now;

            List<ReportOnReservations> reserved = ReportOnReservationsService.GetInstance().GetAll().Where(t => t.TypeReport == "Reserved" && t.Date>=startDate && t.Date<=endDate).ToList();
            List<ReportOnReservations> cancelled = ReportOnReservationsService.GetInstance().GetAll().Where(t => t.TypeReport == "Cancelled" && t.Date >= startDate && t.Date <= endDate).ToList();


            //notificationManager.Show("Success", "Pdf Successfully created!", NotificationType.Success);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    //page.Header().Height(100).Background(Colors.Grey.Lighten1);
                    //page.Content().Background(Colors.Grey.Lighten3);
                    //page.Footer().Height(50).Background(Colors.Grey.Lighten1);


                    page.Header()
                        .Column(column =>
                        {
                            column.Item()
                                .Text("Reservations Report")
                                .FontSize(30)
                                .FontColor(Colors.Black)
                                .SemiBold()
                                .AlignCenter();

                            column.Item()
                                .Row(row =>
                                {

                                });
                        });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text(Placeholders.LoremIpsum());
                            x.Item().Image(Placeholders.Image(200, 100));
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });
            document.GeneratePdf(downloadsPath);
            document.ShowInPreviewer();

        }
    }
}
