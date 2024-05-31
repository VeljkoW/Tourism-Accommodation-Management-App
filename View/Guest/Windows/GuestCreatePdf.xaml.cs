using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for GuestCreatePdf.xaml
    /// </summary>
    public partial class GuestCreatePdf : Window
    {
        public int pdfChecked {  get; set; }
        public GuestCreatePdfViewModel GuestCreatePdfViewModel { get; set; }
        public GuestCreatePdf()
        {
            InitializeComponent();
            pdfChecked = 0;
            GuestCreatePdfViewModel = new GuestCreatePdfViewModel(this);
            DataContext = GuestCreatePdfViewModel;
        }

        private void PdfRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton.IsChecked == true)
                {
                    if (radioButton.Name == "PdfRadioButton1")
                        pdfChecked = 1;
                    else if (radioButton.Name == "PdfRadioButton2")
                        pdfChecked = 2;
                }
            }
        }
    }
}
