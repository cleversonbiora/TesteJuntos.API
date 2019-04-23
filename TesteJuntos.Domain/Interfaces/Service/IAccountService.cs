using TesteJuntos.Domain.Models;
using TesteJuntos.Domain.Commands.Sample;
using System;
using System.Collections.Generic;
using System.Text;
using TesteJuntos.Domain.ViewModel;
using System.Threading.Tasks;

namespace TesteJuntos.Domain.Interfaces.Service
{
    public interface IAccountService
    {
        Task<object> Login(LoginAccountCommand sample);
        Task<object> Register(RegisterAccountCommand sample);
    }
}
