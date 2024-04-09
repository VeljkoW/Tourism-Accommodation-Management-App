using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourReservationService
    {
        private TourReservationRepository tourReservationRepository = TourReservationRepository.GetInstance();
        public TourReservationService() { }
        public static TourReservationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourReservationService>();
        }
        public TourReservation Add(TourReservation newReservation)
        {
            return tourReservationRepository.Add(newReservation);
        }

        public List<TourReservation> GetAll()
        {
            return tourReservationRepository.GetAll();
        }

        public TourReservation? GetById(int Id)
        {
            return tourReservationRepository.GetById(Id);
        }
        public TourReservation? GetByScheduleId(int Id)
        {
            return tourReservationRepository.GetByScheduleId(Id);
        }
    }
}
