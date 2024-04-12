using BookingApp.Domain.Model;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class GuestReviewsViewModel
    {
        public User User { get; set; }
        public OwnerRating SelectedOwnerRating {  get; set; }
        public ObservableCollection<OwnerRating> OwnerRatings {  get; set; }
        public GuestReviewsViewModel(User User, OwnerRating SelectedOwnerRating)
        {
            this.User = User;
            this.SelectedOwnerRating = SelectedOwnerRating;
            OwnerRatings = new ObservableCollection<OwnerRating>();
            Update();
        }
        public ObservableCollection<OwnerRating> Update()
        {
            OwnerRatings.Clear();
            foreach (OwnerRating ownerRating in OwnerRatingService.GetInstance().GetAll())
            {
                if (User.Id == ownerRating.ownerId)
                {
                    foreach (GuestRating guestRating in GuestRatingService.GetInstance().GetAll())
                    {
                        if (guestRating.ownerId == User.Id && guestRating.guestId == ownerRating.guestId)
                        {
                            OwnerRatings.Add(ownerRating);
                        }
                    }
                }
            }
            return OwnerRatings;
        }
    }
}
