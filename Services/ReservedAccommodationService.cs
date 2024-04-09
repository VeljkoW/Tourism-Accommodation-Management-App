using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class ReservedAccommodationService
    {
        private ReservedAccommodationRepository reservedAccommodationRepository = ReservedAccommodationRepository.GetInstance();
        public ReservedAccommodationService() { }
        public ReservedAccommodationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReservedAccommodationService>();
        }
        public void Add(ReservedAccommodation newReservedAccommodation)
        {
            reservedAccommodationRepository.Add(newReservedAccommodation);
        }

        public ReservedAccommodation? GetById(int Id)
        {
            return reservedAccommodationRepository.GetById(Id);
        }
        public List<ReservedAccommodation> GetAll()
        {
            return reservedAccommodationRepository.GetAll();
        }
    }
}
