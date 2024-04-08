using BookingApp.Domain.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.Domain.Model.Converters
{
    public class GuestIdToUsernameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int userId = (int)value;
            UserRepository userRepository = new UserRepository();
            User user = new User();
            user = userRepository.GetById(userId);
            return user.Username; // Vratite odgovarajuću vrednost korisničkog imena
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
