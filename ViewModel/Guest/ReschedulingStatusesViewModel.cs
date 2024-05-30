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
            List<GuestReschedulingRequest> guestReschedulingRequest = GuestReschedulingRequestService.GetInstance().GetAll().Where(t => t.GuestId == User.Id).ToList();
            List<ProcessedReschedulingRequest> processedReschedulingRequest = ProcessedReschedulingRequestService.GetInstance().GetAll().Where(t => t.GuestId == User.Id).ToList();
            foreach (GuestReschedulingRequest guest in guestReschedulingRequest)
                    guestReschedulingRequests.Add(guest);

            foreach (ProcessedReschedulingRequest processed in processedReschedulingRequest)
                    processedReschedulingRequests.Add(processed);
        }
    }
}
