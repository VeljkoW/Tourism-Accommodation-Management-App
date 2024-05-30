using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.Domain.Model.Converters
{
    public class UserIdToGuestOrOwner : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int userId = (int)value;
            User? user = new User();
            user = UserService.GetInstance().GetById(userId);
            if (user.UserType == UserType.Owner)
                return "Owner:";
            else if(user.UserType == UserType.Guest)
                return "Guest:";

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
