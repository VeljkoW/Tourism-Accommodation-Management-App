using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IReservedAccommodationRepository
    {
        List<ReservedAccommodation> GetAll();
        List<ReservedAccommodation>? GetByAccommodationId(int Id);
        ReservedAccommodation? GetById(int Id);
        void Add(ReservedAccommodation newReservedAccommodation);
        void Delete(ReservedAccommodation reservedAccommodation);
        void UpdateDatesByReschedulingRequest(GuestReschedulingRequest GuestReschedulingRequest);
    }
}
