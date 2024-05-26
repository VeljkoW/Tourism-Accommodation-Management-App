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
    public class OwnerNotificationService
    {
        public IOwnerNotificationRepository OwnerNotificationRepository { get; set; }
        public OwnerNotificationService(IOwnerNotificationRepository ownerNotificationRepository) { OwnerNotificationRepository = ownerNotificationRepository; }
        public static OwnerNotificationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<OwnerNotificationService>();
        }
        public void Add(OwnerNotification OwnerNotification)
        {
            OwnerNotificationRepository.Add(OwnerNotification);
        }
        public List<OwnerNotification> GetAll()
        {
            return OwnerNotificationRepository.GetAll();
        }
        public OwnerNotification? GetById(int Id)
        {
            return OwnerNotificationRepository.GetById(Id);
        }
    }
}
