using BookingApp.Domain.Model;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BookingApp.ViewModel.Tourist
{
    public class ImageViewerViewModel
    {
        public ImageViewer ImageViewer { get; set; }
        System.Windows.Controls.Image Image { get; set; }
        public RelayCommand ClickClose => new RelayCommand(execute => CloseExecute());
        public ImageViewerViewModel(ImageViewer imageViewer, System.Windows.Controls.Image image) 
        {
            ImageViewer = imageViewer;
            Image = image;
            var converter = new ImageSourceConverter();
            ImageViewer.ImageDisplay.Source = image.Source;
        }

        public void CloseExecute()
        {
            ImageViewer.Close();
        }
    }
}
