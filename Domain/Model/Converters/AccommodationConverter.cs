using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookingApp.Model.Converters
{
    public class AccommodationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int accommodationId = (int)value;
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            Accommodation? accommodation = new Accommodation();
            accommodation = accommodationRepository.GetById(accommodationId);
            return accommodation;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
