using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guest
{

    public class GuestRescheduleStatusViewModel
    {
        public User user {  get; set; }

        public RescheduleStatus rescheduleStatus { get; set; }

        public ObservableCollection<GuestReschedulingRequest> guestReschedulingRequests { get; set; }

        public ObservableCollection<ProcessedReschedulingRequest> processedReschedulingRequests { get; set; }
        public GuestRescheduleStatusViewModel(RescheduleStatus rescheduleStatus, User user)
        {
            this.user = user;
            this.rescheduleStatus = rescheduleStatus;
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
                if(guestReschedulingRequest.GuestId == user.Id)
                {
                    guestReschedulingRequests.Add(guestReschedulingRequest);
                }
            }

            foreach (ProcessedReschedulingRequest processedReschedulingRequest in ProcessedReschedulingRequestService.GetInstance().GetAll())
            {
                if (processedReschedulingRequest.GuestId == user.Id)
                {
                     processedReschedulingRequests.Add(processedReschedulingRequest);
                }
            }
        }
    }
}
