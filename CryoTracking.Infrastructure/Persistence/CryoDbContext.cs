using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;    
using CryoTracking.Domain.Entities; 

namespace CryoTracking.Infrastructure.Persistence
{
   public class CryoDbContext : DbContext
    {
        public CryoDbContext(DbContextOptions<CryoDbContext> options) : base(options)
        {
        }
        public DbSet<Patient> Patients { get; set; }
    
    }
}
