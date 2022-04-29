using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.DbContexts
{
    public class ReservroomDbContextFactory
    {
        private readonly string _connectionString;

        public ReservroomDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ReservroomDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;

            return new ReservroomDbContext(options);
        }
    }
}
