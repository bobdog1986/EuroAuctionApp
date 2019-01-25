using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace EuroAuctionApp.Infra.Interfaces
{
    public interface ILocalizerService
    {
        void SetLocale(string locale);

        void SetLocale(CultureInfo culture);

        string GetLocalizedValue(string key);

        T GetLocalizedValue<T>(string key);

        IEnumerable<CultureInfo> SupportedLanguages { get; }

        CultureInfo SelectedLanguage { get; set; }
    }
}
