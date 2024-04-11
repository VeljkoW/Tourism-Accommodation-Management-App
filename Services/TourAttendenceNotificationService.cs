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
        private ITourAttendenceNotificationRepository tourAttendenceNotificationRepository = TourAttendenceNotificationRepository.GetInstance();
        public TourAttendenceNotificationService() { }
        public static TourAttendenceNotificationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourAttendenceNotificationService>();
        }
        public TourAttendenceNotification Add(TourAttendenceNotification newTourAttendenceNotification)
        {
            return tourAttendenceNotificationRepository.Add(newTourAttendenceNotification);
        }

        public List<TourAttendenceNotification> GetAll()
        {
            return tourAttendenceNotificationRepository.GetAll();
        }

        public TourAttendenceNotification? GetById(int Id)
        {
            return tourAttendenceNotificationRepository.GetById(Id);
        }
        public TourAttendenceNotification? Update(TourAttendenceNotification tourAttendenceNotification)
        {
            return tourAttendenceNotificationRepository.Update(tourAttendenceNotification);
        }
    }
}
