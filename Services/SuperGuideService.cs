using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class SuperGuideService
    {
        private TourService tourService =TourService.GetInstance();
        private TourScheduleService tourScheduleService =TourScheduleService.GetInstance();
        private TourReviewService tourReviewService = TourReviewService.GetInstance();
        public ISuperGuideRepository SuperGuideRepository { get; set; }
        public SuperGuideService(ISuperGuideRepository superGuideRepository)
        {
            SuperGuideRepository = superGuideRepository;
        }
        public static SuperGuideService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<SuperGuideService>();
        }
        public SuperGuide Add(SuperGuide superGuide)
        {
            return SuperGuideRepository.Add(superGuide);
        }
        public bool Delete(SuperGuide superGuide)
        {
            return SuperGuideRepository.Delete(superGuide);
        }
        public List<SuperGuide> GetAll()
        {
            return SuperGuideRepository.GetAll();
        }
        public void UpdateSuperGuide(int userId)
        {
            List<Tour> tours =tourService.GetAll();
            List<string> languages = tours.Where(t => t.OwnerId == userId).Select(t => t.Language).Distinct().ToList();

            List<TourSchedule> schedules = tourScheduleService.GetAll();
            List <TourReview> reviews = tourReviewService.GetAll();

            foreach(string language in languages)
            {
                List<int> tourIds= tours.Where(t=>t.OwnerId == userId && t.Language.Equals(language)).Select(t=>t.Id).ToList();
                List<TourSchedule> filteredschedules = schedules.Where(t => tourIds.Contains(t.TourId)).ToList();
                List<int> scheduleIds = filteredschedules.Where(t => t.Date >= DateTime.Now.AddYears(-1)).Select(t => t.Id).ToList();

                List<TourReview> filteredreviews = reviews.Where(t => scheduleIds.Contains(t.TourScheduleId)).ToList();
                if(filteredreviews.Count == 0)
                {
                    Delete(new SuperGuide(userId, language));
                    continue;
                }
                //if (filteredreviews.Count < 20)
                //{
                //    Delete(new SuperGuide(userId, language));
                //    continue;
                //}
                double averageGrade = filteredreviews.Average(t => (t.TourEnjoyment + t.GuideKnowledge + t.GuideSpeech) / 3);
                if (averageGrade < 2.6)
                {
                    Delete(new SuperGuide(userId, language));
                    continue;
                }
                else
                {
                    Add(new SuperGuide(userId, language));
                }
            }
        }
    }
}
