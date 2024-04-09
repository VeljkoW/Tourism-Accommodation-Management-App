using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourImageRepository
    {
        void Add(TourImage tourImage);
        List<TourImage> GetAll();
    }
}
