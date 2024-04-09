using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Domain.Model
{
    public class Image : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public Image() { }
        public Image(int id, string path)
        {
            Id = id;
            Path = path;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Path.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Path = Convert.ToString(values[1]);
        }
        public string Print
        {
            get
            {
                return Path;
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
    }
}