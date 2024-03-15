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

        public TourSchedule? Update(TourSchedule tourSchedule)
        {
            TourSchedule? oldTourSchedule = GetById(tourSchedule.Id);
            if (oldTourSchedule is null) return null;
            oldTourSchedule.Date = tourSchedule.Date;
            oldTourSchedule.TourId = tourSchedule.TourId;
            oldTourSchedule.Guests = tourSchedule.Guests;
            oldTourSchedule.ScheduleStatus = tourSchedule.ScheduleStatus;
            oldTourSchedule.VisitedKeypoints = tourSchedule.VisitedKeypoints;
            _serializer.ToCSV(FilePath, _tourSchedules);
            return oldTourSchedule;
        }

        public TourSchedule? GetById(int Id)
        {
            _tourSchedules = _serializer.FromCSV(FilePath);
            return _tourSchedules.Find(c => c.Id == Id);
        }
    }
}
