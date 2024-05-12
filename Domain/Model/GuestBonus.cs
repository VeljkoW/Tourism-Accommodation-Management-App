using System;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.Domain.Model
{
    public class GuestBonus : ISerializable, INotifyPropertyChanged
    {
        private int id { get; set; }
        private int guestId { get; set; }
        private bool isSuperGuest { get; set; }
        private DateTime startSuperGuest { get; set; }
        private int bonus { get; set; }

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
        public bool IsSuperGuest
        {
            get
            {
                return isSuperGuest;
            }
            set
            {
                if (value != isSuperGuest)
                {
                    isSuperGuest = value;
                    OnPropertyChanged(nameof(isSuperGuest));
                }
            }
        }

        public DateTime StartSuperGuest
        {
            get
            {
                return startSuperGuest;
            }
            set
            {
                if (value != startSuperGuest)
                {
                    startSuperGuest = value;
                    OnPropertyChanged(nameof(startSuperGuest));
                }
            }
        }
        public int GuestId
        {
            get
            {
                return guestId;
            }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged(nameof(guestId));
                }
            }
        }
        public int Bonus
        {
            get
            {
                return bonus;
            }
            set
            {
                if (value != bonus)
                {
                    bonus = value;
                    OnPropertyChanged(nameof(bonus));
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(), 
                GuestId.ToString(),
                IsSuperGuest.ToString(),
                StartSuperGuest.ToString(),
                Bonus.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            IsSuperGuest = Convert.ToBoolean(values[2]);
            StartSuperGuest = Convert.ToDateTime(values[3]);
            Bonus = Convert.ToInt32(values[4]);
        }
    }
}
