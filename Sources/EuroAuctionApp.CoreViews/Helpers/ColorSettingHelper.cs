using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Constants;
using EuroAuctionApp.Infra.Helpers;


namespace EuroAuctionApp.CoreViews.Helpers
{
    public static class ColorSettingHelper
    {
        public static Dictionary<string, object> ValuePairs = new Dictionary<string, object>();

        public static async void UpdateProfitSettings()
        {
            try
            {
                var up3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpProfit3ValueKey);
                var up2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpProfit2ValueKey);
                var up = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpProfitValueKey);
                var down = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownProfitValueKey);
                var down2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownProfit2ValueKey);
                var down3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownProfit3ValueKey);

                var upcolor3 = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpProfit3ColorKey);
                var upcolor2 = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpProfit2ColorKey);
                var upcolor = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpProfitColorKey);
                var downcolor = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownProfitColorKey);
                var downcolor2 = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownProfit2ColorKey);
                var downcolor3 = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownProfit3ColorKey);
                var normal = await AppSettingHelper.TryGetSettingAsync(KeyNames.NormalProfitColorKey);

                AddOrUpdateValue(KeyNames.UpProfit3ValueKey, up3);
                AddOrUpdateValue(KeyNames.UpProfit2ValueKey, up2);
                AddOrUpdateValue(KeyNames.UpProfitValueKey, up);
                AddOrUpdateValue(KeyNames.DownProfitValueKey, down);
                AddOrUpdateValue(KeyNames.DownProfit2ValueKey, down2);
                AddOrUpdateValue(KeyNames.DownProfit3ValueKey, down3);

                AddOrUpdateValue(KeyNames.UpProfit3ColorKey, upcolor3);
                AddOrUpdateValue(KeyNames.UpProfit2ColorKey, upcolor2);
                AddOrUpdateValue(KeyNames.UpProfitColorKey, upcolor);
                AddOrUpdateValue(KeyNames.DownProfitColorKey, downcolor);
                AddOrUpdateValue(KeyNames.DownProfit2ColorKey, downcolor2);
                AddOrUpdateValue(KeyNames.DownProfit3ColorKey, downcolor3);
                AddOrUpdateValue(KeyNames.NormalProfitColorKey, normal);
            }
            catch (Exception ex)
            {

            }
        }

        public static async void UpdateAuctionSettings()
        {
            try
            {
                var up3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpAuction3ValueKey);
                var up2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpAuction2ValueKey);
                var up = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpAuctionValueKey);
                var down = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownAuctionValueKey);
                var down2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownAuction2ValueKey);
                var down3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownAuction3ValueKey);

                var upcolor3 = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpAuction3ColorKey);
                var upcolor2 = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpAuction2ColorKey);
                var upcolor = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpAuctionColorKey);
                var downcolor = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownAuctionColorKey);
                var downcolor2 = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownAuction2ColorKey);
                var downcolor3 = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownAuction3ColorKey);
                var normal = await AppSettingHelper.TryGetSettingAsync(KeyNames.NormalAuctionColorKey);

                AddOrUpdateValue(KeyNames.UpAuction3ValueKey, up3);
                AddOrUpdateValue(KeyNames.UpAuction2ValueKey, up2);
                AddOrUpdateValue(KeyNames.UpAuctionValueKey, up);
                AddOrUpdateValue(KeyNames.DownAuctionValueKey, down);
                AddOrUpdateValue(KeyNames.DownAuction2ValueKey, down2);
                AddOrUpdateValue(KeyNames.DownAuction3ValueKey, down3);

                AddOrUpdateValue(KeyNames.UpAuction3ColorKey, upcolor3);
                AddOrUpdateValue(KeyNames.UpAuction2ColorKey, upcolor2);
                AddOrUpdateValue(KeyNames.UpAuctionColorKey, upcolor);
                AddOrUpdateValue(KeyNames.DownAuctionColorKey, downcolor);
                AddOrUpdateValue(KeyNames.DownAuction2ColorKey, downcolor2);
                AddOrUpdateValue(KeyNames.DownAuction3ColorKey, downcolor3);
                AddOrUpdateValue(KeyNames.NormalAuctionColorKey, normal);
            }
            catch (Exception ex)
            {

            }
        }

        private static void AddOrUpdateValue<T>(string key, T value)
        {
            if (ValuePairs.ContainsKey(key))
            {
                ValuePairs[key] = value;
            }
            else
            {
                ValuePairs.Add(key, value);
            }
        }
    }
}
