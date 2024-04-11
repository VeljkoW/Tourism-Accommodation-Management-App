using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class GuestReschedulingRequestService
    {
        private IGuestReschedulingRequestRepository guestReschedulingRequestRepository = GuestReschedulingRequestRepository.GetInstance();
        public GuestReschedulingRequestService() { }
        public static GuestReschedulingRequestService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<GuestReschedulingRequestService>();
        }
        public void Add(GuestReschedulingRequest newGuestReschedulingRequest)
        {
            guestReschedulingRequestRepository.Add(newGuestReschedulingRequest);
        }

        public List<GuestReschedulingRequest> GetAll()
        {
            return guestReschedulingRequestRepository.GetAll();
        }

        public GuestReschedulingRequest? GetById(int Id)
        {
            return guestReschedulingRequestRepository.GetById(Id);
        }
    }
}
