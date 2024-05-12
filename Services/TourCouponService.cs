using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourCouponService
    {
        private ITourCouponRepository TourCouponRepository {get;set;}
        public TourCouponService(ITourCouponRepository tourCouponRepository) { TourCouponRepository = tourCouponRepository; }
        public static TourCouponService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourCouponService>();
        }
        public TourCoupon Add(TourCoupon newTourCoupon)
        {
            return TourCouponRepository.Add(newTourCoupon);
        }

        public List<TourCoupon> GetAll()
        {
            return TourCouponRepository.GetAll();
        }

        public TourCoupon? GetById(int Id)
        {
            return TourCouponRepository.GetById(Id);
        }
        public TourCoupon? Update(TourCoupon tourCoupon)
        {
            return TourCouponRepository.Update(tourCoupon);
        }
    }
}
