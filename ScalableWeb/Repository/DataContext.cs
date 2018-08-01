using Microsoft.EntityFrameworkCore;
using ScalableWeb.Domain.Models;

namespace ScalableWeb.Repository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<DataRecord> DataRecords { get; set; }
    }
}