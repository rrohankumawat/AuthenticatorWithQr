using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Services.Account
{
    public class AccountServices
    {
        private readonly IAccountRepository _accountRepository;
        public AccountServices(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        public async Task<bool> IsValidOtp(string email, string otp)
        {
            try
            {
                return await _accountRepository.IsValidOtp(email, otp);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
