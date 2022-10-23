using Booking.Domain.Responses;
using Booking.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interfaces.Accounts
{
    public interface IAccountService
    {
        Task<BaseResponse<bool>> Register(RegisterViewModel model);

        Task<BaseResponse<bool>> Login(LoginViewModel model);

        Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model);
    }
}
