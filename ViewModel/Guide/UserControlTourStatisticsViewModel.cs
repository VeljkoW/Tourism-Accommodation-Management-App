using BookingApp.Domain.Model;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guide
{
    public class UserControlTourStatisticsViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public UserControlTourStatisticsViewModel(User user, Tour tour,TourStatistics tourStatistics)
        {
            TourName = tour.Name;
            Elderly= tourStatistics.Elderly.ToString();
            Adults = tourStatistics.Adult.ToString();
            Underage = tourStatistics.Underage.ToString();
            Visitors = tourStatistics.Visitors.ToString();
            List<int> tourImages = TourImageService.GetInstance().GetAll().Where(t => t.TourId==tour.Id).Select(t => t.ImageId).ToList();
            List<Image> images = ImageService.GetInstance().GetAll().Where(t=> tourImages.Contains(t.Id)).ToList();
            ImgPath = images[0].Path;
        }
        public UserControlTourStatisticsViewModel()
        {
        }

        private string _tourName;
        public string TourName
        {
            get => _tourName;
            set
            {
                if (value != _tourName)
                {
                    _tourName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _elderly;
        public string Elderly
        {
            get => string.Format("Elderly: {0}",_elderly.ToString());
            set
            {
                if (value != _elderly)
                {
                    _elderly = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _adults;
        public string Adults
        {
            get => string.Format("Adults: {0}", _adults.ToString());
            set
            {
                if (value != _adults)
                {
                    _adults = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _underage;
        public string Underage
        {
            get => string.Format("Underage: {0}", _underage.ToString());
            set
            {
                if (value != _underage)
                {
                    _underage = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _visitors;
        public string Visitors
        {
            get => string.Format("Visitors: {0}", _visitors.ToString());
            set
            {
                if (value != _visitors)
                {
                    _visitors = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _imgpath;
        public string ImgPath
        {
            get => _imgpath;
            set
            {
                if (value != _imgpath)
                {
                    _imgpath = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
