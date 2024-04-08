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



            _serviceProvider = _services.BuildServiceProvider();

            SignInForm signInForm = new SignInForm();
            signInForm.Show();
        }

    }
}