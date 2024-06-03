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
    public class BoolIsSpecialToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int Id = (int)value;
            GuestPost guestPost = GuestPostService.GetInstance().GetById(Id);
            User user = UserService.GetInstance().GetById(guestPost.UserId);
            if (guestPost.SpecialUser == true)
            {
                return "Special Guest";
            }
            else
            {
                if(user.UserType == UserType.Guest)
                    return "Reports: " + guestPost.Reports.ToString();
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
