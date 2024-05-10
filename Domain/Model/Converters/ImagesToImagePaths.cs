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
    public class ImagesToImagePaths : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Image> images = new List<Image>();
            ObservableCollection<string> imagePaths = new ObservableCollection<string>();
            images = (List<Image>)value;
            foreach (Image image in images)
                imagePaths.Add(image.Path);
            return imagePaths;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
