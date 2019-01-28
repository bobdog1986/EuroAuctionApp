using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Globalization;
using System.IO;

namespace EuroAuctionApp.CoreViews.Models
{
    public class QuoteFileModel:BindableBase
    {
        private static string prefix = "QuoteBoardExport_";
        //2019-01-26_00-32-37
        private static string format = "yyyy-MM-dd_HH-mm-ss";
        public QuoteFileModel(string filename)
        {
            FileName = filename;
            ParseShortName();
            DateTime=ParseDateTime(filename);
            ParseFirstSymbol();
        }

        private string firstSymbol;
        public string FirstSymbol
        {
            get { return firstSymbol; }
            set { SetProperty(ref firstSymbol, value); }
        }

        private string firstMarket;
        public string FirstMarket
        {
            get { return firstMarket; }
            set { SetProperty(ref firstMarket, value); }
        }

        private string filename;
        public string FileName
        {
            get { return filename; }
            set { SetProperty(ref filename, value); }
        }

        private string shortName;
        public string ShortName
        {
            get { return shortName; }
            set { SetProperty(ref shortName, value); }
        }

        private DateTime dateTime;
        public DateTime DateTime
        {
            get { return dateTime; }
            set { SetProperty(ref dateTime, value); }
        }

        private bool is25File;
        public bool Is25File
        {
            get { return is25File; }
            set
            {
                SetProperty(ref is25File, value);
            }
        }

        private bool isCloseFile;
        public bool IsCloseFile
        {
            get { return isCloseFile; }
            set
            {
                SetProperty(ref isCloseFile, value);
            }
        }

        private bool isAuctionFile;
        public bool IsAuctionFile
        {
            get { return isAuctionFile; }
            set
            {
                SetProperty(ref isAuctionFile, value);
            }
        }

        void ParseShortName()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                ShortName = Path.GetFileName(FileName);
            }
        }
        public static DateTime ParseDateTime(string filename)
        {
            string shortName = Path.GetFileName(filename);
            if (!string.IsNullOrEmpty(shortName))
            {
                string timeString = shortName.Substring(prefix.Length, format.Length);
                return DateTime.ParseExact(timeString, format, CultureInfo.InvariantCulture);
            }
            else
            {
                return DateTime.Now;
            }
        }

        void ParseFirstSymbol()
        {
            var lines = File.ReadAllLines(FileName);
            if (lines != null && lines.Length > 1)
            {
                var cells= lines[1].Split(',');
                if (cells != null && cells.Length > 0)
                {
                    FirstSymbol = cells[0];
                    FirstMarket = FirstSymbol.Split('.')[1];
                }
            }
        }
    }
}
