using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public enum UserType
    {
        Owner, Guest, Guide, Tourist
    }
    public enum ScheduleStatus
    {
        Ready, Ongoing, Finished, Canceled
    }

    public enum CouponStatus
    {
        Valid, Expired
    }
    public enum ReviewStatus
    {
        Valid, Invalid
    }
    public enum TourSuggestionStatus
    {
        Accepted, Rejected, Pending
    }
    public enum NotificationStatus
    {
        Unread,Read
    }
}
