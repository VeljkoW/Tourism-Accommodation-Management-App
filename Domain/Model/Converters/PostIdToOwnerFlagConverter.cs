using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BookingApp.Domain.Model.Converters
{
    public class PostIdToOwnerFlagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int postId = (int)value;
            GuestPost? guestPost = GuestPostService.GetInstance().GetById(postId);
            User? user = UserService.GetInstance().GetById(guestPost.UserId);
            if(user.UserType == UserType.Owner)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
