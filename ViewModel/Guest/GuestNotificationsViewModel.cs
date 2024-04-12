using BookingApp.Domain.Model;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guest
{
    public class GuestNotificationsViewModel
    {
        public User User { get; set; }

        public ObservableCollection<ProcessedReschedulingRequest> ProcessedReschedulingRequests { get; set; }
        public GuestNotificationsViewModel(User user)
        {
            User = user;
            ProcessedReschedulingRequests = new ObservableCollection<ProcessedReschedulingRequest>();
            foreach(ProcessedReschedulingRequest processedReschedulingRequest in ProcessedReschedulingRequestService.GetInstance().GetAll())
            {

                if (User.Id == processedReschedulingRequest.GuestId)
                {
                    ProcessedReschedulingRequests.Add(processedReschedulingRequest);
                }
            }
        }
    }
}
