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
            for (DateTime date = GuestReschedulingRequest.CheckInDate; date <= GuestReschedulingRequest.CheckOutDate; date = date.AddDays(1))
            {
                foreach (ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().GetAll())
                {
                    if (GuestReschedulingRequest.AccommodationId == reservedAccommodation.Accommodation.Id &&
                        reservedAccommodation.Id != GuestReschedulingRequest.ReservedAccommodationId &&
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
                ReservedAccommodationService.GetInstance().UpdateDatesByReschedulingRequest(SelectedGuestReschedulingRequest);
            Update();
        }
    }
}
