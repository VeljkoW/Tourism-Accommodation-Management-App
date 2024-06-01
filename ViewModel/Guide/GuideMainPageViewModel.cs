using BookingApp.Repository.TourRepositories;
using BookingApp.Repository;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;
using BookingApp.View.Guide.Pages;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using VirtualKeyboard.Wpf;

namespace BookingApp.ViewModel.Guide
{
    public class GuideMainPageViewModel : INotifyPropertyChanged
    {
        public RelayCommand ResignCommand => new RelayCommand(execute => ResignCommandExecute());

        private void ResignCommandExecute()
        {
            TourService.GetInstance().Resign(user.Id);
            Resigned?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _userName;

        public string UserName
        {
            get => _userName;
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
        private User user;
        public Action Resigned;
        public GuideMainPageViewModel(User user)
        {
            VKeyboard.Listen<System.Windows.Controls.TextBox>(e => e.Text);
            VKeyboard.Config(typeof(KeyboardCustom));
            this.user = user;
            UserName = user.Username;
            SuperGuideService.GetInstance().UpdateSuperGuide(user.Id);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
