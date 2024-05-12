using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IScheduledRenovationRepository
    {
        List<ScheduledRenovation> GetAll();
        ScheduledRenovation? GetById(int Id);
        int NextId();
        void Add(ScheduledRenovation newScheduledRenovation);
        void Delete(ScheduledRenovation ScheduledRenovation);
    }
}
