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
using System.Windows.Media;

namespace BookingApp.ViewModel.Guest
{
    public class GuestCreatePdfViewModel
    {
        public INotificationManager notificationManager = App.GetNotificationManager();
        public GuestCreatePdf GuestCreatePdf { get; set; }

        public User User { get; set; }
        public RelayCommand Exit => new RelayCommand(execute => CloseWindow());
        public RelayCommand FocusStartDatePicker => new RelayCommand(execute => FocusStartDate());
        public RelayCommand FocusEndDatePicker => new RelayCommand(execute => FocusEndDate());
        public RelayCommand FocusReservedRadioButton => new RelayCommand(execute => FocusReservedRadio());
        public RelayCommand FocusCancelledRadioButton => new RelayCommand(execute => FocusCancelledRadio());
        public RelayCommand createPdf => new RelayCommand(execute => CreatePdf(), canExecute => CanCreatePdf());
        public GuestCreatePdfViewModel(GuestCreatePdf guestCreatePdf, User user)
        {
            GuestCreatePdf = guestCreatePdf;
            User = user;
        }
        public void CloseWindow()
        {
            GuestCreatePdf.Close();
        }
        public void FocusCancelledRadio()
        {
            GuestCreatePdf.PdfRadioButton2.IsChecked = true;
        }
        public void FocusReservedRadio() 
        {
            GuestCreatePdf.PdfRadioButton1.IsChecked = true;
        }
        public void FocusStartDate()
        {
            GuestCreatePdf.startDatePicker.IsDropDownOpen = true;
        }
        public void FocusEndDate()
        {
            GuestCreatePdf.endDatePicker.IsDropDownOpen = true;
        }
        public bool CanCreatePdf()
        {
            if (string.IsNullOrEmpty(GuestCreatePdf.startDatePicker.Text) || string.IsNullOrEmpty(GuestCreatePdf.endDatePicker.Text) ||
                GuestCreatePdf.pdfChecked == 0)
                return false;
            else
            {
                GuestCreatePdf.startDatePicker.Focusable = false;
                GuestCreatePdf.endDatePicker.Focusable = false;
                //GuestCreatePdf.CreateButton.Focus();
                return true;
            }
        }

        public void CreatePdf()
        {
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\reservationsReport.pdf";
            DateTime startDate = GuestCreatePdf.startDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = GuestCreatePdf.endDatePicker.SelectedDate ?? DateTime.Now;

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
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Column(column =>
                        {
                            column.Item()
                               .Text("Booking App")
                               .FontSize(15)
                               .FontColor(Colors.Black)
                               .AlignLeft();

                            column.Item()
                               .Text($"Guest: {User.Username}")
                               .FontSize(15)
                               .FontColor(Colors.Black)
                               .AlignLeft();

                            column.Item()
                               .Text($"Date: {DateTime.Now.ToString("dd/MM/yyyy")}")
                               .FontSize(15)
                               .FontColor(Colors.Black)
                               .AlignLeft();

                            if(GuestCreatePdf.pdfChecked == 1)
                            {
                                column.Item()
                                .Text($"Checked dates: {startDate.ToString("dd/MM/yyyy")} - {endDate.ToString("dd/MM/yyyy")}")
                                .FontSize(15)
                                .FontColor(Colors.Black)
                                .AlignLeft();

                                column.Item()
                               .Text("Report for reserved accommodations\n")
                               .FontSize(15)
                               .FontColor(Colors.Black)
                               .AlignLeft();

                            }
                            else
                            {
                                column.Item()
                               .Text($"Checked dates: {startDate.ToString("dd/MM/yyyy")} - {endDate.ToString("dd/MM/yyyy")}")
                               .FontSize(15)
                               .FontColor(Colors.Black)
                               .AlignLeft();

                                column.Item()
                               .Text("Report for cancelled accommodations\n")
                               .FontSize(15)
                               .FontColor(Colors.Black)
                               .AlignLeft();

                                
                            }

                            column.Item()
                                .Text("Reservations Report")
                                .FontSize(20)
                                .FontColor(Colors.Black)
                                .AlignCenter();

                            column.Item()
                                .Row(row =>
                                {

                                });
                        });
                    if (GuestCreatePdf.pdfChecked == 1 && reserved.Count > 0)
                    {
                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .BorderColor(Colors.Black)
                        .Table(table =>
                        {

                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("#").AlignCenter();
                                header.Cell().Element(CellStyle).Text("Accommodation").AlignCenter();
                                header.Cell().Element(CellStyle).Text("Location").AlignCenter();
                                header.Cell().Element(CellStyle).Text("Check In Date").AlignCenter();
                                header.Cell().Element(CellStyle).Text("Check Out Date").AlignCenter();

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingBottom(2).BorderBottom(1).BorderTop(1).BorderLeft(1).BorderRight(1).Background(Colors.Grey.Medium);
                                }
                            });
                            for (int i = 0; i < reserved.Count(); i++)
                            {
                                var backgroundColor = i % 2 == 0 ? Colors.Grey.Lighten3 : Colors.Yellow.Lighten3;

                                table.Cell().Element(CellStyle).Background(backgroundColor).Text((i + 1).ToString()).AlignCenter();
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(AccommodationService.GetInstance().GetById(reserved[i].AccommodationId).Name).AlignCenter();
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(AccommodationService.GetInstance().GetById(reserved[i].AccommodationId).Location.State + " - " + AccommodationService.GetInstance().GetById(reserved[i].AccommodationId).Location.City).AlignCenter();
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(ReservedAccommodationService.GetInstance().GetById(reserved[i].ReservedId).CheckInDate.ToString("dd/MM/yyyy")).AlignCenter();
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(ReservedAccommodationService.GetInstance().GetById(reserved[i].ReservedId).CheckOutDate.ToString("dd/MM/yyyy")).AlignCenter();

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.PaddingBottom(2).PaddingTop(2).BorderBottom(1).BorderTop(1).BorderLeft(1).BorderRight(1).BorderColor(Colors.Black);
                                }
                            }
                        });
                        
                       
                    }
                    else if (GuestCreatePdf.pdfChecked == 2 && cancelled.Count > 0)
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
                            });
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("#").AlignCenter();
                                header.Cell().Element(CellStyle).Text("Accommodation").AlignCenter();
                                header.Cell().Element(CellStyle).Text("Location").AlignCenter();

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingBottom(2).BorderBottom(1).BorderTop(1).BorderLeft(1).BorderRight(1).Background(Colors.Grey.Medium);
                                }
                            });

                            for (int i = 0; i < cancelled.Count(); i++)
                            {
                                var backgroundColor = i % 2 == 0 ? Colors.Grey.Lighten3 : Colors.Yellow.Lighten3;

                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(i.ToString()).AlignCenter();
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(AccommodationService.GetInstance().GetById(cancelled[i].AccommodationId).Name).AlignCenter();
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(AccommodationService.GetInstance().GetById(cancelled[i].AccommodationId).Location.State + " - " + AccommodationService.GetInstance().GetById(cancelled[i].AccommodationId).Location.City).AlignCenter();

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.PaddingBottom(2).PaddingTop(2).BorderBottom(1).BorderTop(1).BorderLeft(1).BorderRight(1).BorderColor(Colors.Black);
                                }
                            }
                        });

                    }
                    else
                    {
                        page.Content()
                       .PaddingVertical(1, Unit.Centimetre)
                       .Text("For this period, there is no data!")
                       .FontSize(16)
                       .SemiBold()
                       .AlignCenter();

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

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = downloadsPath,
                UseShellExecute = true
            });

        }
    }
}
