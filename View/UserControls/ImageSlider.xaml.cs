using BookingApp.ViewModel;
using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.UserControls
{
    /// <summary>
    /// Interaction logic for ImageSlider.xaml
    /// </summary>
    public partial class ImageSlider : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ImagePathsProperty =
            DependencyProperty.Register("ImagePaths", typeof(ObservableCollection<string>), typeof(ImageSlider), new PropertyMetadata(null));

        private int currentImageIndex = 0;
        //public ObservableCollection<string> ImagePaths { get; set; }
        public string CurrentImagePath => ImagePaths?.ElementAtOrDefault(CurrentImageIndex);
        public int TotalImages => ImagePaths?.Count ?? 0;
        public RelayCommand PreviousImageCommand => new RelayCommand(execute => PreviousImage(), canExecute => CanPreviousImage());
        public RelayCommand NextImageCommand => new RelayCommand(execute => NextImage(), canExecute => CanNextImage());
        public ObservableCollection<string> ImagePaths
        {
            get { return (ObservableCollection<string>)GetValue(ImagePathsProperty); }
            set { SetValue(ImagePathsProperty, value); }
        }
        public ObservableCollection<Image> Images { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public ImageSlider()
        {
            InitializeComponent();
        }
        public int CurrentImageIndex
        {
            get { return currentImageIndex; }
            set
            {
                if (value >= 0 && value < ImagePaths.Count)
                {
                    currentImageIndex = value;
                    OnPropertyChanged(nameof(CurrentImageIndex));
                }
            }
        }
        public void NextImage()
        {
            if (CurrentImageIndex < TotalImages - 1)
            {
                CurrentImageIndex++;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        public bool CanNextImage()
        {
            if (CurrentImageIndex == TotalImages - 1 || TotalImages == 0)
                return false;
            return true;
        }
        public void PreviousImage()
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        public bool CanPreviousImage()
        {
            if (CurrentImageIndex == 0 || TotalImages == 0)
                return false;
            return true;
        }
    }
}
