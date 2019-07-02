using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using System.Threading;
using System.Reactive.Linq;
using EuroAuctionApp.Infra.Constants;

namespace EuroAuctionApp.Infra.Helpers
{
    public static class AppSettingHelper
    {
        async public static Task<T> TryGetSettingAsync<T>(string key)
        {
            var result = default(T);

            try
            {
                result = await BlobCache.UserAccount.GetObject<T>(key);
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (KeyNotFoundException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                if(AppDefaultSettings.ValuePairs.ContainsKey(key))
                    return (T)AppDefaultSettings.ValuePairs[key];
            }

            return result;
        }

        async public static Task<string> TryGetSettingAsync(string key)
        {
            string result = string.Empty;

            try
            {
                result = await BlobCache.UserAccount.GetObject<string>(key);
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (KeyNotFoundException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                if (AppDefaultSettings.ValuePairs.ContainsKey(key))
                    return AppDefaultSettings.ValuePairs[key].ToString();
            }

            return result;
        }

        async public static Task TryInsertSettingAsync(string key, string value)
        {
            try
            {
                await BlobCache.UserAccount.InsertObject(key, value);
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                ;
            }
        }

        async public static Task TryInsertSettingAsync<T>(string key, T value)
        {
            try
            {
                await BlobCache.UserAccount.InsertObject(key, value);
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                ;
            }
        }

    }
}
