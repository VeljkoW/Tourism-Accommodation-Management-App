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
    public class TourSuggestionNotificationRepository : ITourSuggestionNotificationRepository
    {
        public static TourSuggestionNotificationRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourSuggestionNotificationRepository>();
        }
        private const string FilePath = "../../../Resources/Data/toursuggestionnotifications.csv";

        private readonly Serializer<TourSuggestionNotification> _serializer;

        private List<TourSuggestionNotification> _tourSuggestionNotifications;
        public TourSuggestionNotificationRepository()
        {
            _serializer = new Serializer<TourSuggestionNotification>();
            _tourSuggestionNotifications = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourSuggestionNotifications = _serializer.FromCSV(FilePath);
            if (_tourSuggestionNotifications.Count < 1)
            {
                return 1;
            }
            return _tourSuggestionNotifications.Max(c => c.Id) + 1;
        }
        public void Add(TourSuggestionNotification newTourSuggestionNotification)
        {
            newTourSuggestionNotification.Id = NextId();
            _tourSuggestionNotifications.Add(newTourSuggestionNotification);
            _serializer.ToCSV(FilePath, _tourSuggestionNotifications);
        }
        public List<TourSuggestionNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourSuggestionNotification? Update(TourSuggestionNotification tourSuggestionNotification)
        {
            TourSuggestionNotification? oldTourSuggestionNotification = GetById(tourSuggestionNotification.Id);
            if (oldTourSuggestionNotification is null) return null;
            oldTourSuggestionNotification.TourSuggestionId = tourSuggestionNotification.TourSuggestionId;
            oldTourSuggestionNotification.NotificationDate = tourSuggestionNotification.NotificationDate;
            oldTourSuggestionNotification.NotificationStatus = tourSuggestionNotification.NotificationStatus;
            _serializer.ToCSV(FilePath, _tourSuggestionNotifications);
            return oldTourSuggestionNotification;
        }

        public TourSuggestionNotification? GetById(int Id)
        {
            _tourSuggestionNotifications = _serializer.FromCSV(FilePath);
            return _tourSuggestionNotifications.Find(c => c.Id == Id);
        }
    }
}
