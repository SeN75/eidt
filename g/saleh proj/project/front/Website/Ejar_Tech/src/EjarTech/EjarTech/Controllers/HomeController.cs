using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EjarTech.Services.DatabaseServices.Connection.Helpers;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using EjarTech.Models.ConfigurationModel;

namespace EjarTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMongoDatabase _database;
        private readonly MailServices _mailService;

        public HomeController(IDatabaseConnection database, IOptions<MailServices> mailService)
        {
            _mailService = mailService.Value;
        }

        public async Task<IActionResult> Index()
        {
            //await _mailService.SendVirifyMailAsync("el.tayeb.karrar1@gmail.com", "Virify Mail", $"{Request.Host.Host}:{Request.Host.Port}");
            return View();
        }
    }
}