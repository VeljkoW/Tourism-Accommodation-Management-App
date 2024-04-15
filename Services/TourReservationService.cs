using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
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
        public ITourReservationRepository TourReservationRepository { get; set; }
        public TourReservationService(ITourReservationRepository tourReservationRepository) {
            TourReservationRepository = tourReservationRepository;
        }
        public static TourReservationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourReservationService>();
        }
        public TourReservation Add(TourReservation newReservation)
        {
            return TourReservationRepository.Add(newReservation);
        }

        public List<TourReservation> GetAll()
        {
            return TourReservationRepository.GetAll();
        }

        public TourReservation? GetById(int Id)
        {
            return TourReservationRepository.GetById(Id);
        }
        public TourReservation? GetByScheduleId(int Id)
        {
            return TourReservationRepository.GetByScheduleId(Id);
        }

        public List<TourPerson>? GetTouristsByReservationId(int id)
        {
            TourReservation? t = GetById(id);
            return t?.People;
        }
        public List<TourReservation> GetReservationsByScheduleId(int id)
        {
            return GetAll().Where(t => t.TourScheduleId == id).ToList();
        }
    }
}
