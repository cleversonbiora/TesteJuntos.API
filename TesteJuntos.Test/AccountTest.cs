using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TesteJuntos.Application.Controllers;
using TesteJuntos.Domain.Commands.Sample;
using TesteJuntos.Domain.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;

namespace TesteJuntos.Test
{

    [TestClass]
    public class AccountTest : BaseTest
    {

        private AccountController _accountController { get; set; }
        public AccountTest() : base()
        {
            var _appService = _serviceProvider.GetService<IAccountService>();
            _accountController = new AccountController(_appService);
        }
        [TestMethod]
        [TestCategory("Login")]
        public async Task ValidLogin()
        {
            dynamic resut = await _accountController.Login(new LoginAccountCommand() { User = "teste@teste.com", Password = "102030-aB" });
            Assert.IsTrue(resut.success);
        }
    }
}
