using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class Repositoryy<T> : IRepository<T> where T : class
    {
        private readonly DbFactory _dbFactory;

        private DbSet<T> _dbSet;

        protected DbSet<T> DbSet
        {
            get => _dbSet ?? (_dbSet = _dbFactory.DbContext.Set<T>());
        }

        public Repositoryy(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        
        public ApplicationDbContext DbContextObj()
        {
            return _dbFactory.DbContextObj();

        }
    }
}
