using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class OwnerNotificationRepository : IOwnerNotificationRepository
    {
        public static OwnerNotificationRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<OwnerNotificationRepository>();
        }
        private const string FilePath = "../../../Resources/Data/ownerNotification.csv";

        private readonly Serializer<OwnerNotification> _serializer;

        private List<OwnerNotification> _ownerNotifications;
        public OwnerNotificationRepository()
        {
            _serializer = new Serializer<OwnerNotification>();
            _ownerNotifications = _serializer.FromCSV(FilePath);
        }
        public void Add(OwnerNotification OwnerNotification)
        {
            OwnerNotification.Id = NextId();
            _ownerNotifications.Add(OwnerNotification);
            _serializer.ToCSV(FilePath, _ownerNotifications);
        }
        public List<OwnerNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public OwnerNotification? GetById(int Id)
        {
            _ownerNotifications = _serializer.FromCSV(FilePath);
            return _ownerNotifications.Find(c => c.Id == Id);
        }
        public int NextId()
        {
            _ownerNotifications = _serializer.FromCSV(FilePath);
            if (_ownerNotifications.Count < 1)
            {
                return 1;
            }
            return _ownerNotifications.Max(c => c.Id) + 1;
        }
    }
}
