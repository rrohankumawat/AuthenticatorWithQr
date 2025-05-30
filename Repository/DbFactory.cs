using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class DbFactory : IDisposable
    {
        private readonly Func<ApplicationDbContext> _instanceFunc;
        private DbContext _dbContext;
        public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());


        public DbFactory(Func<ApplicationDbContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            //if (!_disposed && _dbContext != null)
            //{
            //    _disposed = true;
            //    _dbContext.Dispose();
            //}
        }
        public ApplicationDbContext DbContextObj()
        {
            var context = _instanceFunc();
            return context;
        }
    }
}
