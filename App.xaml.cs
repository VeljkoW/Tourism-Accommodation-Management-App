using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using BookingApp.View;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.Repository.AccommodationRepositories;

namespace BookingApp
{
    public partial class App : Application
    {
        public static IServiceCollection _services;
        public static IServiceProvider _serviceProvider;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _services = new ServiceCollection();
            _services.AddSingleton<ImageService>();
            _services.AddSingleton<ImageRepository>();
            _services.AddSingleton<TourRepository>();
            _services.AddSingleton<TourService>();
            _services.AddSingleton<UserRepository>();
            _services.AddSingleton<UserService>();
            _services.AddSingleton<LocationRepository>();
            _services.AddSingleton<LocationService>();
            _services.AddSingleton<CommentRepository>();
            _services.AddSingleton<CommentService>();
            _services.AddSingleton<KeyPointRepository>();
            _services.AddSingleton<KeyPointService>();
            _services.AddSingleton<TourScheduleRepository>();
            _services.AddSingleton<TourScheduleService>();
            _services.AddSingleton<TourReservationRepository>();
            _services.AddSingleton<TourReservationService>();
            _services.AddSingleton<TourPersonRepository>();
            _services.AddSingleton<TourPersonService>();
            _services.AddSingleton<TourImageRepository>();
            _services.AddSingleton<TourImageService>();
            _services.AddSingleton<TourCouponRepository>();
            _services.AddSingleton<TourCouponService>();
            _services.AddSingleton<ReservedAccommodationRepository>();
            _services.AddSingleton<ReservedAccommodationService>();
            _services.AddSingleton<GuestRatingRepository>();
            _services.AddSingleton<GuestRatingService>();
            _services.AddSingleton<AccommodationRepository>();
            _services.AddSingleton<AccommodationService>();
            _services.AddSingleton<GuideComplexService>();
            _services.AddSingleton<TourAttendenceNotificationRepository>();
            _services.AddSingleton<TourAttendenceNotificationService>();


            _serviceProvider = _services.BuildServiceProvider();

            SignInForm signInForm = new SignInForm();
            signInForm.Show();
        }

    }
}