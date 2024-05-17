using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guest
{
    public class OwnerReviewsViewModel
    {
        public ObservableCollection<GuestRating> GuestRatings {  get; set; }
       
        public User user { get; set; }

        public OwnerReviews OwnerReviews { get; set; }
        public OwnerReviewsViewModel(OwnerReviews ownerReviews, User user)
        {
            this.user = user;
            OwnerReviews = ownerReviews;
            GuestRatings = new ObservableCollection<GuestRating>();

            
            GuestRatings = GuestRatingService.GetInstance().Update(user);

            OwnerReviews.reviewsItems.ItemsSource = GuestRatings;

            OwnerReviews.NumberOfReviews.Content += GuestRatings.Count().ToString();
            OwnerReviews.AverageReviews.Content += GuestRatingService.GetInstance().GetAverageGrade(user).ToString();

            if (GuestBonusService.GetInstance().IsSuperGuest(user))
            {
                OwnerReviews.SuperGuestImage.Visibility = System.Windows.Visibility.Visible;
                OwnerReviews.SuperGuest.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                OwnerReviews.SuperGuestImage.Visibility = System.Windows.Visibility.Collapsed;
                OwnerReviews.SuperGuest.Visibility = System.Windows.Visibility.Collapsed;
            }

            OwnerReviews.GuestBonus.Content += GuestBonusService.GetInstance().GetBonus(user).ToString();
        }
    }
}
