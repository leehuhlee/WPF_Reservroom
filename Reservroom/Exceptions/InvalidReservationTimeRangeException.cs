using Reservroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.Exceptions
{
    public class InvalidReservationTimeRangeException : Exception
    {
        public Reservation Reservation { get; }

        public InvalidReservationTimeRangeException(Reservation reservation)
        {
            Reservation = reservation;
        }
    }
}
