using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BookingApp.Services
{
    public class ReservedAccommodationService
    {
        private IReservedAccommodationRepository ReservedAccommodationRepository { get; set; }
        public ReservedAccommodationService(IReservedAccommodationRepository reservedAccommodationRepository) {
            ReservedAccommodationRepository = reservedAccommodationRepository;
        }
        public static ReservedAccommodationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReservedAccommodationService>();
        }
        public void Add(ReservedAccommodation newReservedAccommodation)
        {
            ReservedAccommodationRepository.Add(newReservedAccommodation);
        }

        public ReservedAccommodation? GetByAccommodationId(int Id)
        {
            return ReservedAccommodationRepository.GetByAccommodationId(Id);
        }

        public ReservedAccommodation? GetById(int Id)
        {
            return ReservedAccommodationRepository.GetById(Id);
        }
        public List<ReservedAccommodation> GetAll()
        {
            return ReservedAccommodationRepository.GetAll();
        }

        public void Delete(ReservedAccommodation reservedAccommodation)
        {
            ReservedAccommodationRepository.Delete(reservedAccommodation);
        }
        public void UpdateDatesByReschedulingRequest(GuestReschedulingRequest GuestReschedulingRequest)
        {
            ReservedAccommodationRepository.UpdateDatesByReschedulingRequest(GuestReschedulingRequest);
        }

        public ObservableCollection<ReservedAccommodation> Update(User user)
        {
            ObservableCollection<ReservedAccommodation> reservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            foreach (ReservedAccommodation reserved in GetAll())
            {
                if (user.Id == reserved.GuestId) reservedAccommodations.Add(reserved);
            }
            return reservedAccommodations;
        }
    }
}
