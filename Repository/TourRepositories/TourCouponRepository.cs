using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourCouponRepository
    {
        private const string FilePath = "../../../Resources/Data/tourcoupons.csv";

        private readonly Serializer<TourCoupon> _serializer;

        private List<TourCoupon> _tourCoupons;

        public TourCouponRepository()
        {
            _serializer = new Serializer<TourCoupon>();
            _tourCoupons = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourCoupons = _serializer.FromCSV(FilePath);
            if (_tourCoupons.Count < 1)
            {
                return 1;
            }
            return _tourCoupons.Max(c => c.Id) + 1;
        }
        public TourCoupon Add(TourCoupon newTourCoupon)
        {
            newTourCoupon.Id = NextId();
            _tourCoupons.Add(newTourCoupon);
            _serializer.ToCSV(FilePath, _tourCoupons);
            return newTourCoupon;
        }
        public List<TourCoupon> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public TourCoupon? GetById(int Id)
        {
            _tourCoupons = _serializer.FromCSV(FilePath);
            return _tourCoupons.Find(c => c.Id == Id);
        }

        public TourCoupon? Update(TourCoupon tourCoupon)
        {
            TourCoupon? oldTourCoupon = GetById(tourCoupon.Id);
            if (oldTourCoupon is null) return null;
            oldTourCoupon.Name = tourCoupon.Name;
            oldTourCoupon.Reason = tourCoupon.Reason;
            oldTourCoupon.AcquiredDate = tourCoupon.AcquiredDate;
            oldTourCoupon.Status = tourCoupon.Status;
            oldTourCoupon.ExpirationMonths= tourCoupon.ExpirationMonths;
            _serializer.ToCSV(FilePath, _tourCoupons);
            return oldTourCoupon;
        }

    }
}
