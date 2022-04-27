using Reservroom.Extensions;
using Reservroom.Models;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            Hotel hotel = new Hotel("Hotel Del Luna");

            try
            {
                hotel.MakeReservation(new Reservation(
                    new RoomID(1, 3),
                    "IU",
                    new DateTime(2000, 1, 1),
                    new DateTime(2000, 1, 2)));

                hotel.MakeReservation(new Reservation(
                    new RoomID(1, 4),
                    "PO",
                    new DateTime(2000, 1, 1),
                    new DateTime(2000, 1, 4)));

                IEnumerable<Reservation> reservations = hotel.GetAllReservations();
            }
            catch (ReservationConflictException ex)
            {

            }

            base.OnStartup(e);
        }
    }
}
