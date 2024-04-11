using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Guest.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class GuestReschedulingRequestRepository : IGuestReschedulingRequestRepository
    {
        public static GuestReschedulingRequestRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<GuestReschedulingRequestRepository>();
        }

        private const string FilePath = "../../../Resources/Data/guestReschedulingRequest.csv";

        private readonly Serializer<GuestReschedulingRequest> _serializer;

        private List<GuestReschedulingRequest> _guestReschedulingRequest;

        public GuestReschedulingRequestRepository()
        {
            _serializer = new Serializer<GuestReschedulingRequest>();
            _guestReschedulingRequest = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _guestReschedulingRequest = _serializer.FromCSV(FilePath);
            if (_guestReschedulingRequest.Count < 1)
            {
                return 1;
            }
            return _guestReschedulingRequest.Max(c => c.Id) + 1;
        }

        public List<GuestReschedulingRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public GuestReschedulingRequest? GetById(int Id)
        {
            _guestReschedulingRequest = _serializer.FromCSV(FilePath);
            return _guestReschedulingRequest.Find(c => c.Id == Id);
        }

        public void Add(GuestReschedulingRequest newGuestReschedulingRequest)
        {
            newGuestReschedulingRequest.Id = NextId();
            _guestReschedulingRequest.Add(newGuestReschedulingRequest);
            _serializer.ToCSV(FilePath, _guestReschedulingRequest);
        }
    }
}
