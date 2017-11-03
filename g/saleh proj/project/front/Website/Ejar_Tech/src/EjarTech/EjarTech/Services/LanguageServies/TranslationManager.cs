using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using EjarTech.Services.LanguageServies.Helpers;
using System.IO;

namespace EjarTech.Services.LanguageServies
{
    public class TranslationManager : ITranslationProvider
    {
        private readonly string[] _supportedLanguage = new string[]
            { "en","ar" };
        private readonly JObject _source;
        public TranslationManager(IHttpContextAccessor contextAccessor)
        {
            HttpContext context = contextAccessor.HttpContext;
            if (context.Request.Cookies.ContainsKey("_lang"))
                if (!string.IsNullOrWhiteSpace(context.Request.Cookies["_lang"]))
                    if (_supportedLanguage.Contains(context.Request.Cookies["_lang"]))
                    {
                        _source = JObject.Parse(File.ReadAllText($"Resources/lang/{context.Request.Cookies["_lang"]}.json"));
                        return;
                    }
            context.Response.Cookies.Append("_lang", "ar", new CookieOptions() { Expires = DateTime.Now.AddDays(365) });
            _source = JObject.Parse(File.ReadAllText("Resources/lang/ar.json"));
        }
        public JToken this[string Key] => _source[Key];
    }
}
