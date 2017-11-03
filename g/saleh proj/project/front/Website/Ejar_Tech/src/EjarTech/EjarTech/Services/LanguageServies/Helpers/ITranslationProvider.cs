using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace EjarTech.Services.LanguageServies.Helpers
{
    public interface ITranslationProvider
    {
        JToken this[string Key] { get; }
    }
}
