using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourSuggestionNotificationRepository
    {
        List<TourSuggestionNotification> GetAll();
        TourSuggestionNotification? GetById(int Id);
        int NextId();
        void Add(TourSuggestionNotification newTourSuggestionNotification);
        TourSuggestionNotification? Update(TourSuggestionNotification tourSuggestionNotification);
    }
}
