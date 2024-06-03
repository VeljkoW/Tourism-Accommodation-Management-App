using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.View.Guest.Pages;
using System.Collections.ObjectModel;
using BookingApp.Services;
using Guests = BookingApp.View.Guest.Pages.GuestReservations;
using System.Reflection.Metadata.Ecma335;
using BookingApp.View.Guest.Windows;
using System.Windows;
using BookingApp.View.Guest;
using Reservations = BookingApp.View.Guest.Pages.GuestReservations;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationsViewModel
    {
        public Guests guestReservations { get; set; }

        public ObservableCollection<ReservedAccommodation> reservedAccommodations { get; set; }
        public RelayCommand ReservationsTab => new RelayCommand(execute => ReservationsTabClick());
        public RelayCommand ReschedulingTab => new RelayCommand(execute => ReschedulingTabClick());
        public RelayCommand HistoryTab => new RelayCommand(execute => HistoryTabClick());
        public RelayCommand FirstCardSelect => new RelayCommand(execute => FirstCardSelectClick());
        public RelayCommand RescheduleB => new RelayCommand(execute => RescheduleBClick());
        public RelayCommand DeleteB => new RelayCommand(execute => DeleteBClick());
        public RelayCommand PdfCreate => new RelayCommand(execute => PdfCreateClick());
        public User user { get; set; }
        public GuestReservationsViewModel(Guests guestReservations, User user)
        {
            this.user = user;
            reservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            this.guestReservations = guestReservations;

            foreach(ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().Update(user))
                if(reservedAccommodation.CheckOutDate > DateTime.Now)
                    reservedAccommodations.Add(reservedAccommodation);
        }
        public void ReservationsTabClick()
        {
            Reservations Reservations = new Reservations(user, guestReservations.GuestMainWindow);
            guestReservations.GuestMainWindow.mainFrame.Navigate(Reservations);
        }
        public void ReschedulingTabClick()
        {
            ReschedulingStatuses reschedulingStatuses = new ReschedulingStatuses(user, guestReservations.GuestMainWindow);
            guestReservations.GuestMainWindow.mainFrame.Navigate(reschedulingStatuses);
        }
        public void HistoryTabClick()
        {
            ReservationHistory reservationHistory = new ReservationHistory(user, guestReservations.GuestMainWindow);
            guestReservations.GuestMainWindow.mainFrame.Navigate(reservationHistory);
        }
        private void SelectFirstCard()
        {
            var container = guestReservations.reservationsItems.ItemContainerGenerator.ContainerFromIndex(0) as ContentPresenter;
            if (container != null)
            {
                var contentTemplate = container.ContentTemplate;
                var textBlock = contentTemplate.FindName("BorderBlock", container) as Border;
                if (textBlock != null)
                {
                    Keyboard.Focus(textBlock); // Focus the TextBlock or other inner element
                }
            }
        }
        public void FirstCardSelectClick()
        {
            SelectFirstCard();
        }
        public void RescheduleBClick()
        {
            
        }
        public void DeleteBClick()
        { 
        
        }
        public void PdfCreateClick()
        {
            GuestCreatePdf guestCreatePdf = new GuestCreatePdf(user);
            guestCreatePdf.Show();
        }

    }
}
