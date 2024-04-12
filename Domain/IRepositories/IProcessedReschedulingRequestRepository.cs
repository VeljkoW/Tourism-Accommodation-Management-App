using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IProcessedReschedulingRequestRepository
    {
        List<ProcessedReschedulingRequest> GetAll();

        ProcessedReschedulingRequest? GetById(int Id);

        int NextId();

        void Add(ProcessedReschedulingRequest newProcessedReschedulingRequest);
    }
}
