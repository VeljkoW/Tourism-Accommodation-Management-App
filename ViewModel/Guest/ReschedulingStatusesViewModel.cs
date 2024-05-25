using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guest
{
    public class ReschedulingStatusesViewModel
    {
        public User User { get; set; }

        public ReschedulingStatuses ReschedulingStatuses { get; set; }
        public ObservableCollection<GuestReschedulingRequest> guestReschedulingRequests { get; set; }

        public ObservableCollection<ProcessedReschedulingRequest> processedReschedulingRequests { get; set; }
        public ReschedulingStatusesViewModel(ReschedulingStatuses reschedulingStatuses, User user)
        {
            User = user;
            ReschedulingStatuses = reschedulingStatuses;
            guestReschedulingRequests = new ObservableCollection<GuestReschedulingRequest>();
            processedReschedulingRequests = new ObservableCollection<ProcessedReschedulingRequest>();
            Update();
        }

        public void Update()
        {
            guestReschedulingRequests.Clear();
            processedReschedulingRequests.Clear();
            foreach (GuestReschedulingRequest guestReschedulingRequest in GuestReschedulingRequestService.GetInstance().GetAll())
            {
                if (guestReschedulingRequest.GuestId == User.Id)
                    guestReschedulingRequests.Add(guestReschedulingRequest);
            }

            foreach (ProcessedReschedulingRequest processedReschedulingRequest in ProcessedReschedulingRequestService.GetInstance().GetAll())
            {
                if (processedReschedulingRequest.GuestId == User.Id)
                    processedReschedulingRequests.Add(processedReschedulingRequest);
            }
        }
    }
}
