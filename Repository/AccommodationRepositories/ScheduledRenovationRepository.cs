using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using BookingApp.View.Guest.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class ScheduledRenovationRepository : IScheduledRenovationRepository
    {
        public static ScheduledRenovationRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ScheduledRenovationRepository>();
        }
        private const string FilePath = "../../../Resources/Data/scheduledRenovation.csv";

        private readonly Serializer<ScheduledRenovation> _serializer;

        private List<ScheduledRenovation> _scheduledRenovations;
        public ScheduledRenovationRepository()
        {
            _serializer = new Serializer<ScheduledRenovation>();
            _scheduledRenovations = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _scheduledRenovations = _serializer.FromCSV(FilePath);
            if (_scheduledRenovations.Count < 1)
            {
                return 1;
            }
            return _scheduledRenovations.Max(c => c.Id) + 1;
        }
        public void Add(ScheduledRenovation newScheduledRenovation)
        {
            newScheduledRenovation.Id = NextId();
            _scheduledRenovations.Add(newScheduledRenovation);
            _serializer.ToCSV(FilePath, _scheduledRenovations);
        }

        public List<ScheduledRenovation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ScheduledRenovation? GetById(int Id)
        {
            _scheduledRenovations = _serializer.FromCSV(FilePath);
            return _scheduledRenovations.Find(c => c.Id == Id);
        }
        public void Delete(ScheduledRenovation scheduledRenovation)
        {
            _scheduledRenovations = _serializer.FromCSV(FilePath);
            _scheduledRenovations.RemoveAll(c => c.Id == scheduledRenovation.Id);
            _serializer.ToCSV(FilePath, _scheduledRenovations);
        }
    }
}
