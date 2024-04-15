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
        public IGuestReschedulingRequestRepository GuestReschedulingRequestRepository {get;set;}
        public GuestReschedulingRequestService(IGuestReschedulingRequestRepository GuestReschedulingRequestRepository) { }
        public static GuestReschedulingRequestService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<GuestReschedulingRequestService>();
        }
        public void Add(GuestReschedulingRequest newGuestReschedulingRequest)
        {
            GuestReschedulingRequestRepository.Add(newGuestReschedulingRequest);
        }

        public List<GuestReschedulingRequest> GetAll()
        {
            return GuestReschedulingRequestRepository.GetAll();
        }

        public GuestReschedulingRequest? GetById(int Id)
        {
            return GuestReschedulingRequestRepository.GetById(Id);
        }
        public void DeleteById(int Id)
        {
            GuestReschedulingRequestRepository.DeleteById(Id);
        }
    }
}
