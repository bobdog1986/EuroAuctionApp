using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using System.Threading;
using System.Reactive.Linq;   

namespace EuroAuctionApp.Infra.Services
{
    public static class AppSettingHelper
    {
        async public static Task<T> TryGetSettingByKey<T>(string key)
        {
            var result = default(T);

            try
            {
                result = await BlobCache.UserAccount.GetObject<T>(key);
            }
            catch (KeyNotFoundException ex)
            {
                ;
            }

            return result;
        }

        async public static Task<string> TryGetSettingByKey(string key)
        {
            string result = string.Empty;

            try
            {
                result = await BlobCache.UserAccount.GetObject<string>(key);
            }
            catch (KeyNotFoundException ex)
            {
                ;
            }

            return result;
        }

        async public static Task TryInsertSetting(string key,string value)
        {
            try
            {
                await BlobCache.UserAccount.InsertObject(key, value);
            }
            catch (Exception ex)
            {
                ;
            }
        }
             
    }
}
