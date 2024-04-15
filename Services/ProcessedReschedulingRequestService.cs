using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class ProcessedReschedulingRequestService
    {
        public IProcessedReschedulingRequestRepository ProcessedReschedulingRequestRepository {get;set;}
        public ProcessedReschedulingRequestService(IProcessedReschedulingRequestRepository processedReschedulingRequestRepository) { }
        public static ProcessedReschedulingRequestService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ProcessedReschedulingRequestService>();
        }
        public void Add(ProcessedReschedulingRequest newProcessedReschedulingRequest)
        {
            ProcessedReschedulingRequestRepository.Add(newProcessedReschedulingRequest);
        }
        public List<ProcessedReschedulingRequest> GetAll()
        {
            return ProcessedReschedulingRequestRepository.GetAll();
        }
        public ProcessedReschedulingRequest? GetById(int Id)
        {
            return ProcessedReschedulingRequestRepository.GetById(Id);
        }
    }
}
