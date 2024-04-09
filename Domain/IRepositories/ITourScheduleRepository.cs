using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourScheduleRepository
    {
        List<TourSchedule> GetAll();
        TourSchedule? GetById(int Id);
        int NextId();
        void Add(TourSchedule newTourSchedule);
        TourSchedule? Update(TourSchedule TourSchedule);
    }
}
