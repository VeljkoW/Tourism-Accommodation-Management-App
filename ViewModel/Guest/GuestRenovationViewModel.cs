using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Windows;
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
        public GuestRenovation GuestRenovation { get; set; }
        public ReservedAccommodation ReservedAccommodation { get; set; }

        public RelayCommand SendButton => new RelayCommand(execute => SendRenovation(), canExecute => CanSendRenovation());
        public GuestRenovationViewModel(GuestRenovation guestRenovation, ReservedAccommodation reservedAccommodation)
        {
            GuestRenovation = guestRenovation;
            ReservedAccommodation = reservedAccommodation;
            GuestRenovation.AccommodationInformation.Content += ReservedAccommodation.AccommodationName + ", " + ReservedAccommodation.Location.State + " - " + ReservedAccommodation.Location.City;
        }

        public void SendRenovation()
        {
            RenovationRequest renovationRequest = new RenovationRequest();
            renovationRequest.AccommodationId = ReservedAccommodation.AccommodationId;
            renovationRequest.GuestId = ReservedAccommodation.GuestId;
            
            Comment comment = new Comment();
            comment.Text = GuestRenovation.CommentTextBox.Text;
            comment.CreationTime = DateTime.Now;
            comment.User = UserService.GetInstance().GetById(ReservedAccommodation.GuestId);
            comment = CommentService.GetInstance().Save(comment);

            renovationRequest.CommentId = comment.Id;
            renovationRequest.Level = GuestRenovation.Level;

            RenovationRequestService.GetInstance().Add(renovationRequest);

            GuestRenovation.Close();

            GuestRenovation.GuestRate.RenovationButton.IsEnabled = false;
            GuestRenovation.GuestRate.RenovationButton.Content = "Renovation sent";

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
    }
}
