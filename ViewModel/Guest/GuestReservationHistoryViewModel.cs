using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Guest.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Res = BookingApp.View.Guest.Pages.GuestReservations;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationHistoryViewModel
    {
        public User user { get; set; }

        public ReservationHistory ReservationHistory { get; set; }

        public ReservedAccommodation reservedAccommodation;

        public ReservedAccommodation selectedAccommodation;
        public ObservableCollection<ReservedAccommodation> reservedAccommodations { get; set; }
        public RelayCommand ReservationsTab => new RelayCommand(execute => ReservationsTabClick());
        public RelayCommand ReschedulingTab => new RelayCommand(execute => ReschedulingTabClick());
        public RelayCommand HistoryTab => new RelayCommand(execute => HistoryTabClick());
        public RelayCommand FirstCardSelect => new RelayCommand(execute => FirstCardSelectClick());

        public bool IsRateButtonVisible {  get; set; }
        public GuestMainWindow GuestMainWindow { get; set; }

       
        public GuestReservationHistoryViewModel(ReservationHistory reservationHistory, User user) 
        {
            ReservationHistory = reservationHistory;
            this.user = user; 
            reservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            foreach (ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().Update(user))
                if (reservedAccommodation.CheckOutDate <= DateTime.Now)
                    reservedAccommodations.Add(reservedAccommodation);
        }
        public void ReservationsTabClick()
        {
            Res Reservations = new Res(user, ReservationHistory.GuestMainWindow);
            ReservationHistory.GuestMainWindow.mainFrame.Navigate(Reservations);
        }
        public void ReschedulingTabClick()
        {
            ReschedulingStatuses reschedulingStatuses = new ReschedulingStatuses(user, ReservationHistory.GuestMainWindow);
            ReservationHistory.GuestMainWindow.mainFrame.Navigate(reschedulingStatuses);
        }
        public void HistoryTabClick()
        {
            ReservationHistory reservationHistory = new ReservationHistory(user, ReservationHistory.GuestMainWindow);
            ReservationHistory.GuestMainWindow.mainFrame.Navigate(reservationHistory);
        }
        private void SelectFirstCard()
        {
            var container = ReservationHistory.reservationsItems.ItemContainerGenerator.ContainerFromIndex(0) as ContentPresenter;
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

    }
}
