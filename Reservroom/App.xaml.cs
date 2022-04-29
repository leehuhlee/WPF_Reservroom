using Microsoft.EntityFrameworkCore;
using Reservroom.DbContexts;
using Reservroom.Extensions;
using Reservroom.Models;
using Reservroom.Services;
using Reservroom.Services.ReservationConflictValidators;
using Reservroom.Services.ReservationCreators;
using Reservroom.Services.ReservationProviders;
using Reservroom.Stores;
using Reservroom.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Reservroom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string CONNECTION_STRING = "Data Source=reservroom.db";
        private readonly Hotel _hotel;
        private readonly NavigationStore _navigationStore;
        private readonly ReservroomDbContextFactory _reservroomDbContextFactory;

        public App()
        {
            _reservroomDbContextFactory = new ReservroomDbContextFactory(CONNECTION_STRING);

            IReservationProvider reservationsProvider = new DatabaseReservationProvider(_reservroomDbContextFactory);
            IReservationCreator reservationsCreator = new DatabaseReservationCreator(_reservroomDbContextFactory);
            IReservationConflictValidator reservationConflictValidator = new DatabaseReservationConflictValidator(_reservroomDbContextFactory);

            ReservationBook reservationBook = new ReservationBook(reservationsProvider, reservationsCreator, reservationConflictValidator);

            _hotel = new Hotel("Hotel Del Luna", reservationBook);
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            using (ReservroomDbContext dbContext = _reservroomDbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }

            _navigationStore.CurrentViewModel = CreateReservationViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private MakeReservationViewModel CreateMakeReservationViewModel()
        {
            return new MakeReservationViewModel(_hotel, new NavigationService(_navigationStore, CreateReservationViewModel));
        }

        private ReservationListingViewModel CreateReservationViewModel()
        {
            return ReservationListingViewModel.LoadViewModel(_hotel, new NavigationService(_navigationStore, CreateMakeReservationViewModel));
        }
    }
}
