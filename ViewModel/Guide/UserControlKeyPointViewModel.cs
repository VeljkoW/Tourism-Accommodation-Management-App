using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guide
{
    public class UserControlKeyPointViewModel
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
        public UserControlKeyPointViewModel() { }
        public UserControlKeyPointViewModel(KeyPoint keyPoint)
        {
            Point = keyPoint.Point;
            KeyPoint = new KeyPoint();
            KeyPoint = keyPoint;
        }
    }
}
