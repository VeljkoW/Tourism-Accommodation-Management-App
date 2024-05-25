using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BookingApp.Domain.Model
{
    public class Location : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public Location()
        {
            City = string.Empty;
            State = string.Empty;
        }
        public Location(string city, string state)
        {
            City = city;
            State = state;
        }
        public string Print
        {
            get
            {
                return Id + ": " + State + " - " + City;
            }
            set
            {
                if (value != Print)
                {
                    Print = value;
                    OnPropertyChanged("Print");
                }
            }
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), State, City };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            State = values[1];
            City = values[2];
        }
        public string PrintCities
        {
            get
            {
                return City;
            }
            set
            {
                if (value != PrintCities)
                {
                    PrintCities = value;
                    OnPropertyChanged("PrintCities");
                }
            }
        }
    }
}
