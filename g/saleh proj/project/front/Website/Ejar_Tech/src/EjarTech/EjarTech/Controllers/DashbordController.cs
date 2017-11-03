using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EjarTech.Services.AuthServices.Helpers;
using EjarTech.Services.DatabaseServices.Connection.Helpers;
using EjarTech.Services.LanguageServies.Helpers;
using EjarTech.Models.DatabaseModels;
using EjarTech.Models.DashbordModel;
using MongoDB.Driver;
using EjarTech.Models.ConfigurationModel.Database;
using Microsoft.Extensions.Options;
using EjarTech.StaticOperation;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;

namespace EjarTech.Controllers
{
    public class DashbordController : Controller
    {
        private readonly IUserProvider _userProvider;
        private readonly IDatabaseConnection _databaseProvider;
        private readonly ITranslationProvider _translation;
        private readonly DatabaseOptions _databaseOptions;

        public DashbordController(IUserProvider userProvider, IDatabaseConnection databaseConnection, ITranslationProvider translation, IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions.Value;
            _userProvider = userProvider;
            _databaseProvider = databaseConnection;
            _translation = translation;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branchmanager" && user.Permission.ToLower() != "branch")
                return RedirectToAction("Index", "Home");
            return View();
        }

        public async Task<IActionResult> AddBranch()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branchmanager")
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBranch([Bind("")] AddBranch newBranch)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branchmanager")
                return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
                return View(newBranch);
            IMongoDatabase database = await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName);
            IMongoCollection<Company> companyCollection = database.GetCollection<Company>("cr_company");
            Company cmp = await (await companyCollection.FindAsync(comp => comp.ManagerId == user._id)).FirstOrDefaultAsync();
            if (cmp == null)
                return RedirectToAction("Index");
            IMongoCollection<Branch> branchCollection = database.GetCollection<Branch>("cr_branch");
            IMongoCollection<User> usersCollection = database.GetCollection<User>("cr_users");
            IMongoCollection<VirifyMobile> virifyCollection = database.GetCollection<VirifyMobile>("cr_virify_mobile");
            if(await usersCollection.Find(usr=>usr.PhoneNumber == newBranch.PhoneNumber || usr.EMail == newBranch.BranchEMail).CountAsync() != 0)
            {
                ViewData["extra"] = "هناك بيانات مشابهة للبيانات المدخلة - Please Check Your Input This Info Alreday Exsist";
                return View(newBranch);
            }
            Branch branch = new Branch()
            {
                Active = true,
                BranchEMail = newBranch.BranchEMail,
                BranchName = newBranch.BranchName,
                BranchSupervisor = newBranch.BranchSupervisor,
                CompanyId = cmp._id,
                Longitude = newBranch.Longitude,
                PhoneNumber = newBranch.PhoneNumber,
                Latitude = newBranch.Latitude,
                CityName = newBranch.CityName
            };
            
