using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.Domain.Model.Converters
{
    public class ImagesToImagePaths : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /*List<Image> images = new List<Image>();
            ObservableCollection<string> imagePaths = new ObservableCollection<string>();
            images = (List<Image>)value;
            foreach (Image image in images)
                imagePaths.Add(image.Path);
            return imagePaths;*/
            /*List<Image> images = new List<Image>();
            ObservableCollection<Image> imagePaths = new ObservableCollection<Image>();
            images = (List<Image>)value;
            foreach (Image image in images)
                imagePaths.Add(image);
            return imagePaths;*/
            if (value is List<Image> images)
            {
                List<string> imagePaths = images.Select(image => image.Path).ToList();
                return new ObservableCollection<string>(imagePaths);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
