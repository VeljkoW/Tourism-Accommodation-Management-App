using BookingApp.View.Guest.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guest
{
    public class GuestCreatePdfViewModel
    {
        public GuestCreatePdf GuestCreatePdf { get; set; }
        public RelayCommand createPdf => new RelayCommand(execute => CreatePdf(), canExecute => CanCreatePdf());
        public GuestCreatePdfViewModel(GuestCreatePdf guestCreatePdf)
        {
            GuestCreatePdf = guestCreatePdf;
        }

        public bool CanCreatePdf()
        {
            if(string.IsNullOrEmpty(GuestCreatePdf.startDatePicker.Text) || string.IsNullOrEmpty(GuestCreatePdf.endOutDatePicker.Text) ||
                GuestCreatePdf.pdfChecked == 0)
                return false;
            return true;
        }

        public void CreatePdf()
        {
            MessageBox.Show("CAOO");
        }
    }
}
