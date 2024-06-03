using BookingApp.Repository;
using BookingApp.Serializer;
using BookingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BookingApp.Domain.Model
{
    public class GuestPost : ISerializable, INotifyPropertyChanged
    { 
        private int id {  get; set; }
        private int forumId {  get; set; }
        private int userId { get; set; }
        private string comment { get; set; }
        private bool specialUser { get; set; }
        private int reports {  get; set; }
        public GuestPost()
        {
            reports = 0;
        }

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
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                if (value != userId)
                {
                    userId = value;
                    OnPropertyChanged(nameof(userId));
                }
            }
        }
        public bool SpecialUser
        {
            get
            {
                return specialUser;
            }
            set
            {
                if (value != specialUser)
                {
                    specialUser = value;
                    OnPropertyChanged(nameof(specialUser));
                }
            }
        }
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged(nameof(comment));
                }
            }
        }
        public int Reports
        {
            get
            {
                return reports;
            }
            set
            {
                if (value != reports)
                {
                    reports = value;
                    OnPropertyChanged(nameof(reports));
                }
            }
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                    Id.ToString(),
                    ForumId.ToString(),
                    UserId.ToString(),
                    Comment,
                    SpecialUser.ToString(),
                    Reports.ToString()
                };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ForumId = Convert.ToInt32(values[1]);
            UserId = Convert.ToInt32(values[2]);
            Comment = values[3];
            SpecialUser = Convert.ToBoolean(values[4]);
            Reports = Convert.ToInt32(values[5]);
        }
    }
}
