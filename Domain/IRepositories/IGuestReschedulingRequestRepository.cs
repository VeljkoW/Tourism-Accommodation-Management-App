using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IGuestReschedulingRequestRepository
    {
        List<GuestReschedulingRequest> GetAll();

        GuestReschedulingRequest? GetById(int Id);

        int NextId();

        void Add(GuestReschedulingRequest newGuestReschedulingRequest);
    }
}
