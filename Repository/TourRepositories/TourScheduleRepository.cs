using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourScheduleRepository
    {
        private const string FilePath = "../../../Resources/Data/tourschedules.csv";

        private readonly Serializer<TourSchedule> _serializer;

        private List<TourSchedule> _tourSchedules;
        public TourScheduleRepository()
        {
            _serializer = new Serializer<TourSchedule>();
            _tourSchedules = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourSchedules = _serializer.FromCSV(FilePath);
            if (_tourSchedules.Count < 1)
            {
                return 1;
            }
            return _tourSchedules.Max(c => c.Id) + 1;
        }
        internal void Add(TourSchedule newSchedule)
        {
            newSchedule.Id = NextId();
            _tourSchedules.Add(newSchedule);
            _serializer.ToCSV(FilePath, _tourSchedules);
        }
        public List<TourSchedule> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
    }
}
