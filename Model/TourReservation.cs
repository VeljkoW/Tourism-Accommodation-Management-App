using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.Model
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int TourScheduleId {  get; set; }    //Removed the TourId property because it can be accessed in TourSchedule
        public List<TourPerson> People { get; set; }

        public TourReservation(int id,int userId,int tourScheduleId,List<TourPerson> people) 
        {
            Id = id;
            UserId = userId;
            TourScheduleId = tourScheduleId;
            People = people;
        }
        public TourReservation() 
        {
            Id=0;
            UserId=0;
            TourScheduleId=0;
            People = new List<TourPerson>();
        }

        public string[] ToCSV()
        {
            string[] ret = {
                Id.ToString(),
                UserId.ToString(),
                TourScheduleId.ToString(),
                PersonIdToCSV()
            };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            TourScheduleId = Convert.ToInt32(values[2]); 
            if (values[3].Length > 0)
            {
                string[] PeopleIds = values[3].Split(',');
                for (int i = 0; i < PeopleIds.Length; i++)
                {
                    TourPerson ?person = new TourPerson();
                    TourPersonRepository tourPersonRepository = new TourPersonRepository();
                    person = tourPersonRepository.GetById(Convert.ToInt32(PeopleIds[i]));
                    if (person != null)
                    {
                        People.Add(person);
                    }
                }
            }
        }

        public string PersonIdToCSV()
        {
            string str = "";
            foreach (TourPerson p in People)
            {
                str += p.Id + ",";
            }
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return str;
        }
    }
}
