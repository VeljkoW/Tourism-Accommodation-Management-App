using BookingApp.Repository.TourRepositories;
using BookingApp.Serializer;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourSuggestion : ISerializable
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public Location Location { get; set; }
        public int LocationId {  get; set; }
        public string Description {  get; set; }
        public string Language {  get; set; }
        public int NumberOfPeople {  get; set; }
        public List<TourPerson> Tourists { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime Date {  get; set; }
        public TourSuggestionStatus Status { get; set; }

        public TourSuggestion() 
        {
            Id = -1;
            UserId = -1;
            Location = new Location();
            LocationId = -1;
            Description = string.Empty;
            Language = string.Empty;
            NumberOfPeople = 0;
            Tourists = new List<TourPerson>();
            FromDate = DateTime.MinValue;
            ToDate = DateTime.MinValue;
            Date = DateTime.MinValue;
            Status = TourSuggestionStatus.Rejected;
        }

        public TourSuggestion(int userId,Location location, string description, string language, int numberOfPeople, List<TourPerson> tourists,DateTime fromDate,DateTime toDate,DateTime date,TourSuggestionStatus status)
        {
            UserId = userId;
            Location = location;
            LocationId = Location.Id;
            Description = description;
            Language = language;
            NumberOfPeople = numberOfPeople;
            Tourists = tourists;
            FromDate = fromDate;
            ToDate = toDate;
            Date = date;
            Status = status;
        }

        public string[] ToCSV()
        {
            string[] ret = {
                            Id.ToString(),
                            UserId.ToString(),
                            LocationId.ToString(),
                            Description,
                            Language,
                            NumberOfPeople.ToString(),
                            TouristIdToCSV(),
                            FromDate.ToString(),
                            ToDate.ToString(),
                            Date.ToString(),
                            Status.ToString()
                            };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            LocationId = Convert.ToInt32(values[2]);
            Description = values[3];
            Language = values[4];
            NumberOfPeople = Convert.ToInt32(values[5]);
            if (values[6].Length > 0)
            {
                string[] PeopleIds = values[6].Split(',');
                for (int i = 0; i < PeopleIds.Length; i++)
                {
                    TourPerson? person = new TourPerson();
                    person = TourPersonService.GetInstance().GetById(Convert.ToInt32(PeopleIds[i]));
                    if (person != null)
                    {
                        Tourists.Add(person);
                    }
                }
            }
            FromDate = Convert.ToDateTime(values[7]);
            ToDate = Convert.ToDateTime(values[8]);
            Date = Convert.ToDateTime(values[9]);
            Status = (TourSuggestionStatus)Enum.Parse(typeof(TourSuggestionStatus), values[10]);
        }
        public string TouristIdToCSV()
        {
            string str = "";
            foreach (TourPerson p in Tourists)
            {
                str += p.Id + ",";
            }
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return str;
        }
    }
}
