﻿using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class NotificationWindowViewModel
    {
        public NotificationWindow NotificationWindow { get; set; }
        public ObservableCollection<TourAttendenceNotification> TourAttendenceNotifications { get; set; }
        public ObservableCollection<TourSuggestionNotification> TourSuggestionNotifications { get; set; }
        public ObservableCollection<TourNotification> TourLanguageNotifications {  get; set; }
        public ObservableCollection<TourNotification> TourLocationNotifications { get; set; }
        public User User { get; set; }
        public RelayCommand ClickClose => new RelayCommand(execute => CloseExecute());
        public RelayCommand ClickReadAll => new RelayCommand(execute => ReadAllExecute());
        public NotificationWindowViewModel(NotificationWindow notificationWindow,User user) 
        { 
            this.NotificationWindow = notificationWindow;
            this.User = user;
            this.TourAttendenceNotifications = new ObservableCollection<TourAttendenceNotification>();
            this.TourSuggestionNotifications = new ObservableCollection<TourSuggestionNotification>();
            this.TourLanguageNotifications = new ObservableCollection<TourNotification>();
            this.TourLocationNotifications = new ObservableCollection<TourNotification>();
            Update();
        }
        public void Update()
        {
            UpdateTourAttencenceNotifications();
            UpdateTourSuggestionNotifications();
            UpdateTourLanguageNotifications();
            UpdateTourLocationNotifications();
        }
        public void UpdateTourAttencenceNotifications()
        {
            TourAttendenceNotifications.Clear();
            foreach (TourAttendenceNotification tr in TourAttendenceNotificationService.GetInstance().GetAll())
            {
                if (tr.ConfirmedAttendence == false && User.Id == tr.UserId)
                {
                    TourPerson? tourPerson = TourPersonService.GetInstance().GetById(tr.TourPersonId);

                    if (tourPerson != null)
                    {
                        tr.TourPersonNameSurname = tourPerson.Name + " " + tourPerson.Surname;
                        KeyPoint? keypoint = KeyPointService.GetInstance().GetById(tourPerson.KeyPointId);
                        if (keypoint != null)
                        {
                            tr.KeypointName = keypoint.Point;
                        }
                        else
                        {
                            tr.KeypointName = "No keypoint name error";
                        }
                    }
                    else
                    {
                        tr.TourPersonNameSurname = "No name error";
                        tr.KeypointName = "No keypoint name error";
                    }
                    int TourScheduleid = -1;
                    foreach (TourReservation tourReservation in TourReservationService.GetInstance().GetAll())
                    {
                        foreach (TourPerson tourPerson1 in tourReservation.People)
                        {
                            if (tourPerson1.Id == tr.TourPersonId)
                            {
                                TourScheduleid = tourReservation.TourScheduleId;
                                break;
                            }
                        }
                    }
                    if (TourScheduleid != -1)
                    {
                        foreach (TourSchedule tourSchedule in TourScheduleService.GetInstance().GetAll())
                        {
                            if (tourSchedule.Id == TourScheduleid)
                            {
                                Tour? tour = TourService.GetInstance().GetById(tourSchedule.TourId);
                                if (tour != null)
                                {
                                    tr.TourName = tour.Name;
                                }
                                else
                                {
                                    tr.TourName = "No TourName error";
                                }
                            }
                        }
                    }
                    else
                    {
                        tr.TourName = "No TourName error";
                    }


                    TourAttendenceNotifications.Add(tr);
                }
            }
            TourAttendenceNotifications = new ObservableCollection<TourAttendenceNotification>(TourAttendenceNotifications.Reverse());
        }
        public void UpdateTourSuggestionNotifications()
        {
            TourSuggestionNotifications.Clear();
            foreach(var notif in TourSuggestionNotificationService.GetInstance().GetAllUnread(User.Id))
            {
                TourSuggestionNotifications.Add(notif);
            }
        }
        public void UpdateTourLanguageNotifications()
        {
            TourLanguageNotifications.Clear();
            foreach(var notif in TourNotificationService.GetInstance().GetAllLanguage(User.Id))
            {
                TourLanguageNotifications.Add(notif);
            }
        }
        public void UpdateTourLocationNotifications()
        {
            TourLocationNotifications.Clear();
            foreach(var notif in TourNotificationService.GetInstance().GetAllLocation(User.Id))
            {
                TourLocationNotifications.Add(notif);
            }
        }
        public void CloseExecute()
        {
            NotificationWindow.Close();
        }
        public void ReadAllExecute()
        {
            foreach(var notif in TourAttendenceNotifications)
            {
                notif.ConfirmedAttendence = true;
                TourAttendenceNotificationService.GetInstance().Update(notif);
            }
            foreach(var notif in TourSuggestionNotifications)
            {
                notif.NotificationStatus = NotificationStatus.Read;
                TourSuggestionNotificationService.GetInstance().Update(notif);
            }
            foreach(var notif in TourLanguageNotifications)
            {
                notif.Status = NotificationStatus.Read;
                TourNotificationService.GetInstance().Update(notif);
            }
            foreach(var notif in TourLocationNotifications)
            {
                notif.Status = NotificationStatus.Read;
                TourNotificationService.GetInstance().Update(notif);
            }
            Update();
        }
    }
}
