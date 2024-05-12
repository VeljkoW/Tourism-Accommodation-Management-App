using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourSuggestionNotificationService
    {
        public ITourSuggestionNotificationRepository TourSuggestionNotificationRepository { get; set; }
        public TourSuggestionNotificationService(ITourSuggestionNotificationRepository tourSuggestionNotificationRepository)
        {
            TourSuggestionNotificationRepository = tourSuggestionNotificationRepository;
        }
        public static TourSuggestionNotificationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourSuggestionNotificationService>();
        }
        public void Add(TourSuggestionNotification newTourSuggestionNotification)
        {
            TourSuggestionNotificationRepository.Add(newTourSuggestionNotification);
        }

        public List<TourSuggestionNotification> GetAll()
        {
            return TourSuggestionNotificationRepository.GetAll();
        }

        public TourSuggestionNotification? GetById(int Id)
        {
            return TourSuggestionNotificationRepository.GetById(Id);
        }
        public TourSuggestionNotification? Update(TourSuggestionNotification tourSuggestionNotification)
        {
            return TourSuggestionNotificationRepository.Update(tourSuggestionNotification);
        }
        public List<TourSuggestionNotification> GetAllUnread(int userId)
        {
            List<TourSuggestionNotification> unreadNotifications = new List<TourSuggestionNotification>();
            foreach(TourSuggestionNotification notification in GetAll())
            {
                TourSuggestion ?tourSuggestion = TourSuggestionService.GetInstance().GetById(notification.TourSuggestionId);
                int tourSuggestionUserId = -1;
                if (tourSuggestion != null)
                {
                    tourSuggestionUserId = tourSuggestion.UserId;

                    if (notification.NotificationStatus == NotificationStatus.Unread && userId == tourSuggestionUserId)
                    {
                        notification.TourDate = tourSuggestion.Date;
                        Location ?location = LocationService.GetInstance().GetById(tourSuggestion.LocationId);
                        if (location != null)
                        {
                            notification.CityName = location.City;
                        }
                        else
                        {
                            notification.CityName = "error";
                        }
                        unreadNotifications.Add(notification);
                    }
                }
            }
            return unreadNotifications;
        }
    }
}
