using System.Linq;
using ScalableWeb.Domain.Models;
using ScalableWeb.Domain.Patterns;

namespace ScalableWeb.Repository
{
    public class DataRepository : IRepository<DataRecord>
    {
        private readonly DataContext _dbContext;

        public DataRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Insert(DataRecord entity)
        {
            _dbContext.DataRecords.Add(entity);
            _dbContext.SaveChanges();
        }

        public IQueryable<DataRecord> Queriable()
        {
            return _dbContext.DataRecords;
        }
    }
}