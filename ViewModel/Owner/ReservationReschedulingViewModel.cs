using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;
using Notification.Wpf;

namespace BookingApp.ViewModel.Owner
{
    public class ReservationReschedulingViewModel
    {
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";
        public INotificationManager notificationManager = App.GetNotificationManager();
        public RelayCommand AcceptClick => new RelayCommand(execute => AcceptExecute(), canExecute => CanExecute());
        public RelayCommand DeclineClick => new RelayCommand(execute=> DeclineExecute(), canExecute => CanExecute()); 
        public RelayCommand DownloadReservationReportCommand => new RelayCommand(execute => DownloadReservationReportExecute(), canExecute => DownloadReservationReportCanExecute());
        public ObservableCollection<GuestReschedulingRequest> GuestReschedulingRequests { get; set; }
        public GuestReschedulingRequest SelectedGuestReschedulingRequest {  get; set; }
        public User user { get; set; }
        public ReservationRescheduling ReservationRescheduling {  get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public ObservableCollection<ReservedAccommodation> ReservedAccommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public ReservationReschedulingViewModel(ReservationRescheduling ReservationRescheduling, User user)
        {
            this.user = user;
            this.ReservationRescheduling = ReservationRescheduling;
            GuestReschedulingRequests = new ObservableCollection<GuestReschedulingRequest>();
            ReservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            Accommodations = AccommodationService.GetInstance().GetAllByUser(user);
            Update();
        }
        public void DownloadReservationReportExecute()
        {
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\"+SelectedAccommodation.Name+" Reservation Report.pdf";
            DateTime startDate = ReservationRescheduling.StartDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = ReservationRescheduling.EndDatePicker.SelectedDate ?? DateTime.Now;
            endDate.AddDays(1);

            List<ReservedAccommodation> reservedAccommodations = ReservedAccommodationService.GetInstance().GetAll().Where(t => t.Accommodation.Id == SelectedAccommodation.Id).ToList();
            List<ReservedAccommodation> reservations = new List<ReservedAccommodation>();
            foreach (ReservedAccommodation reservedAccommodation in reservedAccommodations)
                if (reservedAccommodation.CheckInDate >= startDate && reservedAccommodation.CheckOutDate <= endDate)
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
                                .Text("Reservations Report\n")
                                .FontSize(30)
                                .FontColor(Colors.Grey.Darken2)
                                .SemiBold()
                                .AlignCenter();

                            column.Item()
                                .Text($"Accommodation: {SelectedAccommodation.Name}, {SelectedAccommodation.Location.State} - {SelectedAccommodation.Location.City}")
                                .FontSize(14)
                                .FontColor(Colors.BlueGrey.Medium);

                            column.Item()
                                .Text($"Start date: {startDate.ToString("dd.MM.yyyy")}")
                                .FontSize(14)
                                .FontColor(Colors.BlueGrey.Medium);

                            column.Item()
                                .Text($"End date: {endDate.ToString("dd.MM.yyyy")}")
                                .FontSize(14)
                                .FontColor(Colors.BlueGrey.Medium);
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
                                header.Cell().Element(CellStyle).Text("#").FontSize(14);
                                header.Cell().Element(CellStyle).Text("Guest username").FontSize(14);
                                header.Cell().Element(CellStyle).Text("Check-in date").FontSize(14);
                                header.Cell().Element(CellStyle).Text("Check-out date").FontSize(14);

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

                                var backgroundColor = i % 2 == 0 ? Colors.LightBlue.Lighten5 : Colors.Grey.Lighten2;

                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(i.ToString());
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(user.Username);
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(checkInDateFormatted);
                                table.Cell().Element(CellStyle).Background(backgroundColor).Text(checkOutDateFormatted);

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.PaddingBottom(3).PaddingTop(3);
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

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = downloadsPath,
                UseShellExecute = true
            });
            if (App.currentLanguage() == ENG)
                notificationManager.Show("Success!", "Reservation report successfully downloaded!", NotificationType.Success);
            else
                notificationManager.Show("Uspeh!", "Izveštaj za rezervacije uspešno preuzet!", NotificationType.Success);
        }
        public bool DownloadReservationReportCanExecute()
        {
            if(SelectedAccommodation == null ||
                ReservationRescheduling.StartDatePicker.SelectedDate == null ||
                ReservationRescheduling.EndDatePicker.SelectedDate == null)
                return false;
            return true;
        }
        public void AccommodationSelectionChanged()
        {
            ReservedAccommodations.Clear();
            List<ReservedAccommodation> AllReservedAccommodations = new List<ReservedAccommodation>();
            AllReservedAccommodations = ReservedAccommodationService.GetInstance().GetByAccommodationId(SelectedAccommodation.Id);
            if (AllReservedAccommodations.Count == 0 || AllReservedAccommodations == null) 
                return;
            foreach(ReservedAccommodation reservedAccommodation in AllReservedAccommodations)
            {
                if(reservedAccommodation.CheckOutDate >= DateTime.Now)
                    ReservedAccommodations.Add(reservedAccommodation);
            }
            for (int i = 0; i < ReservedAccommodations.Count - 1; i++)
            {
                for (int j = i + 1; j < ReservedAccommodations.Count; j++)
                {
                    if (ReservedAccommodations[i].CheckInDate > ReservedAccommodations[j].CheckInDate)
                    {
                        ReservedAccommodation tempReservedAccommodation = new ReservedAccommodation(ReservedAccommodations[i]);
                        ReservedAccommodations[i] = ReservedAccommodations[j];
                        ReservedAccommodations[j] = tempReservedAccommodation;
                    }
                }
            }
        }
        public void Update()
        {
            GuestReschedulingRequests.Clear();
            UpdateGuestReschedulingRequests();
            GuestReschedulingRequestAvailability();
            if(GuestReschedulingRequests.Count <= 0)
            {
                if (App.currentLanguage() == ENG)
                    notificationManager.Show("Info", "There are no reservation request to accept/decline", NotificationType.Information);
                else
                    notificationManager.Show("Info", "ne postoje zahtevi za renoviranje na koje treba da se odgovori", NotificationType.Information);
            }
        }
        public void UpdateGuestReschedulingRequests()
        {
            foreach (GuestReschedulingRequest guestReschedulingRequest in GuestReschedulingRequestService.GetInstance().GetAll())
            {
                foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
                {
                    if (accommodation.Id == guestReschedulingRequest.AccommodationId && accommodation.OwnerId == user.Id)
                    {
                        GuestReschedulingRequests.Add(guestReschedulingRequest);
                    }
                }
            }
        }
        public void GuestReschedulingRequestAvailability()
        {
            foreach (GuestReschedulingRequest GuestReschedulingRequest in GuestReschedulingRequests)
            {
                if (isFreeSlot(GuestReschedulingRequest))
                    GuestReschedulingRequest.ImagePath = "../../Resources/Images/Owner/check-box.png";
                else
                    GuestReschedulingRequest.ImagePath = "../../Resources/Images/Owner/x-box.png";

            }
        }
        public bool isFreeSlot(GuestReschedulingRequest GuestReschedulingRequest)
        {
            List<ReservedAccommodation> reservedAccommodations = ReservedAccommodationService.GetInstance().GetAll().Where(t => t.Accommodation.Id == GuestReschedulingRequest.AccommodationId).ToList();
            for (DateTime date = GuestReschedulingRequest.CheckInDate; date <= GuestReschedulingRequest.CheckOutDate; date = date.AddDays(1))
            {
                foreach (ReservedAccommodation reservedAccommodation in reservedAccommodations)
                {
                    if (reservedAccommodation.Id != GuestReschedulingRequest.ReservedAccommodationId &&
                        date > reservedAccommodation.CheckInDate &&
                        date < reservedAccommodation.CheckOutDate)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void AcceptExecute() { ProcessReschedulingRequest(true); }
        public void DeclineExecute() { ProcessReschedulingRequest(false); }
        public bool CanExecute()
        {
            if (SelectedGuestReschedulingRequest == null)
                return false;
            else
                return true;
        }
        public void ProcessReschedulingRequest(bool accepted)
        {
            ProcessedReschedulingRequest processedReschedulingRequest = new ProcessedReschedulingRequest();
            if (!string.IsNullOrEmpty(ReservationRescheduling.CommentTextBox.Text))
            {
                Comment comment = new Comment();
                comment.Text = ReservationRescheduling.CommentTextBox.Text;
                comment.CreationTime = DateTime.Now;
                comment.User = UserService.GetInstance().GetById(user.Id);
                comment = CommentService.GetInstance().Save(comment);
                processedReschedulingRequest.CommentId = comment.Id;
            }
            AcceptedReservationRescheduling acceptedReservationRescheduling = new AcceptedReservationRescheduling();

            acceptedReservationRescheduling.AccommodationId = SelectedGuestReschedulingRequest.AccommodationId;
            acceptedReservationRescheduling.GuestId = SelectedGuestReschedulingRequest.GuestId;
            acceptedReservationRescheduling.AcceptedDate = DateTime.Now;

            AcceptedReservationReschedulingService.GetInstance().Add(acceptedReservationRescheduling);

            processedReschedulingRequest.AccommodationId = SelectedGuestReschedulingRequest.AccommodationId;
            processedReschedulingRequest.GuestId = SelectedGuestReschedulingRequest.GuestId;
            processedReschedulingRequest.IsAccepted = accepted;
            processedReschedulingRequest.CheckInDate = SelectedGuestReschedulingRequest.CheckInDate;
            processedReschedulingRequest.CheckOutDate = SelectedGuestReschedulingRequest.CheckOutDate;
            ProcessedReschedulingRequestService.GetInstance().Add(processedReschedulingRequest);
            GuestReschedulingRequestService.GetInstance().DeleteById(SelectedGuestReschedulingRequest.Id);
            if (accepted)
            {
                ReservedAccommodationService.GetInstance().UpdateDatesByReschedulingRequest(SelectedGuestReschedulingRequest);
                if (App.currentLanguage() == ENG)
                    notificationManager.Show("Success!", "Reservation request successfully accepted!", NotificationType.Success);
                else
                    notificationManager.Show("Uspeh!", "Zahtev za rezervaciju uspešno prihvaćen!", NotificationType.Success);
            }
            else
            {
                if (App.currentLanguage() == ENG)
                    notificationManager.Show("Success!", "Reservation request successfully declined!", NotificationType.Success);
                else
                    notificationManager.Show("Uspeh!", "Zahtev za rezervaciju uspešno odbijen!", NotificationType.Success);
            }
            Update();
        }
    }
}
