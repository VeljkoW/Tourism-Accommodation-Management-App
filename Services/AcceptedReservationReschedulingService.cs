using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class AcceptedReservationReschedulingService
    {
        public IAcceptedReservationReschedulingRepository AcceptedReservationReschedulingRepository { get; set; }
        public AcceptedReservationReschedulingService(IAcceptedReservationReschedulingRepository acceptedReservationReschedulingRepository) { AcceptedReservationReschedulingRepository = acceptedReservationReschedulingRepository; }
        public static AcceptedReservationReschedulingService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<AcceptedReservationReschedulingService>();
        }
        public void Add(AcceptedReservationRescheduling acceptedReservationRescheduling)
        {
            AcceptedReservationReschedulingRepository.Add(acceptedReservationRescheduling);
        }

        public List<AcceptedReservationRescheduling> GetAll()
        {
            return AcceptedReservationReschedulingRepository.GetAll();
        }
    }
}
