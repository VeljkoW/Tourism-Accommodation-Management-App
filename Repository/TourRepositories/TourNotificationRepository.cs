using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourNotificationRepository : ITourNotificationRepository
    {
        public static TourNotificationRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourNotificationRepository>();
        }
        private const string FilePath = "../../../Resources/Data/tournotifications.csv";

        private readonly Serializer<TourNotification> _serializer;

        private List<TourNotification> _tourNotifications;
        public TourNotificationRepository()
        {
            _serializer = new Serializer<TourNotification>();
            _tourNotifications = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourNotifications = _serializer.FromCSV(FilePath);
            if (_tourNotifications.Count < 1)
            {
                return 1;
            }
            return _tourNotifications.Max(c => c.Id) + 1;
        }
        public void Add(TourNotification newTourNotification)
        {
            newTourNotification.Id = NextId();
            _tourNotifications.Add(newTourNotification);
            _serializer.ToCSV(FilePath, _tourNotifications);
        }
        public List<TourNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourNotification? Update(TourNotification tourNotification)
        {
            TourNotification? oldTourNotification = GetById(tourNotification.Id);
            if (oldTourNotification is null) return null;
            oldTourNotification.UserId = tourNotification.UserId;
            oldTourNotification.TourId = tourNotification.TourId;
            oldTourNotification.NotificationDate = tourNotification.NotificationDate;
            oldTourNotification.Status = tourNotification.Status;
            oldTourNotification.TourName = tourNotification.TourName;
            oldTourNotification.Location = tourNotification.Location;
            oldTourNotification.Language = tourNotification.Language;
            _serializer.ToCSV(FilePath, _tourNotifications);
            return oldTourNotification;
        }

        public TourNotification? GetById(int Id)
        {
            _tourNotifications = _serializer.FromCSV(FilePath);
            return _tourNotifications.Find(c => c.Id == Id);
        }
    }
}
