using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourNotification : ISerializable
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int TourId {  get; set; }
        public DateTime NotificationDate {  get; set; }
        public NotificationStatus Status { get; set; }
        public string Reason { get; set; }
        public string TourName {  get; set; }
        public string Language {  get; set; }
        public string Location {  get; set; }
        public TourNotification() 
        {
            Id = -1;
            UserId = -1;
            TourId = -1;
            NotificationDate = DateTime.MinValue;
            Status = NotificationStatus.Read;
            Reason = string.Empty;
            TourName = string.Empty;
            Language = string.Empty;
            Location = string.Empty;
        }
        public TourNotification(int userId, int tourId, DateTime notificationDate,string reason)
        {
            UserId = userId;
            TourId = tourId;
            NotificationDate = notificationDate;
            Status = NotificationStatus.Unread;
            Reason = reason;
            TourName = string.Empty;
            Language = string.Empty;
            Location = string.Empty;
        }
        public string[] ToCSV()
        {
            string[] ret = {
                            Id.ToString(),
                            UserId.ToString(),
                            TourId.ToString(),
                            NotificationDate.ToString(),
                            Reason,
                            Status.ToString()
                            };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            TourId = Convert.ToInt32(values[2]);
            NotificationDate = Convert.ToDateTime(values[3]);
            Reason = values[4];
            Status = (NotificationStatus)Enum.Parse(typeof(NotificationStatus), values[5]);
        }
    }
}
