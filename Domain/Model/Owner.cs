using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using BookingApp.Serializer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Domain.Model
{
    public class Owner : ISerializable, INotifyPropertyChanged
    {
        public int id { get; set; }
        public bool isSuperOwner { get; set; }
        public Owner() { }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged(nameof(id));
                }
            }
        }
        public bool IsSuperOwner
        {
            get
            {
                return isSuperOwner;
            }
            set
            {
                if (value != isSuperOwner)
                {
                    isSuperOwner = value;
                    OnPropertyChanged(nameof(isSuperOwner));
                }
            }
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                IsSuperOwner.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            IsSuperOwner = Convert.ToBoolean(values[1]);
        }
    }
}
