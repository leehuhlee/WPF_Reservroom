using Reservroom.Exceptions;
using Reservroom.Extensions;
using Reservroom.Services.ReservationConflictValidators;
using Reservroom.Services.ReservationCreators;
using Reservroom.Services.ReservationProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.Models
{
    public class ReservationBook
    {
        private readonly IReservationProvider _reservationsProvider;
        private readonly IReservationCreator _reservationsCreator;
        private readonly IReservationConflictValidator _reservationConflictValidator;

        public ReservationBook(IReservationProvider reservationsProvider, IReservationCreator reservationsCreator, IReservationConflictValidator reservationConflictValidator)
        {
            _reservationsProvider = reservationsProvider;
            _reservationsCreator = reservationsCreator;
            _reservationConflictValidator = reservationConflictValidator;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            return await _reservationsProvider.GetAllReservations();
        }

        public async Task AddReservation(Reservation reservation)
        {
            if(reservation.StartTime > reservation.EndTime)
            {
                throw new InvalidReservationTimeRangeException(reservation);
            }

            Reservation conflictingReservation  = await _reservationConflictValidator.GetConflictingReservation(reservation);

            if(conflictingReservation != null)
            {
                throw new ReservationConflictException(conflictingReservation, reservation);
            }

            await  _reservationsCreator.CreateReservation(reservation);
        }
    }
}
