using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
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
    public class Forum : ISerializable, INotifyPropertyChanged
    {
        private int id { get; set; }
        private int locationId { get; set; }
        private List<GuestPost> guestPosts { get; set; }
        private bool isValid { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public Forum() 
        { 
            guestPosts = new List<GuestPost>();
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
        public int LocationId
        {
            get
            {
                return locationId;
            }
            set
            {
                if (value != locationId)
                {
                    locationId = value;
                    OnPropertyChanged(nameof(locationId));
                }
            }
        }
        public bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                if (value != isValid)
                {
                    isValid = value;
                    OnPropertyChanged(nameof(isValid));
                }
            }
        }
        public List<GuestPost> GuestPosts
        {
            get
            {
                return guestPosts;
            }
            set
            {
                if (value != guestPosts)
                {
                    guestPosts = value;
                    OnPropertyChanged(nameof(guestPosts));
                }
            }
        }
        public string GuestPostIdToCSV()
        {
            string str = "";
            foreach (GuestPost g in GuestPosts)
            {
                str += g.Id + ",";
            }
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return str;
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                    Id.ToString(),
                    LocationId.ToString(),
                    IsValid.ToString(),
                    GuestPostIdToCSV()
                };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            LocationId = Convert.ToInt32(values[1]);
            IsValid = Convert.ToBoolean(values[2]);
            if (values[3].Length > 0)
            {
                string[] PostsIds = values[3].Split(',');
                for (int i = 0; i < PostsIds.Length; i++)
                {
                    GuestPost guestPost = new GuestPost();
                    GuestPostRepository guestPostRepository = new GuestPostRepository();
                    guestPost = guestPostRepository.GetById(Convert.ToInt32(PostsIds[i]));
                    GuestPosts.Add(guestPost);
                }
            }
        }
    }
}
