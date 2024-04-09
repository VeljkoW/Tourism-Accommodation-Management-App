using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourCouponRepository
    {
        List<TourCoupon> GetAll();
        TourCoupon? GetById(int Id);
        int NextId();
        TourCoupon Add(TourCoupon newTourCoupon);
        TourCoupon? Update(TourCoupon tourCoupon);
    }
}
