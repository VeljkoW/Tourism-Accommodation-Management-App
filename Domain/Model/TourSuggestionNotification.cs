using BookingApp.Serializer;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourSuggestionNotification : ISerializable
    {
        public int Id { get; set; }
        public int TourSuggestionId {  get; set; }
        public DateTime NotificationDate { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public string CityName {  get; set; }
        public DateTime TourDate { get; set; }
        public TourSuggestionNotification() 
        {
            Id = -1;
            TourSuggestionId = -1;
            NotificationDate = DateTime.MinValue;
            NotificationStatus = NotificationStatus.Unread;
            CityName = string.Empty;
            TourDate = DateTime.MinValue;
        }
        public TourSuggestionNotification(int tourSuggestionId,DateTime notificationDate)
        { 
            this.TourSuggestionId = tourSuggestionId;
            this.NotificationDate = notificationDate;
            this.NotificationStatus = NotificationStatus.Unread;
            CityName = string.Empty;
            TourDate= DateTime.MinValue;
        }
        public string[] ToCSV()
        {
            string[] ret = {
                            Id.ToString(),
                            TourSuggestionId.ToString(),
                            NotificationDate.ToString(),
                            NotificationStatus.ToString()  
                            };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourSuggestionId = Convert.ToInt32(values[1]);
            NotificationDate = Convert.ToDateTime(values[2]);
            NotificationStatus = (NotificationStatus)Enum.Parse(typeof(NotificationStatus), values[3]);
        }
    }
}
