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
    public class RenovationRequestRepository : IRenovationRequestRepository
    {
        public static RenovationRequestRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<RenovationRequestRepository>();
        }
        private const string FilePath = "../../../Resources/Data/renovationRequest.csv";

        private readonly Serializer<RenovationRequest> _serializer;

        private List<RenovationRequest> _renovationRequests;
        public RenovationRequestRepository()
        {
            _serializer = new Serializer<RenovationRequest>();
            _renovationRequests = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _renovationRequests = _serializer.FromCSV(FilePath);
            if (_renovationRequests.Count < 1)
            {
                return 1;
            }
            return _renovationRequests.Max(c => c.Id) + 1;
        }
        public void Add(RenovationRequest renovationRequest)
        {
            renovationRequest.Id = NextId();
            _renovationRequests.Add(renovationRequest);
            _serializer.ToCSV(FilePath, _renovationRequests);
        }

        public List<RenovationRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public RenovationRequest? GetById(int Id)
        {
            _renovationRequests = _serializer.FromCSV(FilePath);
            return _renovationRequests.Find(c => c.Id == Id);
        }

    }
}
