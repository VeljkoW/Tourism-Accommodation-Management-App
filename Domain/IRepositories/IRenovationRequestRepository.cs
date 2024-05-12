using BookingApp.Domain.Model;
using BookingApp.View.Guest.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IRenovationRequestRepository
    {
        void Add(RenovationRequest renovationRequest);
        List<RenovationRequest> GetAll();
        int NextId();
        RenovationRequest? GetById(int Id);
        void DeleteById(int Id);
    }
}
