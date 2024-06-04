using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Guest.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Res = BookingApp.View.Guest.Pages.GuestReservations;

namespace BookingApp.ViewModel.Guest
{
    public class ReschedulingStatusesViewModel
    {
        public User User { get; set; }

        public ReschedulingStatuses ReschedulingStatuses { get; set; }
        public ObservableCollection<GuestReschedulingRequest> guestReschedulingRequests { get; set; }
        public RelayCommand ReservationsTab => new RelayCommand(execute => ReservationsTabClick());
        public RelayCommand ReschedulingTab => new RelayCommand(execute => ReschedulingTabClick());
        public RelayCommand HistoryTab => new RelayCommand(execute => HistoryTabClick());
        public RelayCommand FirstCardSelect1 => new RelayCommand(execute => FirstCardSelectClick1());
        public RelayCommand FirstCardSelect2 => new RelayCommand(execute => FirstCardSelectClick2());
        public ObservableCollection<ProcessedReschedulingRequest> processedReschedulingRequests { get; set; }
        public ReschedulingStatusesViewModel(ReschedulingStatuses reschedulingStatuses, User user)
        {
            User = user;
            ReschedulingStatuses = reschedulingStatuses;
            guestReschedulingRequests = new ObservableCollection<GuestReschedulingRequest>();
            processedReschedulingRequests = new ObservableCollection<ProcessedReschedulingRequest>();
            Update();
        }
        public void ReservationsTabClick()
        {
            Res Reservations = new Res(User, ReschedulingStatuses.GuestMainWindow);
            ReschedulingStatuses.GuestMainWindow.mainFrame.Navigate(Reservations);
        }
        public void ReschedulingTabClick()
        {
            ReschedulingStatuses reschedulingStatuses = new ReschedulingStatuses(User, ReschedulingStatuses.GuestMainWindow);
            ReschedulingStatuses.GuestMainWindow.mainFrame.Navigate(reschedulingStatuses);
        }
        public void HistoryTabClick()
        {
            ReservationHistory reservationHistory = new ReservationHistory(User, ReschedulingStatuses.GuestMainWindow);
            ReschedulingStatuses.GuestMainWindow.mainFrame.Navigate(reservationHistory);
        }
        private void SelectFirstCard1()
        {
            var container = ReschedulingStatuses.guestReschedulingRequestsItems.ItemContainerGenerator.ContainerFromIndex(0) as ContentPresenter;
            if (container != null)
            {
                var contentTemplate = container.ContentTemplate;
                var textBlock = contentTemplate.FindName("BorderBlock1", container) as Border;
                if (textBlock != null)
                {
                    Keyboard.Focus(textBlock); // Focus the TextBlock or other inner element
                }
            }
        }
        private void SelectFirstCard2()
        {
            var container = ReschedulingStatuses.processedReschedulingRequestsItems.ItemContainerGenerator.ContainerFromIndex(0) as ContentPresenter;
            if (container != null)
            {
                var contentTemplate = container.ContentTemplate;
                var textBlock = contentTemplate.FindName("BorderBlock2", container) as Border;
                if (textBlock != null)
                {
                    Keyboard.Focus(textBlock); // Focus the TextBlock or other inner element
                }
            }
        }
        public void FirstCardSelectClick1()
        {
            SelectFirstCard1();
        }
        public void FirstCardSelectClick2()
        {
            SelectFirstCard2();
        }
        public void Update()
        {
            guestReschedulingRequests.Clear();
            processedReschedulingRequests.Clear();
            List<GuestReschedulingRequest> guestReschedulingRequest = GuestReschedulingRequestService.GetInstance().GetAll().Where(t => t.GuestId == User.Id).ToList();
            List<ProcessedReschedulingRequest> processedReschedulingRequest = ProcessedReschedulingRequestService.GetInstance().GetAll().Where(t => t.GuestId == User.Id).ToList();
            foreach (GuestReschedulingRequest guest in guestReschedulingRequest)
                    guestReschedulingRequests.Add(guest);

            foreach (ProcessedReschedulingRequest processed in processedReschedulingRequest)
                    processedReschedulingRequests.Add(processed);
        }
        public void ClickedOnCard(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as ProcessedReschedulingRequest;
            OwnerCommentForRescheduling ownerCommentForRescheduling = new OwnerCommentForRescheduling(selectedCard, User);
            ownerCommentForRescheduling.Show();
            ownerCommentForRescheduling.Focus();
        }
    }
}
