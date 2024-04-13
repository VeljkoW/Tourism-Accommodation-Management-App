using BookingApp.Domain.Model;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using BookingApp.View.Owner;
using GuestRatingModel = BookingApp.Domain.Model.GuestRating;

namespace BookingApp.ViewModel.Owner
{
    public class GuestReviewsViewModel
    {
        public User User { get; set; }
        public int NumberOfReviews { get; set; }
        public double AverageGrade { get; set; }
        public OwnerRating SelectedOwnerRating {  get; set; }
        public ObservableCollection<OwnerRating> OwnerRatings {  get; set; }
        public GuestReviews GuestReviews {  get; set; }
        public GuestReviewsViewModel(GuestReviews GuestReviews, User User, OwnerRating SelectedOwnerRating)
        {
            this.User = User;
            this.SelectedOwnerRating = SelectedOwnerRating;
            this.GuestReviews = GuestReviews;
            AverageGrade = 0;
            OwnerRatings = new ObservableCollection<OwnerRating>();
            //Update(); already in use in IsSuperOwner() method
            IsSuperOwner();
        }
        public void IsSuperOwner()
        {
            OwnerRatings = Update();
            AverageGrade = 0;
            foreach (OwnerRating ownerRating in OwnerRatings)
            {
                if(ownerRating.ownerId == User.Id)
                {
                    AverageGrade += (double)(ownerRating.Cleanliness + ownerRating.OwnerIntegrity) / 2;
                }
            }//IZMENI OWNER RATING COUNT NA SAMO TOG VLASNIKA
            AverageGrade /= OwnerRatings.Count;
            NumberOfReviews = OwnerRatings.Count;
            DisplaySuperOwner(AverageGrade);
        }
        public void DisplaySuperOwner(double averageGrade)
        {
            if (OwnerRatings.Count >= 50 && averageGrade >= 4.5)
            {
                GuestReviews.SuperownerLabel.Content = "You are a Superowner!";
                GuestReviews.starImage.Visibility = Visibility.Visible;
            }
            else
            {
                GuestReviews.SuperownerLabel.Content = "You are a Basic Owner!";
                GuestReviews.starImage.Visibility = Visibility.Collapsed;
            }
        }
        public ObservableCollection<OwnerRating> Update()
        {
            OwnerRatings.Clear();
            foreach (OwnerRating ownerRating in OwnerRatingService.GetInstance().GetAll())
            {
                if (User.Id == ownerRating.ownerId)
                {
                    AddRatedGuest(ownerRating);
                }
            }
            return OwnerRatings;
        }
        public void AddRatedGuest(OwnerRating ownerRating)
        {
            foreach (GuestRatingModel guestRating in GuestRatingService.GetInstance().GetAll())
            {
                if (guestRating.ownerId == User.Id && guestRating.guestId == ownerRating.guestId)
                {
                    OwnerRatings.Add(ownerRating);
                }
            }
        }
    }
}
