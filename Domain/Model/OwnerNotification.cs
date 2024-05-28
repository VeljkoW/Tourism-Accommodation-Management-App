using BookingApp.Domain.IRepositories;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Serializer;
using BookingApp.Services;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class OwnerNotification : ISerializable, INotifyPropertyChanged
    {
        private int id {  get; set; }
        private string root {  get; set; }
        private int reservedAccommodationId { get; set; }
        private int forumId {  get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public OwnerNotification() 
        {
            reservedAccommodationId = 0;
            forumId = 0;
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
        public int ReservedAccommodationId
        {
            get
            {
                return reservedAccommodationId;
            }
            set
            {
                if (value != reservedAccommodationId)
                {
                    reservedAccommodationId = value;
                    OnPropertyChanged(nameof(reservedAccommodationId));
                }
            }
        }
        public int ForumId
        {
            get
            {
                return forumId;
            }
            set
            {
                if (value != forumId)
                {
                    forumId = value;
                    OnPropertyChanged(nameof(forumId));
                }
            }
        }
        public string Root
        {
            get
            {
                return root;
            }
            set
            {
                if (value != root)
                {
                    root = value;
                    OnPropertyChanged(nameof(root));
                }
            }
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                    Id.ToString(),
                    Root,
                    ReservedAccommodationId.ToString(),
                    ForumId.ToString()
                };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Root = values[1];
            ReservedAccommodationId = Convert.ToInt32(values[2]);
            ForumId = Convert.ToInt32(values[3]);
        }
        public string Print
        {
            get
            {
                const string SRB = "sr-RS";
                const string ENG = "en-US";
                if(Root == "Forum")
                {
                    Forum forum = ForumService.GetInstance().GetById(ForumId);
                    Location location = LocationService.GetInstance().GetById(forum.LocationId);
                    if (App.currentLanguage() == ENG)
                    {
                        return "New forum is open on location: " + location.State + " - " + location.City;
                    }
                    else
                    {
                        return "Novi forum je otvoren na lokaciji: " + location.State + " - " + location.City;
                    }
                }
                else if(Root == "OwnerRating")
                {
                    ReservedAccommodation? reservedAccommodation = ReservedAccommodationService.GetInstance().GetById(ReservedAccommodationId);
                    User? user = UserService.GetInstance().GetById(reservedAccommodation.GuestId);
                    if (App.currentLanguage() == ENG)
                    {
                        return "Remaining " + (5 - (DateTime.Now - reservedAccommodation.CheckOutDate).Days) + " days to rate the user: " + user.Username;
                    }
                    else
                    {
                        return "Preostalo " + (5 - (DateTime.Now - reservedAccommodation.CheckOutDate).Days) + " dana da ocenite korisnika: " + user.Username;
                    }
                }
                else
                {
                    return "WHATEVER";
                }
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
