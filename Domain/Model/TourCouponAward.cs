using BookingApp.Serializer;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Domain.Model
{
    public class TourCouponAward : ISerializable
    {
        public int Id;
        public int UserId;
        public List<TourSchedule> AttendedSchedules;

        public TourCouponAward() 
        {
            Id = -1;
            UserId = -1;
            AttendedSchedules = new List<TourSchedule>();
        }
        public TourCouponAward(int userId,List<TourSchedule> attendedSchedules)
        {
            this.UserId = userId;
            this.AttendedSchedules = attendedSchedules;
        }
        public string[] ToCSV()
        {
            string[] ret = { Id.ToString(),
                             UserId.ToString(),
                             ScheduleIdToCSV()
                            };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            if (values[2].Length > 0)
            {
                string[] ScheduleIds = values[2].Split(',');
                for (int i = 0; i < ScheduleIds.Length; i++)
                {
                    TourSchedule? schedule = new TourSchedule();
                    schedule = TourScheduleService.GetInstance().GetById(Convert.ToInt32(ScheduleIds[i]));
                    if (schedule != null)
                    {
                        AttendedSchedules.Add(schedule);
                    }
                }
            }
        }
        public string ScheduleIdToCSV()
        {
            string str = "";
            foreach (TourSchedule ts in AttendedSchedules)
            {
                str += ts.Id + ",";
            }
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return str;
        }
    }
}
