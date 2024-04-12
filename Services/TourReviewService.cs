using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourReviewService
    {
        private ITourReviewRepository tourReviewRepository = TourReviewRepository.GetInstance();
        public TourReviewService() { }
        public static TourReviewService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourReviewService>();
        }
        public TourReview Add(TourReview newTourReview)
        {
            return tourReviewRepository.Add(newTourReview);
        }

        public List<TourReview> GetAll()
        {
            return tourReviewRepository.GetAll();
        }

        public TourReview? GetById(int Id)
        {
            return tourReviewRepository.GetById(Id);
        }
        public TourReview? Update(TourReview t)
        {
            return tourReviewRepository.Update(t);
        }

        public static Dictionary<TourSchedule, List<TourReview>> LoadFinishedTours()
        {
            Dictionary<TourSchedule, List<TourReview>> ret = new Dictionary<TourSchedule, List<TourReview>>();
            List<TourReview> tourReviews = GetInstance().GetAll();
            List<int> reviewedTours = tourReviews.Select(t => t.TourScheduleId).ToList();
            if (reviewedTours.Count == 0)
            {
                return ret;
            }
            List<TourSchedule> tourSchedules = TourScheduleService.GetInstance().GetAll();
            tourSchedules = tourSchedules.Where(t => reviewedTours.Contains(t.Id)).ToList();
            foreach (TourSchedule t in tourSchedules)
            {
                List<TourReview> reviews = tourReviews.Where(x => x.TourScheduleId == t.Id).ToList();
                ret.Add(t,reviews);
            }
            return ret;
        }
    }
}
