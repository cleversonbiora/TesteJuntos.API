using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TesteJuntos.Domain.Interfaces.Service;
using TesteJuntos.Domain.Models;
using FluentValidation;
using TesteJuntos.Domain.Commands.Sample;
using TesteJuntos.CrossCutting.Attributes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TesteJuntos.Application.Controllers
{
    [Route("api/[Controller]")]
    [EnableCors("Cors")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService) => _accountService = accountService;
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginAccountCommand login)
        {
            return Response(await _accountService.Login(login));
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterAccountCommand register)
        {
            return Response(await _accountService.Register(register));
        }
    }
}
