using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guide;
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
    /// Interaction logic for UserControlKeyPoint.xaml
    /// </summary>
    public partial class UserControlKeyPoint : UserControl
    {
        UserControlKeyPointViewModel UserControlKeyPointViewModel { get; set; }
        public UserControlKeyPoint(KeyPoint keyPoint)
        {
            InitializeComponent();
            UserControlKeyPointViewModel = new UserControlKeyPointViewModel(keyPoint);
            DataContext = UserControlKeyPointViewModel;
        }
        public EventHandler<KeyPoint> EventHandler { get; set; }

        private void handleEvent()
        {
            EventHandler?.Invoke(this, UserControlKeyPointViewModel.KeyPoint);
        }

        private void ClickVisitKeyPoint(object sender, RoutedEventArgs e)
        {
            handleEvent();
            OnClickedVisitTour?.Invoke(sender, e);
        }
        public EventHandler OnClickedVisitTour { get;set; }
    }
}