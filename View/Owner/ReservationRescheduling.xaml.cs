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
                    page.DefaultTextStyle(x => x.FontSize(12));


                    page.Header()
                        .Column(column =>
                        {
                            column.Item()
                                .Text("Reservations Report")
                                .FontSize(30)
                                .FontColor(Colors.Blue.Medium)
                                .SemiBold()
                                .AlignCenter();

                            column.Item()
                                .Text($"Accommodation: {ReservationReschedulingViewModel.SelectedAccommodation.Name}, {ReservationReschedulingViewModel.SelectedAccommodation.Location.State} - {ReservationReschedulingViewModel.SelectedAccommodation.Location.City}")
                                .FontSize(16)
                                .FontColor(Colors.BlueGrey.Medium)
                                .SemiBold();

                            column.Item()
                                .Text($"Start date: {startDate.ToString("dd.MM.yyyy hhtt")}")
                                .FontSize(16)
                                .FontColor(Colors.BlueGrey.Medium)
                                .SemiBold();

                            column.Item()
                                .Text($"End date: {endDate.ToString("dd.MM.yyyy hhtt")}")
                                .FontSize(16)
                                .FontColor(Colors.BlueGrey.Medium)
                                .SemiBold();
                        });
                    if (reservations.Count() > 0)
                    {
                        page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("#");
                                header.Cell().Element(CellStyle).Text("Guest username");
                                header.Cell().Element(CellStyle).Text("Check-in date");
                                header.Cell().Element(CellStyle).Text("Check-out date");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingBottom(5).PaddingTop(5).Background(Colors.Grey.Medium);
                                }
                            });
                            for (int i = 0; i < reservations.Count(); i++)
                            {
                                User? user = UserService.GetInstance().GetById(reservations[i].GuestId);
                                string checkInDateFormatted = reservations[i].CheckInDate.ToString("dd.MM.yyyy hhtt");
                                string checkOutDateFormatted = reservations[i].CheckOutDate.ToString("dd.MM.yyyy hhtt");

                                var backgroundColor = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten3;

                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(i.ToString());
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(user.Username);
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(checkInDateFormatted);
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(checkOutDateFormatted);

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.PaddingBottom(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                                }
                            }
                        });
                    }
                    else
                    {
                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Text("There are no reservations in this period for this accommodation")
                            .FontSize(16)
                            .SemiBold();
                    }

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
        }

        private void ReservationRequestsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ReservationRequestsTable.SelectedItem != null)
            {
                CancelRenovationAccept.Visibility = Visibility.Collapsed;
            }
            else
            {
                CancelRenovationAccept.Visibility = Visibility.Visible;
            }
        }
    }
}
