using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IOwnerNotificationRepository
    {
        List<OwnerNotification> GetAll();
        OwnerNotification? GetById(int Id);
        int NextId();
        void Add(OwnerNotification OwnerNotification);
    }
}
