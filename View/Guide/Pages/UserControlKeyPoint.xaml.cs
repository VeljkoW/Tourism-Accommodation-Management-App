using BookingApp.Model;
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
        public KeyPoint KeyPoint { get; set; }

        private string _point;
        public string Point
        {
            get => _point;
            set
            {
                if (value != _point)
                {
                    _point = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public UserControlKeyPoint(KeyPoint keyPoint)
        {
            InitializeComponent();
            DataContext = this;
            Point = keyPoint.Point;
        }

        private void VisitKeyPoint(object sender, RoutedEventArgs e)
        {

        }
    }
}
