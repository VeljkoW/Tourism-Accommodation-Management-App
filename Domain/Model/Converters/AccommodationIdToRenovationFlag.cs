using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.Domain.Model.Converters
{
    public class AccommodationIdToRenovationFlag : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int accommodationId = (int)value;
            Accommodation accommodation = AccommodationService.GetInstance().GetById(accommodationId);
            User user = UserService.GetInstance().GetById(accommodation.OwnerId);
            ObservableCollection<ScheduledRenovation> ScheduledRenovations = new ObservableCollection<ScheduledRenovation>();
            ScheduledRenovationService.GetInstance().UpdateUpcomingRenovations(user, ScheduledRenovations);
            foreach(ScheduledRenovation scheduledRenovation in ScheduledRenovations)
                if (scheduledRenovation.AccommodationId == accommodationId)
                    return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
