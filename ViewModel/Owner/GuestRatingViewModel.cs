using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Services;
using BookingApp.View.Owner;
using System;
using System.Collections.ObjectModel;
using BookingApp.ViewModel.Owner;
using GuestRatingModel = BookingApp.Domain.Model.GuestRating;
using GuestRatingPage = BookingApp.View.Owner.GuestRating;

namespace BookingApp.ViewModel.Owner
{
    public class GuestRatingViewModel
    {
        public RelayCommand RateGuest => new RelayCommand(execute => RateGuestExecute(), canExecute => RateGuestCanExecute());
        public User user { get; set; }
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public GuestRatingPage GuestRatingPage {  get; set; }
        public ObservableCollection<ReservedAccommodation> ReservedAccommodations { get; set; }
        public ReservedAccommodation SelectedReservedAccommodations { get; set; }
        public GuestRatingViewModel(GuestRatingPage GuestRatingPage, OwnerMainWindow OwnerMainWindow, User user) 
        {
            this.user = user;
            this.OwnerMainWindow = OwnerMainWindow;
            this.GuestRatingPage = GuestRatingPage;
            ReservedAccommodations = new ObservableCollection<ReservedAccommodation>();
        }
        public void RateGuestExecute()
        {
            Comment comment = new Comment();
            comment.Text = GuestRatingPage.CommentTextBox.Text;
            comment.CreationTime = DateTime.Now;
            comment.User = UserService.GetInstance().GetById(user.Id);
            comment = CommentService.GetInstance().Save(comment);

            GuestRatingModel GuestRatingModel = new GuestRatingModel();
            GuestRatingModel.ownerId = user.Id;
            GuestRatingModel.guestId = SelectedReservedAccommodations.guestId;
            GuestRatingModel.CommentId = comment.Id;
            GuestRatingModel.Cleanliness = Convert.ToInt32(GuestRatingPage.CleanlinessComboBox.SelectionBoxItem);
            GuestRatingModel.FollowingGuidelines = Convert.ToInt32(GuestRatingPage.FollowingGuidelinesComboBox.SelectionBoxItem);
            GuestRatingService.GetInstance().Add(GuestRatingModel);

            Update();
        }
        public bool RateGuestCanExecute()
        {
            if (SelectedReservedAccommodations == null || 
                GuestRatingPage.CleanlinessComboBox.SelectedItem == null ||
                GuestRatingPage.FollowingGuidelinesComboBox.SelectedItem == null ||
                GuestRatingPage.CommentTextBox.Text.Equals(""))
            {
                return false;
            }
            else
                return true;
        }

        public ObservableCollection<ReservedAccommodation> Update()
        {
            ReservedAccommodations.Clear();
            foreach (ReservedAccommodation tempReservedAccommodation in ReservedAccommodationService.GetInstance().GetAll())
                foreach (Accommodation accommodation in AccommodationRepository.GetInstance().GetAll())
                    if (tempReservedAccommodation.accommodationId == accommodation.Id && user.Id == accommodation.OwnerId)
                    {
                        if (GuestRatingService.GetInstance().GetAll().Count == 0)
                            AvailableForRating(tempReservedAccommodation, ReservedAccommodations);
                        else
                        {
                            if (!IsAlreadyRated(tempReservedAccommodation))
                                AvailableForRating(tempReservedAccommodation, ReservedAccommodations);
                        }
                    }
            return ReservedAccommodations;
        }
        public bool IsAlreadyRated(ReservedAccommodation tempReservedAccommodation)
        {
            foreach (GuestRatingModel GuestRatingModel in GuestRatingService.GetInstance().GetAll())
                if (GuestRatingModel.guestId == tempReservedAccommodation.guestId && GuestRatingModel.ownerId == user.Id)
                    return true;
            return false;
        }
        public void AvailableForRating(ReservedAccommodation ReservedAccommodation, ObservableCollection<ReservedAccommodation> ReservedAccommodations)
        {
            if ((DateTime.Now > ReservedAccommodation.checkOutDate) &&
                (DateTime.Now - ReservedAccommodation.checkOutDate).Days <= 5)
                ReservedAccommodations.Add(ReservedAccommodation);
        }
    }
}
