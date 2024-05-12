using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourNotificationTemp
    {
        public int UserId {  get; set; }
        public string Reason { get; set; }
        public TourNotificationTemp(int userId, string reason)
        {
            UserId = userId;
            Reason = reason;
        }
    }
}
