using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourAttendenceNotification : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TourPersonId {  get; set; }
        public string TourPersonNameSurname {  get; set; }
        public string TourName {  get; set; }
        public string KeypointName {  get; set; }
        public DateTime Date { get; set; }
        public bool ConfirmedAttendence {  get; set; }

        public TourAttendenceNotification()
        {
            Id = 0;
            UserId = -1;
            TourPersonId = -1;
            TourPersonNameSurname = string.Empty;
            TourName = string.Empty;
            KeypointName = string.Empty;
            Date = DateTime.Now;
            ConfirmedAttendence = false;
        }
        public TourAttendenceNotification(int userId,int tourPersonId,DateTime date,bool confirmedAttendance)
        {
            UserId = userId;
            TourPersonId = tourPersonId;
            TourPersonNameSurname = string.Empty;
            TourName = string.Empty;
            KeypointName = string.Empty;
            Date = date;
            ConfirmedAttendence = confirmedAttendance;
        }

        public string[] ToCSV()
        {
            string[] ret = { Id.ToString(),
                             UserId.ToString(),
                             TourPersonId.ToString(),
                             Date.ToString(),
                             ConfirmedAttendence.ToString()
                            };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            TourPersonId = Convert.ToInt32(values[2]);
            Date = Convert.ToDateTime(values[3]);
            ConfirmedAttendence = Convert.ToBoolean(values[4]);
        }
    }
}
