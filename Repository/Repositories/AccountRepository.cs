using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OtpNet;
using Shared.Entities;

namespace Repository.Repositories
{
    public class AccountRepository : Repositoryy<object>, IAccountRepository
    {
        public AccountRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }


        public async Task<bool> IsValidOtp(string email, string otp)
        {
            try
            {
                var userDetails = await this.DbContextObj().Users.FirstOrDefaultAsync(x=>x!.Email!.ToLower() == email.ToLower());

                string secretKey = userDetails!.AuthenticatorKey;

                var totp = new Totp(Base32Encoding.ToBytes(secretKey));
                string currentCode = totp.ComputeTotp();
                if(currentCode == otp)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}
