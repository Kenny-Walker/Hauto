using Hauto.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hauto.Context
{
    public class HautoContext : DbContext
    {
        public HautoContext(DbContextOptions<HautoContext> options) : base(options)
        {

        }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

    }
}
