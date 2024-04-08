using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourCoupon : ISerializable
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public string Name {  get; set; }
        public string Reason { get; set; }
        public DateTime AcquiredDate {  get; set; }
        public CouponStatus Status { get; set; }
        public int ExpirationMonths {  get; set; }
        public DateTime ExpirationDate { get; set; }

        public TourCoupon() 
        {
            Id = 0;
            UserId = -1;
            Name = string.Empty;
            Reason = string.Empty;
            AcquiredDate = DateTime.MinValue;
            Status = CouponStatus.Expired;
            ExpirationMonths = 0;
            ExpirationDate = AcquiredDate.AddMonths(ExpirationMonths);
        }
        public TourCoupon(int id,int userId,string name,string reason,DateTime acquiredDate,int expirationMonths, CouponStatus status)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Reason = reason;
            AcquiredDate = acquiredDate;
            ExpirationMonths = expirationMonths;
            ExpirationDate = AcquiredDate.AddMonths(ExpirationMonths);
            Status = status;
        }

        public string[] ToCSV()
        {
            string[] ret = { Id.ToString(),
                             UserId.ToString(),
                             Name,
                             Reason,
                             AcquiredDate.ToString(),
                             ExpirationMonths.ToString(),
                             Status.ToString()
                            };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            Name = values[2];
            Reason = values[3];
            AcquiredDate = Convert.ToDateTime(values[4]);
            ExpirationMonths = Convert.ToInt32(values[5]);
            Status = (CouponStatus)Enum.Parse(typeof(CouponStatus), values[6]);
        }


    }
}
