using Microsoft.EntityFrameworkCore;
using Reservroom.DTO;
using Reservroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.DbContexts
{
    public class ReservroomDbContext : DbContext
    {
        public ReservroomDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ReservationDTO> Reservations { get; set; }
    }
}
