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
    public class RenovationRequestService
    {
        public IRenovationRequestRepository RenovationRequestRepository { get; set; }
        public RenovationRequestService(IRenovationRequestRepository renovationRequestRepository) { RenovationRequestRepository = renovationRequestRepository; }
        public static RenovationRequestService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<RenovationRequestService>();
        }
        public void Add(RenovationRequest renovationRequest)
        {
            RenovationRequestRepository.Add(renovationRequest);
        }

        public List<RenovationRequest> GetAll()
        {
            return RenovationRequestRepository.GetAll();
        }

        public RenovationRequest? GetById(int Id)
        {
            return RenovationRequestRepository.GetById(Id);
        }
    }
}