            User newUser = new User()
            {
                _id = ObjectId.GenerateNewId(),
                EMail = branch.BranchEMail,
                FullName = branch.BranchSupervisor,
                Password = EncriptServices.Md5Encript(newBranch.Password),
                Permission = "Branch",
                PhoneNumber = branch.PhoneNumber,
                VirifyPhoneNumber = true,
                TempCode = ObjectId.GenerateNewId()
            };
            while (await usersCollection.Find(usrx => usrx._id == newUser._id).CountAsync() != 0)
                newUser._id = ObjectId.GenerateNewId();
            branch.ManagerId = newUser._id;
            VirifyMobile virifyMobile = new VirifyMobile()
            {
                Code = GenerateRandomCode(6),
                IsUsed = true,
                SendTime = 1,
                UserId = newUser._id
            };
            while (await usersCollection.Find(usrx => usrx.TempCode == newUser.TempCode).CountAsync() != 0)
                newUser.TempCode = ObjectId.GenerateNewId();
            while (await virifyCollection.Find(usr => usr.Code == virifyMobile.Code).CountAsync() != 0)
                virifyMobile.Code = GenerateRandomCode(6);
            await branchCollection.InsertOneAsync(branch);
            await usersCollection.InsertOneAsync(newUser);
            await virifyCollection.InsertOneAsync(virifyMobile);
            return RedirectToAction("ViewBranches");
        }

        public async Task<IActionResult> ViewBranches()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branchmanager")
                return RedirectToAction("Index", "Home");
            IMongoDatabase database = await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName);
            IMongoCollection<Company> companyCollection = database.GetCollection<Company>("cr_company");
            IMongoCollection<Branch> branchCollection = database.GetCollection<Branch>("cr_branch");
            Company cmpn = await (await companyCollection.FindAsync(cmp => cmp.ManagerId == user._id)).FirstAsync();
            if (cmpn == null)
                return RedirectToAction("Index");
            List<Branch> branches = await (await branchCollection.FindAsync(brc => brc.CompanyId == cmpn._id)).ToListAsync();
            return View(branches);
        }

        public async Task<IActionResult> BlockAndDeBlockBranch(string id)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branchmanager")
                return RedirectToAction("Index", "Home");
            if (!ObjectId.TryParse(id, out ObjectId objId))
                return RedirectToAction("ViewCompanyes");
            IMongoCollection<Branch> companyCollection = (await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<Branch>("cr_branch");
            IMongoCollection<User> usersCollection = (await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<User>("cr_users");
            Branch branch = await (await companyCollection.FindAsync(field => field._id == objId)).FirstOrDefaultAsync();
            if (branch == null)
                return RedirectToAction("ViewBranches");
            var filter = Builders<Branch>.Filter.Eq(field => field._id, branch._id);
            var update = Builders<Branch>.Update.Set(field => field.Active, !branch.Active);
            User manager = await (await usersCollection.FindAsync(usr => usr._id == branch.ManagerId)).FirstOrDefaultAsync();
            await usersCollection.FindOneAndUpdateAsync(usr => usr._id == manager._id, Builders<User>.Update.Set(fields => fields.IsBlocked, !manager.IsBlocked));
            await companyCollection.UpdateOneAsync(filter, update);
            return RedirectToAction("ViewBranches");
        }

        public async Task<IActionResult> EditBranch(string id)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branchmanager")
                return RedirectToAction("Index", "Home");
            if (!ObjectId.TryParse(id, out ObjectId objId))
                return RedirectToAction("ViewBranches");
            Branch branch = await (await (await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<Branch>("cr_branch").FindAsync(filter => filter._id == objId)).FirstOrDefaultAsync();
            if (branch == null)
                return RedirectToAction("ViewBranches");
            EditBranch editBranch = new EditBranch()
            {
                BranchEMail = branch.BranchEMail,
                BranchName = branch.BranchName,
                BranchSupervisor = branch.BranchSupervisor,
                Latitude = branch.Latitude,
                Longitude = branch.Longitude
            };
            Response.Cookies.Append("b_id", branch._id.ToString(), new CookieOptions() { Expires = DateTime.Now.AddMinutes(10) });
            return View(editBranch);
        }

        [HttpPost]
        public async Task<IActionResult> EditBranch([Bind("")] EditBranch branch)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branchmanager")
                return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
                return View(branch);
            if (Request.Cookies.ContainsKey("b_id") == false)
                return RedirectToAction("ViewBranches");
            if (!ObjectId.TryParse(Request.Cookies["b_id"], out ObjectId cId))
                return RedirectToAction("ViewBranches");
            var filter = Builders<Branch>.Filter.Eq(fields => fields._id, cId);
            var update = Builders<Branch>.Update
                .Set(fields => fields.Latitude, branch.Latitude)
                .Set(fields => fields.Longitude, branch.Longitude)
                .Set(fields => fields.BranchEMail, branch.BranchEMail)
                .Set(fields => fields.BranchSupervisor, branch.BranchSupervisor)
                .Set(fields => fields.BranchName, branch.BranchName);
            Response.Cookies.Delete("b_id");
            await (await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<Branch>("cr_branch").UpdateOneAsync(filter, update);
            return RedirectToAction("ViewBranches");
        }

        public async Task<IActionResult> AddCarForRent()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branch")
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCarForRent([Bind("")] AddCarForRent data)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branch")
                return RedirectToAction("Index", "Home");
            if(!ModelState.IsValid)
                return View(data);
            if(data.BagsNumber < 1 || data.DoorsNumber < 1 || data.SeatsNumber < 1 || (data.MileNumber < 1 && data.IsOpenMile == false))
            {
                ViewData["notes"] = _translation["enter_up_min"];
                return View();
            }
            IMongoDatabase database = await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName);
            IMongoCollection<CarForRent> carsForRent = database.GetCollection<CarForRent>("cr_car_for_rent");
            Branch selectedBranch = await (await database.GetCollection<Branch>("cr_branch").FindAsync(br => br.ManagerId == user._id)).FirstOrDefaultAsync();
            if(selectedBranch == null)
            {
                ViewData["notes"] = "Un Known Error Number 444-dash";
                return View(data);
            }
            Company selectedCompany = await (await database.GetCollection<Company>("cr_company").FindAsync(cm => cm._id == selectedBranch.CompanyId)).FirstOrDefaultAsync();
            if (selectedBranch == null)
            {
                ViewData["notes"] = "Un Known Error Number 555-dash";
                return View(data);
            }
            CarForRent cfr = new CarForRent();


            cfr.BranchId = selectedBranch._id;
            cfr.CompanyId = selectedCompany._id;
            cfr.CompanyName = data.CompanyName;
            cfr.CarType = data.CarType;
            cfr.DoorNumber = data.DoorsNumber;
            cfr.GearType = data.GearType;
            cfr.Insurances = data.Insurances;
            cfr.IsActiive = true;
            cfr.IsOpenMile = data.IsOpenMile;
            cfr.MileLimit = data.MileNumber;
            cfr.ModelName = data.ModelName;
            cfr.ModelYear = data.ModelYear;
            cfr.PricePerDay = Convert.ToDecimal(data.PricePerDay.ToString());
            cfr.PricePerWeek = Convert.ToDecimal(data.PricePerWeek.ToString());
            cfr.PricePerMonth = Convert.ToDecimal(data.PricePerMonth.ToString());
            cfr.SeatsNumber = data.SeatsNumber;
            cfr.PanelText = data.CarPanelText;
            cfr.PanelNumber = data.CarPanelNumber;
            cfr.BagsNumber = data.BagsNumber;


            await carsForRent.InsertOneAsync(cfr);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ViewCars()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branch")
                return RedirectToAction("Index", "Home");
            IMongoDatabase database = await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName);
            IMongoCollection<Branch> brancheCollection = database.GetCollection<Branch>("cr_branch");
            IMongoCollection<CarForRent> carsForRent = database.GetCollection<CarForRent>("cr_car_for_rent");
            Branch brn = await (await brancheCollection.FindAsync(br => br.ManagerId == user._id)).FirstOrDefaultAsync();
            if (brn == null)
                return RedirectToAction("Index");
            return View(await (await carsForRent.FindAsync(crf => crf.BranchId == brn._id)).ToListAsync());
        }

        public async Task<IActionResult> BlockAndDeBlockCar(string id)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branch")
                return RedirectToAction("Index", "Home");
            if (!ObjectId.TryParse(id, out ObjectId objId))
                return RedirectToAction("ViewCars");
            IMongoCollection<CarForRent> carsForRent = (await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<CarForRent>("cr_car_for_rent");
            CarForRent car = await (await carsForRent.FindAsync(field => field._id == objId)).FirstOrDefaultAsync();
            if (car == null)
                return RedirectToAction("ViewCars");
            var filter = Builders<CarForRent>.Filter.Eq(field => field._id, car._id);
            var update = Builders<CarForRent>.Update.Set(field => field.IsActiive, !car.IsActiive);
            await carsForRent.UpdateOneAsync(filter, update);
            return RedirectToAction("ViewCars");
        }

        public async Task<IActionResult> EditCar(string id)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() != "branch")
                return RedirectToAction("Index", "Home");
            if (!ObjectId.TryParse(id, out ObjectId objId))
                return RedirectToAction("ViewCars");
            IMongoCollection<CarForRent> carsForRent = (await _databaseProvider.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<CarForRent>("cr_car_for_rent");
            CarForRent car = await (await carsForRent.FindAsync(field => field._id == objId)).FirstOrDefaultAsync();
            if (car == null)
                return RedirectToAction("ViewCars");
            Response.Cookies.Append("_ccd", car._id.ToString(), new CookieOptions { Expires = DateTime.Now.AddHours(1)});
            return View(car);
        }

        private string GenerateRandomCode(int length)  => Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, length);
    }
}