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
    public class TourAttendenceNotificationRepository : ITourAttendenceNotificationRepository
    {
        public static TourAttendenceNotificationRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourAttendenceNotificationRepository>();
        }
        private const string FilePath = "../../../Resources/Data/tourattendencenotifications.csv";

        private readonly Serializer<TourAttendenceNotification> _serializer;

        private List<TourAttendenceNotification> _tourAttendenceNotifications;

        public TourAttendenceNotificationRepository()
        {
            _serializer = new Serializer<TourAttendenceNotification>();
            _tourAttendenceNotifications = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourAttendenceNotifications = _serializer.FromCSV(FilePath);
            if (_tourAttendenceNotifications.Count < 1)
            {
                return 1;
            }
            return _tourAttendenceNotifications.Max(c => c.Id) + 1;
        }
        public TourAttendenceNotification Add(TourAttendenceNotification newTourAttendenceNotification)
        {
            newTourAttendenceNotification.Id = NextId();
            _tourAttendenceNotifications.Add(newTourAttendenceNotification);
            _serializer.ToCSV(FilePath, _tourAttendenceNotifications);
            return newTourAttendenceNotification;
        }
        public List<TourAttendenceNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public TourAttendenceNotification? GetById(int Id)
        {
            _tourAttendenceNotifications = _serializer.FromCSV(FilePath);
            return _tourAttendenceNotifications.Find(c => c.Id == Id);
        }

        public TourAttendenceNotification? Update(TourAttendenceNotification tourAttendenceNotification)
        {
            TourAttendenceNotification? oldTourAttendenceNotification = GetById(tourAttendenceNotification.Id);
            if (oldTourAttendenceNotification is null) return null;
            oldTourAttendenceNotification.UserId = tourAttendenceNotification.UserId;
            oldTourAttendenceNotification.TourPersonId = tourAttendenceNotification.TourPersonId;
            oldTourAttendenceNotification.Date = tourAttendenceNotification.Date;
            oldTourAttendenceNotification.ConfirmedAttendence = tourAttendenceNotification.ConfirmedAttendence;
            _serializer.ToCSV(FilePath, _tourAttendenceNotifications);
            return oldTourAttendenceNotification;
        }
    }
}
