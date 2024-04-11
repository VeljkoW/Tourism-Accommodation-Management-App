using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourReviewRepository
    {
        List<TourReview> GetAll();
        TourReview? GetById(int Id);
        int NextId();
        TourReview Add(TourReview newTourReview);
    }
}
