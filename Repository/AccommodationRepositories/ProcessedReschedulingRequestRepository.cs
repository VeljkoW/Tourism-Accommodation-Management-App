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
    public class ProcessedReschedulingRequestRepository : IProcessedReschedulingRequestRepository
    {
        public static ProcessedReschedulingRequestRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ProcessedReschedulingRequestRepository>();
        }

        private const string FilePath = "../../../Resources/Data/processedReschedulingRequest.csv";

        private readonly Serializer<ProcessedReschedulingRequest> _serializer;

        private List<ProcessedReschedulingRequest> _processedReschedulingRequest;
        public ProcessedReschedulingRequestRepository()
        {
            _serializer = new Serializer<ProcessedReschedulingRequest>();
            _processedReschedulingRequest = _serializer.FromCSV(FilePath);
        }
        public void Add(ProcessedReschedulingRequest newProcessedReschedulingRequest)
        {
            newProcessedReschedulingRequest.Id = NextId();
            _processedReschedulingRequest.Add(newProcessedReschedulingRequest);
            _serializer.ToCSV(FilePath, _processedReschedulingRequest);
        }

        public List<ProcessedReschedulingRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ProcessedReschedulingRequest? GetById(int Id)
        {
            _processedReschedulingRequest = _serializer.FromCSV(FilePath);
            return _processedReschedulingRequest.Find(c => c.Id == Id);
        }

        public int NextId()
        {
            _processedReschedulingRequest = _serializer.FromCSV(FilePath);
            if (_processedReschedulingRequest.Count < 1)
            {
                return 1;
            }
            return _processedReschedulingRequest.Max(c => c.Id) + 1;
        }
    }
}
