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
using BookingApp.Domain.IRepositories;

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
            _services.AddSingleton<IImageRepository,ImageRepository>();
            _services.AddSingleton<ITourRepository,TourRepository>();
            _services.AddSingleton<TourService>();
            _services.AddSingleton<IUserRepository,UserRepository>();
            _services.AddSingleton<UserService>();
            _services.AddSingleton<ILocationRepository,LocationRepository>();
            _services.AddSingleton<LocationService>();
            _services.AddSingleton<ICommentRepository,CommentRepository>();
            _services.AddSingleton<CommentService>();
            _services.AddSingleton<IKeyPointRepository, KeyPointRepository>();
            _services.AddSingleton<KeyPointService>();
            _services.AddSingleton<ITourScheduleRepository,TourScheduleRepository>();
            _services.AddSingleton<TourScheduleService>();
            _services.AddSingleton<ITourReservationRepository,TourReservationRepository>();
            _services.AddSingleton<TourReservationService>();
            _services.AddSingleton<ITourPersonRepository,TourPersonRepository>();
            _services.AddSingleton<TourPersonService>();
            _services.AddSingleton<ITourImageRepository,TourImageRepository>();
            _services.AddSingleton<TourImageService>();
            _services.AddSingleton<ITourCouponRepository,TourCouponRepository>();
            _services.AddSingleton<TourCouponService>();
            _services.AddSingleton<IReservedAccommodationRepository,ReservedAccommodationRepository>();
            _services.AddSingleton<ReservedAccommodationService>();
            _services.AddSingleton<IGuestRatingRepository,GuestRatingRepository>();
            _services.AddSingleton<GuestRatingService>();
            _services.AddSingleton<IAccommodationRepository,AccommodationRepository>();
            _services.AddSingleton<AccommodationService>();
            _services.AddSingleton<ITourAttendenceNotificationRepository,TourAttendenceNotificationRepository>();
            _services.AddSingleton<TourAttendenceNotificationService>();
            _services.AddSingleton<IGuestReschedulingRequestRepository,GuestReschedulingRequestRepository>();
            _services.AddSingleton<GuestReschedulingRequestService>();
            _services.AddSingleton<ITourReviewRepository,TourReviewRepository>();
            _services.AddSingleton<TourReviewService>();
            _services.AddSingleton<ITourReviewImageRepository,TourReviewImageRepository>();
            _services.AddSingleton<TourReviewImageService>();
            _services.AddSingleton<IOwnerRatingRepository, OwnerRatingRepository>();
            _services.AddSingleton<OwnerRatingService>();
            _services.AddSingleton<IProcessedReschedulingRequestRepository,ProcessedReschedulingRequestRepository>();
            _services.AddSingleton<ProcessedReschedulingRequestService>();
            _services.AddSingleton<IOwnerRepository,OwnerRepository>();
            _services.AddSingleton<OwnerService>();
            _services.AddSingleton<ITourSuggestionRepository,TourSuggestionRepository>();
            _services.AddSingleton<TourSuggestionService>();
            _services.AddSingleton<IRenovationRequestRepository, RenovationRequestRepository>();
            _services.AddSingleton<RenovationRequestService>();
            _services.AddSingleton<IGuestBonus, GuestBonusRepository>();
            _services.AddSingleton<GuestBonusService>();

            _serviceProvider = _services.BuildServiceProvider();

            SignInForm signInForm = new SignInForm();
            signInForm.Show();
        }

    }
}