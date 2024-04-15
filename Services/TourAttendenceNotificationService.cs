using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourAttendenceNotificationService
    {
        public ITourAttendenceNotificationRepository TourAttendenceNotificationRepository{get;set;}
        public TourAttendenceNotificationService(ITourAttendenceNotificationRepository tourAttendenceNotificationRepository) { TourAttendenceNotificationRepository = tourAttendenceNotificationRepository; }
        public static TourAttendenceNotificationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourAttendenceNotificationService>();
        }
        public TourAttendenceNotification Add(TourAttendenceNotification newTourAttendenceNotification)
        {
            return TourAttendenceNotificationRepository.Add(newTourAttendenceNotification);
        }

        public List<TourAttendenceNotification> GetAll()
        {
            return TourAttendenceNotificationRepository.GetAll();
        }

        public TourAttendenceNotification? GetById(int Id)
        {
            return TourAttendenceNotificationRepository.GetById(Id);
        }
        public TourAttendenceNotification? Update(TourAttendenceNotification tourAttendenceNotification)
        {
            return TourAttendenceNotificationRepository.Update(tourAttendenceNotification);
        }
    }
}
