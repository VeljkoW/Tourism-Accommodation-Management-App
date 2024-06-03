using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Windows;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guest
{
    public class GuestRenovationViewModel
    {
        public INotificationManager notificationManager = App.GetNotificationManager();
        public GuestRenovation GuestRenovation { get; set; }
        public ReservedAccommodation ReservedAccommodation { get; set; }
        public RelayCommand AddCommentTextBox => new RelayCommand(execute => AddComment());
        public RelayCommand Level => new RelayCommand(execute => LevelChecked());
        public RelayCommand GoRight => new RelayCommand(execute => Right());
        public RelayCommand GoLeft => new RelayCommand(execute => Left());
        public RelayCommand Exit => new RelayCommand(execute => CloseWindow());
        public RelayCommand SendButton => new RelayCommand(execute => SendRenovation(), canExecute => CanSendRenovation());
        public GuestRenovationViewModel(GuestRenovation guestRenovation, ReservedAccommodation reservedAccommodation)
        {
            GuestRenovation = guestRenovation;
            ReservedAccommodation = reservedAccommodation;
            GuestRenovation.AccommodationInformation.Content += ReservedAccommodation.Accommodation.Name + ", " + ReservedAccommodation.Accommodation.Location.State + " - " + ReservedAccommodation.Accommodation.Location.City;
        }

        public void SendRenovation()
        {
            RenovationRequest renovationRequest = new RenovationRequest();
            renovationRequest.AccommodationId = ReservedAccommodation.Accommodation.Id;
            renovationRequest.GuestId = ReservedAccommodation.GuestId;
            
            Comment comment = new Comment();
            comment.Text = GuestRenovation.CommentTextBox.Text;
            comment.CreationTime = DateTime.Now;
            comment.User = UserService.GetInstance().GetById(ReservedAccommodation.GuestId);
            comment = CommentService.GetInstance().Save(comment);

            renovationRequest.CommentId = comment.Id;
            renovationRequest.Level = GuestRenovation.Level;
            renovationRequest.RequestDate = DateTime.Now;

            RenovationRequestService.GetInstance().Add(renovationRequest);

            GuestRenovation.Close();

            GuestRenovation.GuestRate.RenovationButton.IsEnabled = false;
            GuestRenovation.GuestRate.RenovationButton.Content = "Renovation sent";
            notificationManager.Show("Success", "Renovation Request Successfully sent!", NotificationType.Success);

        }

        public bool CanSendRenovation()
        { 
            if(string.IsNullOrEmpty(GuestRenovation.CommentTextBox.Text) || !IsLevelChecked())
                return false;
            return true;
        }

        public bool IsLevelChecked()
        {
            if (GuestRenovation.Level1.IsChecked == false && GuestRenovation.Level2.IsChecked == false &&
                GuestRenovation.Level3.IsChecked == false && GuestRenovation.Level4.IsChecked == false &&
                GuestRenovation.Level5.IsChecked == false)
                return false;
            
            return true;
        }

        public void CloseWindow()
        {
            GuestRenovation.Close();
        }
        public void Left()
        {
            if (GuestRenovation.Level1.IsFocused == true || GuestRenovation.Level2.IsFocused == true || GuestRenovation.Level3.IsFocused == true
                || GuestRenovation.Level4.IsFocused == true || GuestRenovation.Level5.IsFocused == true)
            {
                if (GuestRenovation.Level1.IsChecked == true)
                {
                    GuestRenovation.Level1.IsChecked = false;
                    GuestRenovation.Level5.IsChecked = true;
                    GuestRenovation.Level5.Focus();
                }
                else if (GuestRenovation.Level2.IsChecked == true)
                {
                    GuestRenovation.Level2.IsChecked = false;
                    GuestRenovation.Level1.IsChecked = true;
                    GuestRenovation.Level1.Focus();
                }
                else if (GuestRenovation.Level3.IsChecked == true)
                {
                    GuestRenovation.Level3.IsChecked = false;
                    GuestRenovation.Level2.IsChecked = true;
                    GuestRenovation.Level2.Focus();
                }
                else if (GuestRenovation.Level4.IsChecked == true)
                {
                    GuestRenovation.Level4.IsChecked = false;
                    GuestRenovation.Level3.IsChecked = true;
                    GuestRenovation.Level3.Focus();
                }
                else if (GuestRenovation.Level5.IsChecked == true)
                {
                    GuestRenovation.Level5.IsChecked = false;
                    GuestRenovation.Level4.IsChecked = true;
                    GuestRenovation.Level4.Focus();
                }
            }
           
        }
        public void Right()
        {
            if (GuestRenovation.Level1.IsFocused == true || GuestRenovation.Level2.IsFocused == true || GuestRenovation.Level3.IsFocused == true
                || GuestRenovation.Level4.IsFocused == true || GuestRenovation.Level5.IsFocused == true)
            {
                if (GuestRenovation.Level1.IsChecked == true)
                {
                    GuestRenovation.Level1.IsChecked = false;
                    GuestRenovation.Level2.IsChecked = true;
                    GuestRenovation.Level2.Focus();
                }
                else if (GuestRenovation.Level2.IsChecked == true)
                {
                    GuestRenovation.Level2.IsChecked = false;
                    GuestRenovation.Level3.IsChecked = true;
                    GuestRenovation.Level3.Focus();
                }
                else if (GuestRenovation.Level3.IsChecked == true)
                {
                    GuestRenovation.Level3.IsChecked = false;
                    GuestRenovation.Level4.IsChecked = true;
                    GuestRenovation.Level4.Focus();
                }
                else if (GuestRenovation.Level4.IsChecked == true)
                {
                    GuestRenovation.Level4.IsChecked = false;
                    GuestRenovation.Level5.IsChecked = true;
                    GuestRenovation.Level5.Focus();
                }
                else if (GuestRenovation.Level5.IsChecked == true)
                {
                    GuestRenovation.Level5.IsChecked = false;
                    GuestRenovation.Level1.IsChecked = true;
                    GuestRenovation.Level1.Focus();
                }
            }
        }
        public void LevelChecked()
        {
            GuestRenovation.Level1.IsChecked = true;
            GuestRenovation.Level1.Focus();
        }

        public void AddComment()
        {
            GuestRenovation.CommentTextBox.Focus();
        }
    }
}
