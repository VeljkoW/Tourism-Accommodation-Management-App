using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.View.Guide;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourSuggestionService
    {
        public ITourSuggestionRepository TourSuggestionRepository { get; set; }
        public TourSuggestionService(ITourSuggestionRepository tourSuggestionRepository)
        {
            TourSuggestionRepository = tourSuggestionRepository;
        }
        public static TourSuggestionService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourSuggestionService>();
        }
        public void Add(TourSuggestion newTourSuggestion)
        {
            TourSuggestionRepository.Add(newTourSuggestion);
        }

        public List<TourSuggestion> GetAll()
        {
            return TourSuggestionRepository.GetAll();
        }

        public TourSuggestion? GetById(int Id)
        {
            return TourSuggestionRepository.GetById(Id);
        }
        public TourSuggestion? Update(TourSuggestion tourSuggestion)
        {
            return TourSuggestionRepository.Update(tourSuggestion);
        }
        public List<String> GetAllLanguages(int id)
        {
            List<String> languages = new List<String>();
            foreach(TourSuggestion tourSuggestion in GetAllByUserId(id))
            {
                bool exists = false;
                foreach (string s in languages)
                {
                    if (s == tourSuggestion.Language)
                    {
                        exists = true;
                        break;
                    }
                }
                if(!exists)
                {
                    languages.Add(tourSuggestion.Language);
                }
            }
            return languages;
        }
        public List<String> GetAllLocations(int id)
        {
            List<String> locations = new List<String>();
            foreach (TourSuggestion tourSuggestion in GetAllByUserId(id))
            {
                bool exists = false;
                Location ?location = LocationService.GetInstance().GetById(tourSuggestion.LocationId);
                string stateCity = "";
                if (location != null)
                {
                    stateCity = location.State + ", " + location.City;
                }
                foreach (string s in locations)
                {
                    if (s == stateCity)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists && stateCity != "")
                {
                    locations.Add(stateCity);
                }
            }
            return locations;
        }
        public int CountRequestsByLanguage(string language,int id)
        {
            int count = 0;

            foreach(TourSuggestion tourSuggestion in GetAllByUserId(id))
            {
                if(tourSuggestion.Language == language)
                {
                    count++;
                }
            }
            return count;
        }
        public int CountRequestsByLocation(string location,int id)
        {
            int count = 0;

            foreach(TourSuggestion tourSuggestion in GetAllByUserId(id))
            {
                Location? location1 = LocationService.GetInstance().GetById(tourSuggestion.LocationId);
                string stateCity = "";
                if (location1 != null)
                {
                    stateCity = location1.State + ", " + location1.City;
                }
                if(location == stateCity)
                {
                    count++;
                }
            }
            return count;
        }
        public List<TourSuggestion> GetAllByUserId(int id)
        {
            List<TourSuggestion> tourSuggestions = new List<TourSuggestion>();
            foreach(TourSuggestion tourSuggestion in GetAll())
            {
                if(id == tourSuggestion.UserId)
                {
                    tourSuggestions.Add(tourSuggestion);
                }
            }
            return tourSuggestions;
        }
        public List<TourSuggestion> GetAllByUserIdAndYear(int id,int year)
        {
            List<TourSuggestion> tourSuggestions = new List<TourSuggestion>();
            foreach (TourSuggestion tourSuggestion in GetAll())
            {
                if (id == tourSuggestion.UserId && year.ToString() == tourSuggestion.FromDate.Year.ToString())
                {
                    tourSuggestions.Add(tourSuggestion);
                }
            }
            return tourSuggestions;
        }
        public double GetPercentageOfToursAccepted(List<TourSuggestion> tourSuggestions)
        {
            double percentage = 0;
            List<TourSuggestion> acceptedTourSuggestions = new List<TourSuggestion>();
            foreach(TourSuggestion tourSuggestion in tourSuggestions)
            {
                if(tourSuggestion.Status == TourSuggestionStatus.Accepted)
                {
                    acceptedTourSuggestions.Add(tourSuggestion);
                }
            }
            if(acceptedTourSuggestions.Count > 0)
            {
                percentage = Math.Round((((double)acceptedTourSuggestions.Count/(double)tourSuggestions.Count) * 100),1);
            }
            return percentage; 
        }
        public double GetPercentageOfToursRejected(List<TourSuggestion> tourSuggestions)
        {
            double percentage = 0;
            List<TourSuggestion> rejectedTourSuggestions = new List<TourSuggestion>();
            foreach (TourSuggestion tourSuggestion in tourSuggestions)
            {
                if (tourSuggestion.Status == TourSuggestionStatus.Rejected)
                {
                    rejectedTourSuggestions.Add(tourSuggestion);
                }
            }
            if (rejectedTourSuggestions.Count > 0)
            {
                percentage = Math.Round((((double)rejectedTourSuggestions.Count / (double)tourSuggestions.Count) * 100),1);
            }
            return percentage;
        }
        public double GetAverageNumberOfTouristsAccepted(List<TourSuggestion> tourSuggestions)
        {
            double average = 0;
            double NumberOfPeople = 0;
            double AcceptedNumberOfPeople = 0;
            foreach (TourSuggestion tourSuggestion in tourSuggestions)
            {
                NumberOfPeople += tourSuggestion.NumberOfPeople;
                if (tourSuggestion.Status == TourSuggestionStatus.Accepted)
                {
                    AcceptedNumberOfPeople += tourSuggestion.NumberOfPeople;
                }
            }
            if(NumberOfPeople > 0 && AcceptedNumberOfPeople > 0) 
            {
                average = Math.Round((AcceptedNumberOfPeople/ NumberOfPeople),1);
            }
            
            return average;
        }
        public List<String> GetAllTourSuggestionYears(int id)
        {
            List<String> Years = new List<String>();
            foreach(TourSuggestion tourSuggestion in GetAllByUserId(id))
            {
                bool exists = false;
                foreach(string year in Years)
                {
                    if(tourSuggestion.FromDate.Year.ToString() == year)
                    {
                        exists = true;
                        break;
                    }
                }
                if(!exists)
                {
                    Years.Add(tourSuggestion.FromDate.Year.ToString());
                }
            }

            return Years;
        }
        public int GetMostRequestedLocation()
        {
            List<TourSuggestion> tourSuggestions = GetAll();
            tourSuggestions = tourSuggestions.Where(t=>t.ToDate.AddYears(1) > DateTime.Today).ToList();
            int locationId = tourSuggestions
            .GroupBy(ts => ts.LocationId)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
            var location = LocationService.GetInstance().GetById(locationId);
            return location.Id;
        }
        public string GetMostRequestedLanguage()
        {
            List<TourSuggestion> tourSuggestions = GetAll();
            tourSuggestions = tourSuggestions.Where(t=>t.ToDate.AddYears(1) > DateTime.Now).ToList();
            return tourSuggestions
            .GroupBy(ts => ts.Language)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
        }
        public bool IsGuideFree(DateTime date)
        {
            List<int> tourIds = TourService.GetInstance().GetAll().Where(t => t.OwnerId == GuideMainWindow.UserId).Select(t=>t.Id).ToList();
            List<TourSchedule> tourSchedules = TourScheduleService.GetInstance().GetAll().Where(t=> tourIds.Contains( t.TourId)).ToList();
            foreach(TourSchedule schedule in tourSchedules)
            {
                int duration = TourService.GetInstance().GetById(schedule.TourId).Duration;
                DateTime endtime = schedule.Date.AddHours(duration);
                if (date > schedule.Date)
                {
                    if(date < endtime)
                    {
                    return false;
                    }
                }

            }
            return true;
        }
    }
}
