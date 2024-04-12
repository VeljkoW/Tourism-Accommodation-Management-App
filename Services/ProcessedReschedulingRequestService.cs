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
        private IProcessedReschedulingRequestRepository processedReschedulingRequestRepository = ProcessedReschedulingRequestRepository.GetInstance();
        public ProcessedReschedulingRequestService() { }
        public static ProcessedReschedulingRequestService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ProcessedReschedulingRequestService>();
        }
        public void Add(ProcessedReschedulingRequest newProcessedReschedulingRequest)
        {
            processedReschedulingRequestRepository.Add(newProcessedReschedulingRequest);
        }
        public List<ProcessedReschedulingRequest> GetAll()
        {
            return processedReschedulingRequestRepository.GetAll();
        }
        public ProcessedReschedulingRequest? GetById(int Id)
        {
            return processedReschedulingRequestRepository.GetById(Id);
        }
    }
}
