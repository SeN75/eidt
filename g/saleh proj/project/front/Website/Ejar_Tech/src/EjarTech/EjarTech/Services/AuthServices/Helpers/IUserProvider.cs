using EjarTech.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.Services.AuthServices.Helpers
{
    public interface IUserProvider
    {
        Task<User> GetUserAsync();
    }
}
