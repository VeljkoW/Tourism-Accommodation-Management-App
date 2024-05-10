using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourNotificationRepository
    {
        List<TourNotification> GetAll();
        TourNotification? GetById(int Id);
        int NextId();
        void Add(TourNotification newTourNotification);
        TourNotification? Update(TourNotification tourNotification);
    }
}
