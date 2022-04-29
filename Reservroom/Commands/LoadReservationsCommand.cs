using Reservroom.Models;
using Reservroom.Services;
using Reservroom.Stores;
using Reservroom.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Reservroom.Commands
{
    public class LoadReservationsCommand : AsyncCommandBase
    {
        private readonly ReservationListingViewModel _viewModel;
        private readonly HotelStore _hotelStore;

        public LoadReservationsCommand(ReservationListingViewModel viewModel, HotelStore hotelStore)
        {
            _hotelStore = hotelStore;
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _hotelStore.Load();
                _viewModel.UpdateReservations(_hotelStore.Reservations);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to load reservations.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
