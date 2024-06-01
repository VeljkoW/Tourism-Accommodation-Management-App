using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.ViewModel.Guest
{
    public class GuestNotificationsViewModel : INotifyPropertyChanged
    {
        public User User { get; set; }
        public GuestNotifications GuestNotifications { get; set; }
        public RelayCommand Exit => new RelayCommand(execute => CloseWindow());
        public ObservableCollection<ProcessedReschedulingRequest> ProcessedReschedulingRequests { get; set; }
        public RelayCommand CardsSelect => new RelayCommand(execute => Cards());

        public GuestNotificationsViewModel(User user, GuestNotifications guestNotifications)
        {
            User = user;
            GuestNotifications = guestNotifications;
            ProcessedReschedulingRequests = new ObservableCollection<ProcessedReschedulingRequest>();
            foreach(ProcessedReschedulingRequest processedReschedulingRequest in ProcessedReschedulingRequestService.GetInstance().GetAll())
            {

                if (User.Id == processedReschedulingRequest.GuestId)
                {
                    ProcessedReschedulingRequests.Add(processedReschedulingRequest);
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void CloseWindow()
        {
            GuestNotifications.Close();
        }
        private void SelectFirstCard1()
        {
            var container = GuestNotifications.reviewsItems.ItemContainerGenerator.ContainerFromIndex(0) as ContentPresenter;
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
        public void Cards()
        {
            SelectFirstCard1();
        }
        public void Refresh(object sender, RoutedEventArgs e)
        {
            ProcessedReschedulingRequests = new ObservableCollection<ProcessedReschedulingRequest>();
            foreach (ProcessedReschedulingRequest processedReschedulingRequest in ProcessedReschedulingRequestService.GetInstance().GetAll())
            {
                if (User.Id == processedReschedulingRequest.GuestId)
                {
                    ProcessedReschedulingRequests.Add(processedReschedulingRequest);
                }
            }
        }
    }
}
