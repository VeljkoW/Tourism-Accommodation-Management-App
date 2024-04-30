using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class RenovationRequestPageViewModel
    {
        public RenovationRequestPage RenovationRequestPage {  get; set; }
        public ObservableCollection<RenovationRequest> RenovationRequests { get; set; }
        public RenovationRequestPageViewModel(RenovationRequestPage RenovationRequestPage)
        {
            this.RenovationRequestPage = RenovationRequestPage;
            RenovationRequests = new ObservableCollection<RenovationRequest>();
            foreach (RenovationRequest renovationRequest in RenovationRequestService.GetInstance().GetAll())
            {
                Accommodation? accommodation = AccommodationService.GetInstance().GetById(renovationRequest.accommodationId);
                User? user = UserService.GetInstance().GetById(accommodation.ownerId);
                if (user.Id == RenovationRequestPage.User.Id)
                {
                    RenovationRequests.Add(renovationRequest);
                }
            }
            RenovationRequestPage.ReservationRequestItems.ItemsSource = RenovationRequests;
        }
    }
}
