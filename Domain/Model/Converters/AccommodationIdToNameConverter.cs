using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
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
    public class AccommodationIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int accommodationId = (int)value;
            Accommodation? accommodation = new Accommodation();
            accommodation = AccommodationService.GetInstance().GetById(accommodationId);
            return accommodation.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
