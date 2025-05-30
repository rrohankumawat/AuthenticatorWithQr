using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;

namespace Repository.Repositories
{
    public class AccountRepository : Repositoryy<object>, IAccountRepository
    {
        public AccountRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
