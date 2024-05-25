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
    public class TourCouponAwardService
    {
        private ITourCouponAwardRepository TourCouponAwardRepository { get; set; }
        public TourCouponAwardService(ITourCouponAwardRepository tourCouponAwardRepository) { TourCouponAwardRepository = tourCouponAwardRepository; }
        public static TourCouponAwardService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourCouponAwardService>();
        }
        public TourCouponAward Add(TourCouponAward newTourCouponAward)
        {
            return TourCouponAwardRepository.Add(newTourCouponAward);
        }

        public List<TourCouponAward> GetAll()
        {
            return TourCouponAwardRepository.GetAll();
        }

        public TourCouponAward? GetById(int Id)
        {
            return TourCouponAwardRepository.GetById(Id);
        }
        public void CreateACouponAward(int userId)
        {
            List<TourSchedule> tourSchedules = TourScheduleService.GetInstance().GetSchedulesForCouponAwards(userId);
            List<TourSchedule> tourScheduleForward = new List<TourSchedule>();
            if(tourSchedules.Count >= 5)
            {
                for(int i = 0; i < 5; i++)
                {
                    tourScheduleForward[i] = tourSchedules[i];
                }
                Add(new TourCouponAward(userId, tourScheduleForward));
                TourCouponService.GetInstance().Add(new TourCoupon(userId, "Tour Coupon", "Coupon awarded because you attended 5 different tours in the past year!", DateTime.Now, 6, CouponStatus.Valid));
            }
        }
    }
}
