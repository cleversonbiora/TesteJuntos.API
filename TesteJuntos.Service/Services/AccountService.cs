using FluentValidation;
using TesteJuntos.Domain.Interfaces.Service;
using TesteJuntos.Domain.Models;
using TesteJuntos.Service.Validators;
using TesteJuntos.Domain.Commands.Sample;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using TesteJuntos.Domain.ViewModel;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TesteJuntos.CrossCutting.Utils;
using TesteJuntos.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using FluentValidation.Results;
//https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/identity-custom-storage-providers?view=aspnetcore-2.2
namespace TesteJuntos.Service.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountService(IMapper mapper, UserManager<ApiUser> userManager, SignInManager<ApiUser> signInManager, IEmailSender emailSender)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<object> Register(RegisterAccountCommand login)
        {
            var user = new ApiUser { UserName = login.Email, Email = login.Email, FirstName = login.FirstName, LastName = login.LastName };
            var result = await _userManager.CreateAsync(user, login.Password);
            if (!result.Succeeded)
                throw new ValidationException("Falha ao registrar usuário", result.Errors.Select(x => new ValidationFailure(x.Code,x.Description)));
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _signInManager.SignInAsync(user, isPersistent: false);
            var claims = new[]
    {
                    new Claim(ClaimTypes.Name, login.Email)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new Sha512(Environment.GetEnvironmentVariable("Password")).ToString()));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("Issuer"),
                audience: Environment.GetEnvironmentVariable("Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return new { token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
        
        public async Task<object> Login(LoginAccountCommand login)
        {
            Validate(login, new LoginAccountValidator());

            var result = await _signInManager.PasswordSignInAsync(login.User, login.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var claims = new[]
                    {
                    new Claim(ClaimTypes.Name, login.User)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new Sha512(Environment.GetEnvironmentVariable("Password")).ToString()));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: Environment.GetEnvironmentVariable("Issuer"),
                    audience: Environment.GetEnvironmentVariable("Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);
                return new { token = new JwtSecurityTokenHandler().WriteToken(token) };
            }
            if (result.IsLockedOut)
            {
                throw new ArgumentException("User account locked out.");
            }
            else
            {
                throw new ArgumentException("Invalid login attempt.");
            }
        }
    }
}
