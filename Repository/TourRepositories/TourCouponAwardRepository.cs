using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourCouponAwardRepository : ITourCouponAwardRepository
    {
        public static TourCouponAwardRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourCouponAwardRepository>();
        }
        private const string FilePath = "../../../Resources/Data/tourcouponawards.csv";

        private readonly Serializer<TourCouponAward> _serializer;

        private List<TourCouponAward> _tourCouponAwards;

        public TourCouponAwardRepository()
        {
            _serializer = new Serializer<TourCouponAward>();
            _tourCouponAwards = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourCouponAwards = _serializer.FromCSV(FilePath);
            if (_tourCouponAwards.Count < 1)
            {
                return 1;
            }
            return _tourCouponAwards.Max(c => c.Id) + 1;
        }
        public TourCouponAward Add(TourCouponAward newTourCouponAward)
        {
            newTourCouponAward.Id = NextId();
            //newTourCouponAward.Name = "Tour Coupon #" + newTourCouponAward.Id.ToString();
            _tourCouponAwards.Add(newTourCouponAward);
            _serializer.ToCSV(FilePath, _tourCouponAwards);
            return newTourCouponAward;
        }
        public List<TourCouponAward> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public TourCouponAward? GetById(int Id)
        {
            _tourCouponAwards = _serializer.FromCSV(FilePath);
            return _tourCouponAwards.Find(c => c.Id == Id);
        }
    }
}
