﻿using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BookingApp.ViewModel.Tourist
{
    public class TourFinishedDetailedViewModel
    {
        public TourFinishedDetailed TourFinishedDetailed { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }
        public TourFinishedDetailedViewModel(TourFinishedDetailed tourFinishedDetailed,Tour selectedTour,User user) 
        { 
            this.TourFinishedDetailed = tourFinishedDetailed;
            Tour = selectedTour;
            User = user;
            TourFinishedDetailed.NameTextBlock.Text = Tour.Name;
            TourFinishedDetailed.DescriptionTextBlock.Text = Tour.Description;

            if (Tour.Images != null && Tour.Images.Count > 0)
            {
                Image Image = Tour.Images[0];
                var converter = new ImageSourceConverter();
                TourFinishedDetailed.ImageBox.Source = (ImageSource)converter.ConvertFromString(Image.Path);
            }
            if (Tour.Location != null)
            {
                TourFinishedDetailed.StateTextBlock.Text = Tour.Location.State;
                if (!String.IsNullOrEmpty(Tour.Location.City))
                {
                    TourFinishedDetailed.CityTextBlock.Text = ", " + Tour.Location.City;
                }
                else
                {
                    TourFinishedDetailed.CityTextBlock.Text = Tour.Location.City;
                }
            }

            TourFinishedDetailed.LanguageTextBlock.Text = Tour.Language;
            TourFinishedDetailed.DateTextBlock.Text = Tour.DateTime.Date.ToString();

            TourFinishedDetailed.DurationTextBlock.Text = Tour.Duration.ToString() + "h";

            TourFinishedDetailed.MaxPeopleTextBlock.Text = Tour.MaxTourists.ToString();
        }
        public void OpenReviewWindow(object sender, RoutedEventArgs e)
        {
            bool rated = false;
            foreach(TourReview tourReview in TourReviewService.GetInstance().GetAll())
            {
                TourSchedule tourschedule = TourScheduleService.GetInstance().GetById(tourReview.TourScheduleId);
                if(tourschedule.TourId == Tour.Id && tourschedule.Date == Tour.DateTime) 
                {
                    rated = true;
                }
            }
            if (!rated)
            {
                TourFinishedDetailed.Close();
                TourReviewWindow tourReviewWindow = new TourReviewWindow(Tour, User);
                tourReviewWindow.Show();
            }
            else
            {
                MessageBox.Show("Tour already rated");
            }
        }
        public void GoBack(object sender, RoutedEventArgs e)
        {
            TourFinishedDetailed.Close();
        }
    }
}
