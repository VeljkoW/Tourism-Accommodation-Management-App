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
    public class RenovationHistoryViewModel
    {
        public User User { get; set; }
        public RenovationHistory RenovationHistory { get; set; }
        public ScheduledRenovation SelectedScheduledRenovation { get; set; }
        public ObservableCollection<ScheduledRenovation> ScheduledRenovations { get; set; }
        public RenovationHistoryViewModel(RenovationHistory renovationHistory)
        {
            User = renovationHistory.User;
            RenovationHistory = renovationHistory;
            ScheduledRenovations = new ObservableCollection<ScheduledRenovation>();
            ScheduledRenovationService.GetInstance().UpdatePreviousRenovations(User, ScheduledRenovations);
        }
    }
}
