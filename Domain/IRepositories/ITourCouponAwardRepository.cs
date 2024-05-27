using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourCouponAwardRepository
    {
        List<TourCouponAward> GetAll();
        TourCouponAward? GetById(int Id);
        int NextId();
        TourCouponAward Add(TourCouponAward newTourCouponAward);
    }
}
