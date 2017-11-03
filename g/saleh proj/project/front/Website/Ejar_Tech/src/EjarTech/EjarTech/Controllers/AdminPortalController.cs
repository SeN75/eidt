using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EjarTech.Models.AdminPortalModel;
using EjarTech.Models.DatabaseModels;
using EjarTech.Services.AuthServices.Helpers;
using EjarTech.Services.DatabaseServices.Connection.Helpers;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using EjarTech.Models.ConfigurationModel.Database;
using Microsoft.AspNetCore.Http;
using EjarTech.StaticOperation;

namespace EjarTech.Controllers
{
    public class AdminPortalController : Controller
    {
        private readonly IUserProvider _userProvider;
        private readonly IDatabaseConnection _database;
        private readonly DatabaseOptions _databaseOptions;

        public AdminPortalController(IUserProvider userprovider, IDatabaseConnection database, IOptions<DatabaseOptions> databaseOptions)
        {
            _userProvider = userprovider;
            _database = database;
            _databaseOptions = databaseOptions.Value;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() == "branchmanager" && user.Permission.ToLower() == "branch")
                return RedirectToAction("Index", "Dashbord");
            if (user.Permission.ToLower() != "adminf")
                return RedirectToAction("Index", "Home");
            return View();
        }

        public async Task<IActionResult> AddCompany()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() == "branchmanager" && user.Permission.ToLower() == "branch")
                return RedirectToAction("Index", "Dashbord");
            if (user.Permission.ToLower() != "adminf")
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany([Bind("")] AddCompany newCompany)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() == "branchmanager" && user.Permission.ToLower() == "branch")
                return RedirectToAction("Index", "Dashbord");
            if (user.Permission.ToLower() != "adminf")
                return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
                return View(newCompany);
            IMongoCollection<Company> companyCollection = (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<Company>("cr_company");
            IMongoCollection<User> usersCollection = (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<User>("cr_users");
            IMongoCollection<VirifyMobile> virifyCollection = (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<VirifyMobile>("cr_virify_mobile");
            Company cmp = new Company()
            {
                CompanyId = newCompany.CompanyId,
                CompanyName = newCompany.CompanyName,
                EMail = newCompany.EMail,
                Phone1 = newCompany.PhoneNumber1,
                SupervisorName = newCompany.SupervisorName,
                StatePhone = newCompany.StatePhone,
                Phone2 = newCompany.PhoneNumber2,
                Active = true
            };
            if (await usersCollection.Find(usr => usr.EMail == cmp.EMail || (usr.PhoneNumber == cmp.Phone1 || usr.PhoneNumber == cmp.Phone2 || usr.PhoneNumber == cmp.StatePhone)).CountAsync() != 0) 
            {
                ViewData["extra"] = "Phone number or email it's already exsist - الجوال او البريد الالكتروني مستخدم بالفعل";
                return View(newCompany);
            }
            if(await companyCollection.Find(cmpny => cmpny.CompanyId == cmp.CompanyId || cmpny.CompanyName == cmp.CompanyName).CountAsync() != 0)
            {
                ViewData["extra"] = "This Company Already registerd - هذه الشركة مسجلة بالفعل";
                return View(newCompany);
            }
            User adminUser = new User()
            {
                _id = ObjectId.GenerateNewId(),
                EMail = cmp.EMail,
                Password = EncriptServices.Md5Encript(cmp.CompanyId),
                FullName = cmp.SupervisorName,
                PhoneNumber = cmp.Phone1,
                Permission = "BranchManager",
                VirifyPhoneNumber = true
            };
            while (await usersCollection.Find(usr => usr._id == adminUser._id).CountAsync() != 0)
                adminUser._id = ObjectId.GenerateNewId();
            cmp.ManagerId = adminUser._id;
            VirifyMobile virifyMobile = new VirifyMobile()
            {
                Code = GenerateRandomCode(6),
                IsUsed = true,
                UserId = adminUser._id,
                SendTime = 1
            };
            adminUser.TempCode = ObjectId.GenerateNewId();
            while (await usersCollection.Find(usrx => usrx.TempCode == adminUser.TempCode).CountAsync() != 0)
                adminUser.TempCode = ObjectId.GenerateNewId();
            while (await virifyCollection.Find(usr => usr.Code == virifyMobile.Code).CountAsync() != 0)
                virifyMobile.Code = GenerateRandomCode(6);
            await usersCollection.InsertOneAsync(adminUser);
            await companyCollection.InsertOneAsync(cmp);
            await virifyCollection.InsertOneAsync(virifyMobile);
            return RedirectToAction("ViewCompanyes");
        }

        public async Task<IActionResult> ViewCompanyes()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() == "branchmanager" && user.Permission.ToLower() == "branch")
                return RedirectToAction("Index", "Dashbord");
            if (user.Permission.ToLower() != "adminf")
                return RedirectToAction("Index", "Home");
            List<Company> companys = await (await (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<Company>("cr_company").FindAsync(fil => true)).ToListAsync();
            return View(companys);
        }

        public async Task<IActionResult> BlockAndDeBlockCompany(string id)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() == "branchmanager" && user.Permission.ToLower() == "branch")
                return RedirectToAction("Index", "Dashbord");
            if (user.Permission.ToLower() != "adminf")
                return RedirectToAction("Index", "Home");
            if (!ObjectId.TryParse(id, out ObjectId objId))
                return RedirectToAction("ViewCompanyes");
            IMongoCollection<Company> companyCollection = (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<Company>("cr_company");
            IMongoCollection<User> usersCollection = (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<User>("cr_users");
            Company cmp = await (await companyCollection.FindAsync(field => field._id == objId)).FirstOrDefaultAsync();
            if (cmp == null)
                return RedirectToAction("ViewCompanyes");
            var filter = Builders<Company>.Filter.Eq(field => field._id, cmp._id);
            var update = Builders<Company>.Update.Set(field => field.Active, !cmp.Active);
            User manager = await (await usersCollection.FindAsync(usr => usr._id == cmp.ManagerId)).FirstOrDefaultAsync();
            await usersCollection.FindOneAndUpdateAsync(usr => usr._id == manager._id, Builders<User>.Update.Set(fields => fields.IsBlocked, !manager.IsBlocked));
            await companyCollection.UpdateOneAsync(filter, update);
            return RedirectToAction("ViewCompanyes");
        }

        public async Task<IActionResult> ViewUsers()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() == "branchmanager" && user.Permission.ToLower() == "branch")
                return RedirectToAction("Index", "Dashbord");
            if (user.Permission.ToLower() != "adminf")
                return RedirectToAction("Index", "Home");
            return View(await (await (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<User>("cr_users").FindAsync(fields => fields.Permission == "User")).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> BlockAndDeBlockUser(string id)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() == "branchmanager" && user.Permission.ToLower() == "branch")
                return RedirectToAction("Index", "Dashbord");
            if (user.Permission.ToLower() != "adminf")
                return RedirectToAction("Index", "Home");
            if (!ObjectId.TryParse(id, out ObjectId objId))
                return RedirectToAction("ViewUsers");
            IMongoCollection<User> usersCollection = (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<User>("cr_users");
            User tUser = await (await usersCollection.FindAsync(field => field._id == objId)).FirstOrDefaultAsync();
            if (tUser == null || tUser.Permission != "User")
                return RedirectToAction("ViewUsers");
            var filter = Builders<User>.Filter.Eq(field => field._id, tUser._id);
            var update = Builders<User>.Update.Set(field => field.IsBlocked, !tUser.IsBlocked);
            await usersCollection.UpdateOneAsync(filter, update);
            return RedirectToAction("ViewUsers");
        }

        public async Task<IActionResult> EditCompany(string id)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() == "branchmanager" && user.Permission.ToLower() == "branch")
                return RedirectToAction("Index", "Dashbord");
            if (user.Permission.ToLower() != "adminf")
                return RedirectToAction("Index", "Home");
            if (!ObjectId.TryParse(id, out ObjectId objId))
                return RedirectToAction("ViewCompanyes");
            Company cmp = await (await (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<Company>("cr_company").FindAsync(filter => filter._id == objId)).FirstOrDefaultAsync();
            if (cmp == null)
                return RedirectToAction("ViewCompanyes");
            EditCompany editCompany = new EditCompany()
            {
                _id = cmp._id,
                CompanyId = cmp.CompanyId,
                EMail = cmp.EMail,
                SupervisorName = cmp.SupervisorName,
                StatePhone = cmp.StatePhone,
                CompanyName=cmp.CompanyName,
                PhoneNumber1 = cmp.Phone1,
                PhoneNumber2 = cmp.Phone2
            };
            Response.Cookies.Append("c_id", editCompany._id.ToString(), new CookieOptions() { Expires = DateTime.Now.AddMinutes(10) });
            return View(editCompany);
        }

        [HttpPost]
        public async Task<IActionResult> EditCompany([Bind("")] EditCompany company)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Accounts");
            if (user.Permission.ToLower() == "user")
                return RedirectToAction("Index", "Home");
            if (user.Permission.ToLower() == "branchmanager" && user.Permission.ToLower() == "branch")
                return RedirectToAction("Index", "Dashbord");
            if (user.Permission.ToLower() != "adminf")
                return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
                return View(company);
            if (Request.Cookies.ContainsKey("c_id") == false)
                return RedirectToAction("ViewCompanyes");
            if(!ObjectId.TryParse(Request.Cookies["c_id"], out ObjectId cId))
                return RedirectToAction("ViewCompanyes");
            var filter = Builders<Company>.Filter.Eq(fields => fields._id, cId);
            var update = Builders<Company>.Update
                .Set(fields => fields.CompanyName, company.CompanyName)
                .Set(fields => fields.EMail, company.EMail)
                .Set(fields => fields.CompanyId, company.CompanyId)
                .Set(fields => fields.Phone1, company.PhoneNumber1)
                .Set(fields => fields.Phone2, company.PhoneNumber2)
                .Set(fields => fields.StatePhone, company.StatePhone)
                .Set(fields => fields.SupervisorName, company.SupervisorName);
            Response.Cookies.Delete("c_id");
            await (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<Company>("cr_company").UpdateOneAsync(filter, update);
            return RedirectToAction("ViewCompanyes");
        }

        private string GenerateRandomCode(int length) => Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, length);
    }
}