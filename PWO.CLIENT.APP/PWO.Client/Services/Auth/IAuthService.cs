using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWO.Client.Services.Auth
{
    public interface IAuthService
    {

        Task<bool> LoginAsync(string login, string password, string token);
    }
}
