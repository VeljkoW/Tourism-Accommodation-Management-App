using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for UserControlComplexTourSuggestionListing.xaml
    /// </summary>
    public partial class UserControlComplexTourSuggestionListing : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _header = "";

        public string ComplexTourHeader
        {
            get => String.Format("Complex tour #{0}",_header);
            set
            {
                if (value != _header)
                {
                    _header = value;
                    OnPropertyChanged();
                }
            }
        }
        private ComplexTourRequestToursPage complexTourRequestToursPage;
        public ComplexTourRequestsPage complexTourRequestsPage;
        private int id;
        public UserControlComplexTourSuggestionListing(ComplexTourRequestsPage complexTourRequestsPage, int id)
        {
            InitializeComponent();
            ComplexTourHeader = id.ToString();
            DataContext = this;
            this.id = id;
            this.complexTourRequestsPage = complexTourRequestsPage;
        }
        private void ShowComplexSuggestions(object sender, RoutedEventArgs e)
        {
            complexTourRequestToursPage = new ComplexTourRequestToursPage(this,complexTourRequestsPage.NavigationService, id);
            complexTourRequestsPage.NavigationService.Navigate(complexTourRequestToursPage);
        }
        public void LoadTours()
        {
            complexTourRequestsPage.Load();
        }
    }
}
