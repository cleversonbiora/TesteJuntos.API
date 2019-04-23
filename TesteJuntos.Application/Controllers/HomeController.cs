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

namespace TesteJuntos.Application.Controllers
{
    public class HomeController : Controller
    {
        [Route(""), HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public RedirectResult RedirectToSwaggerUi()
        {
            return Redirect("/swagger/");
        }
    }
}
