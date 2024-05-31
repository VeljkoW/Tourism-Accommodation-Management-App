using BookingApp.ViewModel.Owner;
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
using BookingApp.Domain.Model;
using BookingApp.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;
using QuestPDF.Previewer;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for ReservationRescheduling.xaml
    /// </summary>
    public partial class ReservationRescheduling : Page
    {
        public User User { get; set; }
        public ReservationReschedulingViewModel ReservationReschedulingViewModel { get; set; }
        public ReservationRescheduling(User User)
        {
            InitializeComponent();
            this.User = User;
            ReservationReschedulingViewModel = new ReservationReschedulingViewModel(this, User);
            DataContext = ReservationReschedulingViewModel;
        }

        private void AccommodationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReservationReschedulingViewModel.AccommodationSelectionChanged();
        }

        private void DownloadReservationReportClick(object sender, RoutedEventArgs e)
        {
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\hello.pdf";
            DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.Now;
            endDate.AddDays(1);

            List<ReservedAccommodation> reservedAccommodations = ReservedAccommodationService.GetInstance().GetAll().Where(t => t.Accommodation.Id == ReservationReschedulingViewModel.SelectedAccommodation.Id).ToList();
            List<ReservedAccommodation> reservations = new List<ReservedAccommodation>();
            foreach (ReservedAccommodation reservedAccommodation in reservedAccommodations)
                if(reservedAccommodation.CheckInDate >= startDate && reservedAccommodation.CheckOutDate <= endDate)
                    reservations.Add(reservedAccommodation);

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
                                .FontColor(Colors.Blue.Medium)
                                .SemiBold();

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
