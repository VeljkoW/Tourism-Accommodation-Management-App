using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourReservationRepository
    {
        List<TourReservation> GetAll();
        TourReservation? GetById(int Id);
        int NextId();
        TourReservation Add(TourReservation newTourReservation);
        TourReservation? GetByScheduleId(int Id);
    }
}
