using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourAttendenceNotificationRepository
    {
        List<TourAttendenceNotification> GetAll();
        TourAttendenceNotification? GetById(int Id);
        int NextId();
        TourAttendenceNotification Add(TourAttendenceNotification newTourAttendenceNotification);
        TourAttendenceNotification? Update(TourAttendenceNotification tourAttendenceNotification);
    }
}
