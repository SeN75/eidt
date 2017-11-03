using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EjarTech.Services.AuthServices.Helpers;
using EjarTech.Services.DatabaseServices.Connection.Helpers;
using EjarTech.Models.AccountsModel;
using Microsoft.Extensions.Options;
using EjarTech.Models.ConfigurationModel.Database;
using MongoDB.Driver;
using EjarTech.Models.DatabaseModels;
using MongoDB.Bson;
using EjarTech.StaticOperation;
using Microsoft.AspNetCore.Http;
using EjarTech.Services.LanguageServies.Helpers;
using EjarTech.Services.MessageService.Helpers;

namespace EjarTech.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ISmsService _sms;
        private readonly IUserProvider _userProvider;
        private readonly IDatabaseConnection _database;
        private readonly DatabaseOptions _databaseOptions;
        private readonly ITranslationProvider _translation;

        public AccountsController(IUserProvider userProvider, IDatabaseConnection database, IOptions<DatabaseOptions> databaseOptions, ITranslationProvider translate, ISmsService sms)
        {
            _sms = sms;
            _userProvider = userProvider;
            _database = database;
            _databaseOptions = databaseOptions.Value;
            _translation = translate;
        }

        public async Task<IActionResult> Index()
        {
            if (await _userProvider.GetUserAsync() == null)
                return View();
            else
                return View();
        }

        public async Task<IActionResult> Login()
        {
            if (await _userProvider.GetUserAsync() == null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Phone", "Password")]Login userData)
        {
            if (await _userProvider.GetUserAsync() != null)
                return RedirectToAction("Index");
            if (ModelState.IsValid == false)
                return View(userData);
            IMongoDatabase database = await _database.GetDatabaseAsync(_databaseOptions.DatabaseName);
            User user = await (await database.GetCollection<User>("cr_users").FindAsync(usr => usr.PhoneNumber == userData.Phone && usr.Password == EncriptServices.Md5Encript(userData.Password))).FirstOrDefaultAsync();
            if(user == null)
            {
                ViewData["note"] = _translation["error_login"];
                return View(userData);
            }
            if (user.IsBlocked)
            {
                ViewData["note"] = _translation["u_blocked"];
                return View(userData);
            }

            user.TempCode = ObjectId.GenerateNewId();
            IMongoCollection<User> users = database.GetCollection<User>("cr_users");
            while (users.Find(usr => usr.TempCode == user.TempCode).Count() != 0)
                user.TempCode = ObjectId.GenerateNewId();
            var filter = Builders<User>.Filter.Eq(usr => usr._id, user._id);
            await users.ReplaceOneAsync(filter, user);
            Response.Cookies.Append("_uid", user.TempCode.ToString(), new CookieOptions() { Expires = DateTime.Now.AddYears(1)});
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> VirifyMobile()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login");
            if (user.Permission != "User")
                return RedirectToAction("Index", "Dashbord");
            if (user.VirifyPhoneNumber)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VirifyMobile([Bind("Code")] string Code)
        {
            User user = await _userProvider.GetUserAsync();
            if (await _userProvider.GetUserAsync() == null)
                return RedirectToAction("Login");
            if (user.Permission != "User")
                return RedirectToAction("Index", "Dashbord");
            Code = Code.Trim();
            if (ModelState.IsValid == false || Code.Length != 6)
                return View();
            if (user.VirifyPhoneNumber)
                return RedirectToAction("Index", "Home");
            IMongoDatabase database = await _database.GetDatabaseAsync(_databaseOptions.DatabaseName);
            IMongoCollection<VirifyMobile> virifyCollection = database.GetCollection<VirifyMobile>("cr_virify_mobile");
            IMongoCollection<User> usersCollection = database.GetCollection<User>("cr_users");
            VirifyMobile  virifyUser = await virifyCollection.Find(viri => viri.UserId == user._id).FirstOrDefaultAsync();
            if (virifyUser == null)
            {
                ViewData["warning"] = _translation["no_data"];
                return View();
            }
            if(virifyUser.Code != Code)
            {
                ViewData["warning"] = _translation["code_not_match"];
                return View();
            }
            await usersCollection.UpdateOneAsync(Builders<User>.Filter.Eq(usr => usr._id, user._id), Builders<User>.Update.Set(field => field.VirifyPhoneNumber, true));
            await virifyCollection.UpdateOneAsync(Builders<VirifyMobile>.Filter.Eq(obj => obj._id, virifyUser._id), Builders<VirifyMobile>.Update.Set(obj => obj.IsUsed, true));
            ViewData["msg"] = _translation["code_done"];
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (await _userProvider.GetUserAsync() != null)
                return RedirectToAction("Index");
            else
                return View();
        }

        [HttpPost]
        public async Task<IActionResult>Register([Bind("FirstName,LastName,Password,RePassword,EMail,BirthDay,PhoneNumber")]Register userData)
        {
            if (await _userProvider.GetUserAsync() != null)
                return RedirectToAction("Index");
            if (ModelState.IsValid == false)
                return View(userData);
            if(userData.Password != userData.RePassword)
            {
                ViewData["extra"] = "كلمة المرور غير متطابقة - Password it's dosn't match";
                return View(userData);
            }
            IMongoDatabase database = await _database.GetDatabaseAsync(_databaseOptions.DatabaseName);
            IMongoCollection<User> users = database.GetCollection<User>("cr_users");
            if (users.Find(usr => usr.EMail == userData.EMail || usr.PhoneNumber == userData.PhoneNumber).Count() != 0)
            {
                ViewData["note"] = _translation["there_account"];
                return View("Login");
            }
            ObjectId objId = ObjectId.GenerateNewId();
            while (users.Find(usr => usr._id == objId).Count() != 0)
            {
                objId = ObjectId.GenerateNewId();
            }
            User newInput = new User()
            {
                _id = objId,
                FullName = $"{userData.FirstName.Trim()} {userData.LastName.Trim()}",
                BirthDay = Convert.ToDateTime(userData.BirthDay),
                EMail = userData.EMail,
                Password = EncriptServices.Md5Encript(userData.Password),
                Permission = "User",
                PhoneNumber = userData.PhoneNumber.Trim(),
                VirifyPhoneNumber = false
            };
            newInput.TempCode = ObjectId.GenerateNewId();
            while (users.Find(usr => usr.TempCode == newInput.TempCode).Count() != 0)
            {
                newInput.TempCode = ObjectId.GenerateNewId();
            }
            await users.InsertOneAsync(newInput);
            IMongoCollection<VirifyMobile> virifyMobiles = database.GetCollection<VirifyMobile>("cr_virify_mobile");
            string randomCode = GenerateRandomCode(6);
            while (await virifyMobiles.Find(obj => obj.Code == randomCode).CountAsync() != 0)
            {
                randomCode = GenerateRandomCode(6);
            }
            VirifyMobile virifyInput = new VirifyMobile()
            {
                UserId = newInput._id,
                Code = randomCode,
                IsUsed = false,
                SendTime = 0
            };
            await _sms.SendSmsVirifyAsync(newInput.PhoneNumber, virifyInput.Code, 'U', userData.FirstName);
            virifyInput.SendTime++;
            await virifyMobiles.InsertOneAsync(virifyInput);
            Response.Cookies.Append("_uid", newInput.TempCode.ToString(), new CookieOptions() { Expires = DateTime.Now.AddYears(1) });
            return RedirectToAction("VirifyMobile");
        }

        public async Task<IActionResult> Logout()
        {
            if (await _userProvider.GetUserAsync() == null)
                return RedirectToAction("Index", "Home");
            Response.Cookies.Delete("_uid");
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Settings()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login");
            if (user.Permission != "User")
                return RedirectToAction("Index", "Dashbord");
            AccountsSettings settings = new AccountsSettings()
            {
                FullName = user.FullName,
                BirthDay = user.BirthDay.ToString(),
                EMail = user.EMail
            };
            if (user == null)
                return RedirectToAction("Login");
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Settings([Bind("FullName,EMail,BirthDay")] AccountsSettings accountsSettings)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login");
            if (user.Permission != "User")
                return RedirectToAction("Index", "Dashbord");
            if (!ModelState.IsValid)
                return View(accountsSettings);
            if (string.IsNullOrWhiteSpace(accountsSettings.FullName.Trim()) || string.IsNullOrWhiteSpace(accountsSettings.EMail.Trim()) || string.IsNullOrWhiteSpace(accountsSettings.BirthDay.Trim()))
                return View(accountsSettings);
            var filter = Builders<User>.Filter.Eq(fild => fild._id, user._id);
            var update = Builders<User>.Update
                .Set(fild => fild.FullName, accountsSettings.FullName)
                .Set(fild => fild.BirthDay, Convert.ToDateTime(accountsSettings.BirthDay))
                .Set(fild => fild.EMail, accountsSettings.EMail);
            IMongoCollection<User> userCollection = (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<User>("cr_users");
            await userCollection.UpdateOneAsync(filter, update);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ChangePassword()
        {
            if (await _userProvider.GetUserAsync() == null)
                return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([Bind("OldOne,NewOne,ReNewOne")] ChangePassword changePassword)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login");
            if(EncriptServices.Md5Encript(changePassword.OldOne) != user.Password)
            {
                ViewData["extra"] = _translation["error_in_old_password"];
                return View(changePassword);
            }

            var filter = Builders<User>.Filter.Eq(field => field._id, user._id);
            var update = Builders<User>.Update
                .Set(field => field.Password, EncriptServices.Md5Encript(changePassword.NewOne));
            await (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<User>("cr_users").UpdateOneAsync(filter, update);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ChangePhoneNumber()
        {
            if (await _userProvider.GetUserAsync() == null)
                return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePhoneNumber([Bind("NewNumber")] string newNumber)
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login");
            if (!ModelState.IsValid)
                return View();
            if (newNumber.Length != 12)
                return View();
            IMongoDatabase database = await _database.GetDatabaseAsync(_databaseOptions.DatabaseName);
            IMongoCollection<User> users = database.GetCollection<User>("cr_users");
            IMongoCollection<VirifyMobile> virifys = database.GetCollection<VirifyMobile>("cr_virify_mobile");
            string vCode = GenerateRandomCode(6);
            while (await virifys.Find(ver => ver.Code == vCode).CountAsync() != 0)
                vCode = GenerateRandomCode(6);
            var filterUser = Builders<User>.Filter.Eq(usr => usr._id, user._id);
            var updateUser = Builders<User>.Update.Set(usr => usr.PhoneNumber, newNumber).Set(usr => usr.VirifyPhoneNumber, false);
            var filterVirify = Builders<VirifyMobile>.Filter.Eq(field => field.UserId, user._id);
            var updateVirify = Builders<VirifyMobile>.Update.Set(field => field.IsUsed, false).Set(field => field.Code, vCode).Set(fields => fields.SendTime, 1);
            await users.UpdateOneAsync(filterUser, updateUser);
            await virifys.UpdateOneAsync(filterVirify, updateVirify);
            await _sms.SendSmsVirifyAsync(newNumber, vCode, 'U', user.FullName);
            return RedirectToAction("VirifyMobile");
        }

        public async Task<IActionResult> ReSendVirify()
        {
            User user = await _userProvider.GetUserAsync();
            if (user == null)
                return RedirectToAction("Login");
            if (user.VirifyPhoneNumber)
                return RedirectToAction("Index", "Home");
            IMongoCollection<VirifyMobile> virifyMongoCollection = (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<VirifyMobile>("cr_virify_mobile");
            VirifyMobile virifyRow = await virifyMongoCollection.Find(obj => obj.UserId == user._id).FirstOrDefaultAsync();
            if (virifyRow.SendTime > 4)
            {
                ViewData["notes"] = _translation["to_much_virify"];
                return View();
            }
            string sCode = GenerateRandomCode(6);
            while (await virifyMongoCollection.Find(item=>item.Code == sCode).CountAsync() > 0)
            {
                sCode = GenerateRandomCode(6);
            }
            var filter = Builders<VirifyMobile>.Filter.Eq(item => item._id, virifyRow._id);
            var update = Builders<VirifyMobile>.Update.Set(item => item.SendTime, ++virifyRow.SendTime).Set(item => item.Code, sCode);
            await virifyMongoCollection.UpdateOneAsync(filter, update);
            await _sms.SendSmsVirifyAsync(user.PhoneNumber, sCode, 'U', user.FullName);
            return RedirectToAction("VirifyMobile");
        }

        public async Task<IActionResult> ForgetPassword()
        {
            User user = await _userProvider.GetUserAsync();
            if (user != null)
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword([Bind("PhoneNumber,EMail")] ForgetPassword forgetData)
        {
            if (await _userProvider.GetUserAsync() != null)
                return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
                return View(forgetData);
            IMongoCollection<User> userCollection = (await _database.GetDatabaseAsync(_databaseOptions.DatabaseName)).GetCollection<User>("cr_users");
            User user = await (await userCollection.FindAsync(usr => usr.PhoneNumber == forgetData.PhoneNumber && usr.EMail == forgetData.EMail)).FirstOrDefaultAsync();
            if(user == null)
            {
                ViewData[""] = _translation["forget_error"];
                return View(forgetData);
            }
            string newPass = GenerateRandomCode(8);
            var filter = Builders<User>.Filter.Eq(us => us._id, user._id);
            var update = Builders<User>.Update.Set(us => us.Password, EncriptServices.Md5Encript(newPass));
            await userCollection.UpdateOneAsync(filter, update);
            await _sms.SendSmsForgetPassword(user.PhoneNumber, user.FullName, newPass, 'U');
            return View("Login");
        }

        private string GenerateRandomCode(int length) => Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, length);
    }
}