using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class ReservationReschedulingViewModel
    {
        public RelayCommand AcceptClick => new RelayCommand(execute => AcceptExecute(), canExecute => CanExecute());
        public RelayCommand DeclineClick => new RelayCommand(execute=> DeclineExecute(), canExecute => CanExecute());
        public ObservableCollection<GuestReschedulingRequest> GuestReschedulingRequests { get; set; }
        public GuestReschedulingRequest SelectedGuestReschedulingRequest {  get; set; }
        public User user { get; set; }
        public ReservationRescheduling ReservationRescheduling {  get; set; }
        public ReservationReschedulingViewModel(ReservationRescheduling ReservationRescheduling, User user)
        {
            this.user = user;
            this.ReservationRescheduling = ReservationRescheduling;
            GuestReschedulingRequests = new ObservableCollection<GuestReschedulingRequest>();
            Update();
        }
        public void Update()
        {
            GuestReschedulingRequests.Clear();
            UpdateGuestReschedulingRequests();
            GuestReschedulingRequestAvailability();
        }
        public void UpdateGuestReschedulingRequests()
        {
            foreach (GuestReschedulingRequest guestReschedulingRequest in GuestReschedulingRequestService.GetInstance().GetAll())
            {
                foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
                {
                    if (accommodation.Id == guestReschedulingRequest.accommodationId && accommodation.OwnerId == user.Id)
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
            for (DateTime date = GuestReschedulingRequest.checkInDate; date <= GuestReschedulingRequest.checkOutDate; date = date.AddDays(1))
            {
                foreach (ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().GetAll())
                {
                    if (GuestReschedulingRequest.accommodationId == reservedAccommodation.accommodationId &&
                        reservedAccommodation.Id != GuestReschedulingRequest.ReservedAccommodationId &&
                        date > reservedAccommodation.checkInDate &&
                        date < reservedAccommodation.checkOutDate)
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
            processedReschedulingRequest.checkInDate = SelectedGuestReschedulingRequest.CheckInDate;
            processedReschedulingRequest.checkOutDate = SelectedGuestReschedulingRequest.CheckOutDate;
            ProcessedReschedulingRequestService.GetInstance().Add(processedReschedulingRequest);
            GuestReschedulingRequestService.GetInstance().DeleteById(SelectedGuestReschedulingRequest.Id);
            if (accepted)
                ReservedAccommodationService.GetInstance().UpdateDatesByReschedulingRequest(SelectedGuestReschedulingRequest);
            Update();
        }
    }
}
