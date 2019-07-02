using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroAuctionApp.DAL.Helpers
{
    public static class DbHelper
    {
        private static Dictionary<string, string> marketPairs;

        private static Dictionary<string, string> currencyPairs;

        static DbHelper()
        {
            marketPairs = new Dictionary<string, string>()
            {
                //NCSA
                {"CM","CME" },

                {"NQ","NASDAQ" },
                {"NY","NYSE" },
                {"AM","AMEX" },

                {"TO","TSX" },
                {"VN","VENTURE" },

                {"BZ","BOVESPA" },

                {"PK","OTC Pink" },

                //EMEA
                {"AS","AMSTERDAM" },
                {"BR","BRUSSELS" },
                {"PA","PARIS" },
                {"LS","LISBON" },

                {"MI","MILAN" },
                {"DE","XETRA" },

                {"CH","Switzerland" },

                {"MA","Madrid" },
                {"VI","Vienna" },

                {"DU","Dublin" },
                {"LO","LSE" },

                {"CO","COPENHAGEN" },
                {"HE","HELSINKI" },
                {"ST","STOCKHOLM" },
                {"OL","OSLO" },

                {"RU","Mosco" },
                {"TR","Istanbul" },
                {"ZA","Johannesburg" },

                //APAC
                {"CN","CNSX (Pure)" },
                {"HK","HongKong" },
                {"SG","Singapore" },
                {"KR","Korea" },
                {"ID","Indonesia" },
                {"TH","Thailand" },
                {"MY","Malaysia" },

                {"AX","Australia" },
                {"NZ","New Zealand" },

                //FUTURES
                {"FX","Forex" },
                {"JF","Osaka Futures" },
                {"HF","Hong Kong Futures" },
                {"SF","SGX Futures" },
            };

            currencyPairs = new Dictionary<string, string>()
            {
                //NCSA
                {"CM","USD" },

                {"NQ","USD" },
                {"NY","USD" },
                {"AM","USD" },

                {"TO","CAD" },
                {"VN","CAD" },

                {"BZ","BRL" },

                {"PK","USD" },

                //EMEA
                {"AS","EUR" },
                {"BR","EUR" },
                {"PA","EUR" },
                {"LS","EUR" },

                {"MI","EUR" },
                {"DE","EUR" },

                {"CH","CHF" },

                {"MA","EUR" },
                {"VI","EUR" },

                {"DU","EUR" },
                {"LO","GBP" },

                {"CO","DKK" },
                {"HE","EUR" },
                {"ST","SEK" },
                {"OL","NOK" },

                {"RU","RUB" },
                {"TR","TRY" },
                {"ZA","ZAC" },

                //APAC
                {"CN","CNSX (Pure)" },
                {"HK","HKD" },
                {"SG","SGD" },
                {"KR","KRW" },
                {"ID","IDR" },
                {"TH","THB" },
                {"MY","MYR" },

                {"AX","AUD" },
                {"NZ","NZD" },

                //FUTURES
                {"FX","Forex" },
                {"JF","JPY" },
                {"HF","HKD" },
                {"SF","SGD" },
            };
        }
        
        public static string GetMarketNameByCode(string code)
        {
            if (marketPairs.ContainsKey(code))
            {
                return marketPairs[code];
            }
            else
            {
                return "Unknown Market";
            }
        }

        public static string GetCurrencyByCode(string code)
        {
            if (currencyPairs.ContainsKey(code))
            {
                return currencyPairs[code];
            }
            else
            {
                return "Unknown Currency";
            }
        }
    }
}