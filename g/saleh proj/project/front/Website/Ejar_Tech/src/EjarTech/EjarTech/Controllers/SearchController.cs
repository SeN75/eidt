using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EjarTech.Services.DatabaseServices.Connection.Helpers;
using EjarTech.Services.AuthServices.Helpers;
using EjarTech.Models.ConfigurationModel.Database;
using EjarTech.Services.LanguageServies.Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EjarTech.Models.DatabaseModels;
using EjarTech.Models.SearchModels;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;

namespace EjarTech.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUserProvider _userProvider;
        private readonly IDatabaseConnection _databaseProvider;
        private readonly ITranslationProvider _translation;
        private readonly DatabaseOptions _databaseOptions;

        public SearchController(IUserProvider userProvider, IDatabaseConnection databaseConnection, ITranslationProvider translation, IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions.Value;
            _userProvider = userProvider;
            _databaseProvider = databaseConnection;
            _translation = translation;
        }

        public IActionResult Index(string page = "1")
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CarRentSearch(SearchCarForRent searchData)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home", searchData);
            IMongoDatabase database = await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName);
            IMongoCollection<CarForRent> carsForRent = database.GetCollection<CarForRent>("cr_car_for_rent");
            List<CarForRent> cars = await (await carsForRent.FindAsync(items => items.CompanyName == searchData.CarCompany && items.CarType == searchData.CarType && items.IsActiive == true && items.ModelYear == searchData.ModelYear)).ToListAsync();
            foreach (CarForRent car in cars)
            {
                if (car.OrdersDates == null)
                    continue;
                foreach (var date in car.OrdersDates)
                {
                    if (date.IsConflictWith(searchData.PickupDate, searchData.DropOffDate))
                    {
                        cars.Remove(car);
                        break;
                    }
                }
            }
            ViewCarsRent sc = new ViewCarsRent()
            {
               From = DateTime.Parse(searchData.PickupDate),
               To = DateTime.Parse(searchData.DropOffDate),
               Cars = cars
            };

            Response.Cookies.Append("_fd", searchData.PickupDate, new CookieOptions() { Expires = DateTime.Now.AddMinutes(30) });
            Response.Cookies.Append("_td", searchData.DropOffDate, new CookieOptions() { Expires = DateTime.Now.AddMinutes(30) });

            return View(sc);
        }

        public async Task<IActionResult> ViewReservationInfo(string carId)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission != "User" || user.IsBlocked)
                return RedirectToAction("Index", "Home");
            if (!ObjectId.TryParse(carId, out ObjectId objId))
                return RedirectToAction("Index", "Home");
            if (!Request.Cookies.ContainsKey("_td") || !Request.Cookies.ContainsKey("_fd"))
                return RedirectToAction("Index", "Home");
            if (!DateTime.TryParse(Request.Cookies["_fd"], out DateTime fromDate) || !DateTime.TryParse(Request.Cookies["_td"], out DateTime toDate))
                return RedirectToAction("Index", "Home");
            IMongoDatabase database = await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName);
            IMongoCollection<CarForRent> carsForRent = database.GetCollection<CarForRent>("cr_car_for_rent");
            CarForRent cfr = await (await carsForRent.FindAsync(c => c._id == objId)).FirstOrDefaultAsync();
            if (cfr == null)
                return RedirectToAction("Index", "Home");
            int days = Convert.ToInt32((toDate - fromDate).TotalDays);
            decimal prc;
            if (days < 7)
                prc = days * cfr.PricePerDay;
            else if (days < 30)
                prc = days * cfr.PricePerWeek;
            else
                prc = days * cfr.PricePerMonth;
            Branch branch = await (await database.GetCollection<Branch>("cr_branch").FindAsync(c => c._id == cfr.BranchId)).FirstOrDefaultAsync();
            if(branch == null)
                return RedirectToAction("Index", "Home");
            ViewReservationInfo vModle = new ViewReservationInfo()
            {
                TotalPrice = prc,
                Car = cfr,
                To = toDate,
                From = fromDate,
                Days = days,
                Office = branch
            };
            Response.Cookies.Append("_ccc", objId.ToString(), new CookieOptions() { Expires = DateTime.Now.AddMinutes(30)});
            return View(vModle);
        }

        public async Task<IActionResult> ConfirmReservation()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission != "User" || user.IsBlocked)
                return RedirectToAction("Index", "Home");
            if (!Request.Cookies.ContainsKey("_ccc") || !Request.Cookies.ContainsKey("_td") || !Request.Cookies.ContainsKey("_fd"))
                return RedirectToAction("Index", "Home");
            if (!ObjectId.TryParse(Request.Cookies["_ccc"], out ObjectId objId))
                return RedirectToAction("Index", "Home");
            if (!DateTime.TryParse(Request.Cookies["_fd"], out DateTime fromDate) || !DateTime.TryParse(Request.Cookies["_td"], out DateTime toDate))
                return RedirectToAction("Index", "Home");
            IMongoDatabase database = await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName);
            IMongoCollection<CarForRent> carsForRent = database.GetCollection<CarForRent>("cr_car_for_rent");
            CarForRent cfr = await (await carsForRent.FindAsync(c => c._id == objId)).FirstOrDefaultAsync();
            if (cfr == null)
                return RedirectToAction("Index", "Home");
            int days = Convert.ToInt32((toDate - fromDate).TotalDays);
            decimal prc;
            if (days < 7)
                prc = days * cfr.PricePerDay;
            else if (days < 30)
                prc = days * cfr.PricePerWeek;
            else
                prc = days * cfr.PricePerMonth;
            Reservation res = new Reservation
            {
                BranchId = cfr.BranchId,
                CarId = cfr._id,
                From = fromDate,
                To = toDate,
                Price = prc,
                States = "R",
                UserId = user._id
            };
            Response.Cookies.Delete("_fd");
            Response.Cookies.Delete("_td");
            Response.Cookies.Delete("_ccc");
            await database.GetCollection<Reservation>("cr_reserv").InsertOneAsync(res);
            return View();
        }
    }
}