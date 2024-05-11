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
    public class TourNotificationService
    {
        public ITourNotificationRepository TourNotificationRepository { get; set; }
        public TourNotificationService(ITourNotificationRepository tourNotificationRepository)
        {
            TourNotificationRepository = tourNotificationRepository;
        }
        public static TourNotificationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourNotificationService>();
        }
        public void Add(TourNotification newTourNotification)
        {
            TourNotificationRepository.Add(newTourNotification);
        }

        public List<TourNotification> GetAll()
        {
            return TourNotificationRepository.GetAll();
        }

        public TourNotification? GetById(int Id)
        {
            return TourNotificationRepository.GetById(Id);
        }
        public TourNotification? Update(TourNotification tourNotification)
        {
            return TourNotificationRepository.Update(tourNotification);
        }
        public List<TourNotification> GetAllUnread(int userId)
        {
            List<TourNotification> unreadNotifications = new List<TourNotification>();
            foreach (TourNotification notification in GetAll())
            {

                if (notification.Status == NotificationStatus.Unread && userId == notification.UserId)
                {
                    unreadNotifications.Add(notification);
                }
            }
            return unreadNotifications;
        }
        public List<TourNotificationTemp> CheckTourSuggestions(Location location,string Language)
        {
            List<TourNotificationTemp> userIds = new List<TourNotificationTemp>();
            foreach(TourSuggestion suggestion in TourSuggestionService.GetInstance().GetAll())
            {
                if(suggestion.Status != TourSuggestionStatus.Accepted)
                {
                    Location ?suggestionLocation = LocationService.GetInstance().GetById(suggestion.LocationId);
                    if(suggestionLocation != null)
                    {
                        if(location == suggestionLocation)
                        {
                            bool exists = false;
                            foreach (TourNotificationTemp tmp in userIds) 
                            { 
                                if(tmp.UserId == suggestion.UserId) 
                                {
                                    exists= true;
                                    break;
                                }
                            }
                            if(!exists) 
                            {
                                TourNotificationTemp tourNotification = new TourNotificationTemp(suggestion.UserId, "Location");
                                userIds.Add(tourNotification);
                            }
                        }
                    }
                    if(Language == suggestion.Language)
                    {
                        bool exists = false;
                        foreach (TourNotificationTemp tmp in userIds)
                        {
                            if (tmp.UserId == suggestion.UserId)
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (!exists)
                        {
                            TourNotificationTemp tourNotification = new TourNotificationTemp(suggestion.UserId, "Location");
                            userIds.Add(tourNotification);
                        }
                    }
                }
            }
            return userIds;
        }
        public void SendNotifications(Tour tour)
        {
            
            foreach(TourNotificationTemp tmp in CheckTourSuggestions(tour.Location,tour.Language)) 
            {
                Add(new TourNotification(tmp.UserId,tour.Id,DateTime.Now, tmp.Reason));
            }
        }
        public List<TourNotification> GetAllLanguage(int userId)
        {
            List<TourNotification> tourNotifications = new List<TourNotification>();
            foreach(TourNotification tourNotification in GetAllUnread(userId))
            {
                if(tourNotification.Reason == "Language")
                {
                    Tour ?tour = TourService.GetInstance().GetById(tourNotification.TourId);
                    if (tour != null)
                    {
                        tourNotification.Language = tour.Language;
                        tourNotification.TourName = tour.Name;
                        tourNotifications.Add(tourNotification);
                    }
                }
            }
            return tourNotifications;
        }

        public List<TourNotification> GetAllLocation(int userId)
        {
            List<TourNotification> tourNotifications = new List<TourNotification>();
            foreach (TourNotification tourNotification in GetAllUnread(userId))
            {
                if (tourNotification.Reason == "Location")
                {
                    Tour? tour = TourService.GetInstance().GetById(tourNotification.TourId);
                    if (tour != null)
                    {
                        Location? location = LocationService.GetInstance().GetById(tour.LocationId);
                        if (location != null)
                        {
                            tourNotification.Location = location.State + ", " + location.City;
                            tourNotification.TourName = tour.Name;
                            tourNotifications.Add(tourNotification);
                        }
                    }
                }
            }
            return tourNotifications;
        }
    }
}
